using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
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
        private bool _atListBottom;

        public MailViewModel()
        {
            IsWorking = true;
            InitializeCommands();
            LoadInfo();
        }


        public RelayCommand GoToSettingsCommand { get; private set; }
        public RelayCommand AddFolderCommand { get; private set; }
        public RelayCommand<FolderExtended> DeleteFolderCommand { get; private set; }
        public RelayCommand<EnvelopeWarpper> ReadEmailCommand { get; private set; }
        public RelayCommand CloseMessageCommand { get; private set; }
        public RelayCommand<int> LoadMoreCommand { get; private set; }
        public int SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                Set(ref _selectedFolder, value);
                if (FoldersExtended[_selectedFolder].EnvelopeCollection == null || FoldersExtended[_selectedFolder].EnvelopeCollection.Count == 0)
                    LoadFolderMessages(_selectedFolder);
                else
                {
                    ChangeFolder(_selectedFolder);
                    RaisePropertyChanged($"MailEnvelopeCollection");
                }
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

        public bool AtListBottom
        {
            get => _atListBottom;
            set
            {
                Set(ref _atListBottom, value);
                if (value && FoldersExtended[SelectedFolder].LastLoadedIndex > 1)
                    IsLoadMoreButtonVisible = true;
                else
                    IsLoadMoreButtonVisible = false;
            }
        }

        public ObservableCollection<EnvelopeWarpper> MailEnvelopeCollection => _foldersExtended == null
            ? null
            : FoldersExtended[SelectedFolder].EnvelopeCollection;

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
            ReadEmailCommand = new RelayCommand<EnvelopeWarpper>(ReadEmail);
            CloseMessageCommand = new RelayCommand(CloseMessage);
            LoadMoreCommand = new RelayCommand<int>(LoadFolderMessages);
        }

        private async void LoadInfo()
        {
            await LoadFolders();
            SelectedFolder = 0;
            IsWorking = false;
        }

        private async void LoadFolderMessages(int folderIndex)
        {
            IsLoadMoreButtonVisible = false;
            IsMessagesLoading = true;
            await LoadMessages(folderIndex);
            IsMessagesLoading = false;
        }

        public async void ChangeFolder(int folderIndex)
        {
            await ImapService.ChangeFolder(FoldersExtended[folderIndex].Name);
        }

        public async Task LoadMessages(int folderIndex)
        {
            try
            {
                await ViewModelLocator.ImapClient.SelectFolderAsync(FoldersExtended[folderIndex].Name);
                var msgCount = FoldersExtended[folderIndex].LastLoadedIndex > 50 ? 50 : FoldersExtended[folderIndex].LastLoadedIndex;
                if (FoldersExtended[folderIndex].EnvelopeCollection == null)
                    FoldersExtended[folderIndex].EnvelopeCollection = new ObservableCollection<EnvelopeWarpper>();
                var range = FoldersExtended[folderIndex].LastLoadedIndex + ":" + (FoldersExtended[folderIndex].LastLoadedIndex - msgCount + 1);
                var envelopes = await ViewModelLocator.ImapClient.DownloadEnvelopesAsync(range, false,
                    EnvelopeParts.BodyStructure | EnvelopeParts.MessagePreview | EnvelopeParts.InternalDate | EnvelopeParts.Flags | EnvelopeParts.Uid,
                    1000);
                FoldersExtended[folderIndex].LastLoadedIndex = FoldersExtended[folderIndex].LastLoadedIndex - msgCount + 1;

                envelopes.Reverse();

                foreach (Envelope item in envelopes)
                {
                    FoldersExtended[folderIndex].EnvelopeCollection.Add(new EnvelopeWarpper(item));
                }

                RaisePropertyChanged($"MailEnvelopeCollection");
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
            var flyout = new FlyoutControl { FlyoutContent = new CreateFolderView() };
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

        public async Task<MailMessage> LoadEmail(EnvelopeWarpper envelope)
        {
            var fullMessage =
                await ViewModelLocator.ImapClient.DownloadEntireMessageAsync(Convert.ToInt64(envelope.Uid),
                    true);
            if (fullMessage.BodyPlainText == "")
                fullMessage.MakePlainBodyFromHtmlBody();
            return fullMessage;
        }

        public async void ReadEmail(EnvelopeWarpper envelope)
        {
            /*IsMessageLoading = true;
            IsMessageFormVisible = true;
            try
            {
                var messagefilenameMd5 = Md5Helper.Md5(Domain.Settings.Instance.Accounts[Domain.Settings.Instance.SelectedAccount].Email + envelope.Uid);
                var cacheFilePath = @"Cache\" + messagefilenameMd5 + ".xml";
                if (File.Exists(cacheFilePath))
                {
                    MailMessage message = new MailMessage();
                    await message.DeserializeAsync(cacheFilePath);
                    Message = message;
                }
                else
                {
                    var message = await LoadEmail(envelope);
                    message.EncodeAllHeaders(Encoding.Default, HeaderEncodingOptions.None);
                    await message.SerializeAsync(cacheFilePath);
                    Message = message;
                }
                envelope.IsUnseen = false;
                FoldersExtended[SelectedFolder].UnreadedMessagesCount--s;
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
            }
            IsMessageLoading = false;*/
        }

        public void CloseMessage()
        {
            IsMessageFormVisible = false;
        }
    }
}