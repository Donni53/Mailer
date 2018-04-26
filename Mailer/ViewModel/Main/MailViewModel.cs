using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MailBee.ImapMail;
using MailBee.Mime;
using Mailer.Controls;
using Mailer.Messages;
using Mailer.Model;
using Mailer.Services;
using Mailer.View.Flyouts;

namespace Mailer.ViewModel.Main
{
    public class MailViewModel : ViewModelBase
    {
        private FolderCollection _folders;
        private List<FolderExtended> _foldersExtended;
        private bool _isMessageFormVisible;
        private bool _isMessageLoading;
        private bool _isMessagesLoading;
        private MailMessage _message;
        private int _selectedFolder;
        private bool _isLoadMoreButtonVisible;

        public MailViewModel()
        {
            IsWorking = true;
            InitializeCommands();
            LoadInfo();
        }


        public RelayCommand GoToSettingsCommand { get; private set; }
        public RelayCommand AddFolderCommand { get; private set; }
        public RelayCommand<FolderExtended> DeleteFolderCommand { get; private set; }
        public RelayCommand<Envelope> ReadEmailCommand { get; private set; }
        public RelayCommand CloseMessageCommand { get; private set; }
        public int SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                _selectedFolder = value;
                LoadFolderMessages(_selectedFolder);
            }
        }

        public FolderCollection Folders
        {
            get => _folders;
            set => Set(ref _folders, value);
        }

        public List<FolderExtended> FoldersExtended
        {
            get => _foldersExtended;
            set => Set(ref _foldersExtended, value);
        }

        public bool IsMessagesLoading
        {
            get => _isMessagesLoading;
            set => Set(ref _isMessagesLoading, value);
        }

        public bool IsMessageFormVisible
        {
            get => _isMessageFormVisible;
            set => Set(ref _isMessageFormVisible, value);
        }

        public bool IsMessageLoading
        {
            get => _isMessageLoading;
            set => Set(ref _isMessageLoading, value);
        }

        public MailMessage Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        public bool IsLoadMoreButtonVisible
        {
            get => _isLoadMoreButtonVisible;
            set => Set(ref _isLoadMoreButtonVisible, value);
        }

        public EnvelopeCollection MailEnvelopeCollection => _foldersExtended == null
            ? null
            : FoldersExtended[SelectedFolder].EnvelopeCollectionReversed;

        private void InitializeCommands()
        {
            GoToSettingsCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NavigateToPageMessage
                {
                    Page = "/Settings.SettingsView"
                });
            });
            AddFolderCommand = new RelayCommand(CreateFolder);
            DeleteFolderCommand = new RelayCommand<FolderExtended>(DeleteFolder);
            ReadEmailCommand = new RelayCommand<Envelope>(ReadEmail);
            CloseMessageCommand = new RelayCommand(CloseMessage);
        }

        private async void LoadInfo()
        {
            await LoadFolders();
            SelectedFolder = 0;
            IsWorking = false;
        }

        private async void LoadFolderMessages(int folder)
        {
            IsMessagesLoading = true;
            await LoadMessages(folder);
            IsMessagesLoading = false;
        }

        public async Task LoadMessages(int folderIndex /*, int from, int to*/)
        {
            try
            {
                await ViewModelLocator.ImapClient.SelectFolderAsync(FoldersExtended[folderIndex].Name);
                int msgCount = ViewModelLocator.ImapClient.MessageCount > 50
                    ? 50
                    : ViewModelLocator.ImapClient.MessageCount;
                int firstIndex = ViewModelLocator.ImapClient.MessageCount - msgCount + 1;
                int lastIndex = ViewModelLocator.ImapClient.MessageCount;
                FoldersExtended[folderIndex].EnvelopeCollection =
                    await ViewModelLocator.ImapClient.DownloadEnvelopesAsync(firstIndex + ":" + lastIndex, false,
                        EnvelopeParts.BodyStructure | EnvelopeParts.MessagePreview | EnvelopeParts.InternalDate | EnvelopeParts.Flags | EnvelopeParts.Uid,
                        1000);

                RaisePropertyChanged("MailEnvelopeCollection");
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
            }
        }

        public async Task LoadFolders()
        {
            try
            {
                Folders = await ViewModelLocator.ImapClient.DownloadFoldersAsync();
                var tmpFldrs = new List<FolderExtended>();
                foreach (Folder item in Folders)
                {
                    var folderInfo = await ViewModelLocator.ImapClient.GetFolderStatusAsync(item.Name);
                    var folderExtended = new FolderExtended(item.Name, item.ShortName, folderInfo.MessageCount,
                        folderInfo.UnseenCount);
                    tmpFldrs.Add(folderExtended);
                }

                FoldersExtended = tmpFldrs;
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
            }
        }

        public void CreateFolder()
        {
            var flyout = new FlyoutControl {FlyoutContent = new CreateFolderView()};
            flyout.Show();
            flyout.Unloaded += Flyout_Unloaded;
        }

        private void Flyout_Unloaded(object sender, RoutedEventArgs e)
        {
            LoadInfo();
        }

        public async void DeleteFolder(FolderExtended folder)
        {
            await ViewModelLocator.ImapClient.DeleteFolderAsync(folder.Name);
            LoadInfo();
        }

        public async void ReadEmail(Envelope envelope)
        {
            try
            {
                string account = Domain.Settings.Instance.Accounts[Domain.Settings.Instance.SelectedAccount].ImapData.Login;
                bool recordExists = false;
                if (!File.Exists(ViewModelLocator.databaseName))
                {
                    SQLiteConnection.CreateFile(ViewModelLocator.databaseName);
                }
                var connection = new SQLiteConnection($"Data Source={ViewModelLocator.databaseName};");
                connection.Open();
                var command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;", connection);
                var reader = command.ExecuteReader();
                foreach (DbDataRecord record in reader)
                    if (account == (string) record["name"])
                    {
                        recordExists = true;
                        break;
                    }

                if (!recordExists)
                {
                    command = new SQLiteCommand($"CREATE TABLE {account} (id INTEGER PRIMARY KEY, value TEXT);", connection);
                    command.ExecuteNonQuery();
                }
                connection.Close();


                IsMessageLoading = true;
                IsMessageFormVisible = true;
                var fullMessage =
                    await ViewModelLocator.ImapClient.DownloadEntireMessageAsync(Convert.ToInt64(envelope.Uid),
                        true);
                if (fullMessage.BodyPlainText == "")
                    fullMessage.MakePlainBodyFromHtmlBody();
                Message = fullMessage;
                IsMessageLoading = false;
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
            }
        }

        public void CloseMessage()
        {
            IsMessageFormVisible = false;
        }
    }
}