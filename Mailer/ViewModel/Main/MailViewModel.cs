using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MailBee.ImapMail;
using MailBee.Mime;
using Mailer.Controls;
using Mailer.Helpers;
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

        public async Task<MailMessage> LoadEmail(Envelope envelope)
        {
            var fullMessage =
                await ViewModelLocator.ImapClient.DownloadEntireMessageAsync(Convert.ToInt64(envelope.Uid),
                    true);
            if (fullMessage.BodyPlainText == "")
                fullMessage.MakePlainBodyFromHtmlBody();
            return fullMessage;
        }

        public async void ReadEmail(Envelope envelope)
        {
            IsMessageLoading = true;
            IsMessageFormVisible = true;
            try
            {
                var message = await LoadEmail(envelope);
                Message = message;
                //envelope.Flags.AllFlags[0];
                //envelope.Flags.AllFlags.Contains()
                /*var account = Md5Helper.Md5(Domain.Settings.Instance.Accounts[Domain.Settings.Instance.SelectedAccount].ImapData.Login);

                if (DatabaseService.DatabaseExists(ViewModelLocator.databaseName))
                {
                    if (DatabaseService.TableExists(ViewModelLocator.databaseName, account))
                    {
                        //Пробуем найти запись
                        //Нашли - кастуем к MailMessage, устанавливаем
                        //Не нашли - качаем, устанавливаем, сохраняем в БД
                        //var message = await LoadEmail(envelope);
                        //Message = message;
                        DatabaseService.ExecCommand(ViewModelLocator.databaseName, $"SELECT * FROM '{account}' WHERE id = {envelope.Uid};");
                    }
                }
                else
                {
                    DatabaseService.CreateDataBase(ViewModelLocator.databaseName);
                    DatabaseService.ExecCommand(ViewModelLocator.databaseName, $"CREATE TABLE {account} (id INTEGER PRIMARY KEY, value BLOB);");
                    var message = await LoadEmail(envelope);
                    Message = message;
                    //Добавить в БД
                    DatabaseService.ExecCommand(ViewModelLocator.databaseName, $"INSERT INTO '{account}' ('id', 'value') VALUES ({message.UidOnServer}, {message});");
                }*/
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
            }
            IsMessageLoading = false;
        }

        public void CloseMessage()
        {
            IsMessageFormVisible = false;
        }
    }
}