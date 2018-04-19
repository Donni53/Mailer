using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private int _selectedFolder;
        private bool _isMessagesLoading;

        public RelayCommand GoToSettingsCommand { get; private set; }
        public RelayCommand AddFolderCommand { get; private set; }
        public RelayCommand<FolderExtended> DeleteFolderCommand { get; private set; }

        public MailViewModel()
        {
            IsWorking = true;
            InitializeCommands();
            LoadInfo();
        }

        private void InitializeCommands()
        {
            GoToSettingsCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NavigateToPageMessage()
                {
                    Page = "/Settings.SettingsView"
                });
            });
            AddFolderCommand = new RelayCommand(CreateFolder);
            DeleteFolderCommand = new RelayCommand<FolderExtended>(DeleteFolder);
        }

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

        public MailMessageCollection MailMessageCollection => _foldersExtended == null ? null : FoldersExtended[SelectedFolder].MailMessageCollectionReversed;

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

        public async Task LoadMessages(int folderIndex/*, int from, int to*/)
        {
            try
            {
                await ViewModelLocator.ImapClient.SelectFolderAsync(FoldersExtended[folderIndex].Name);
                int msgCount = ViewModelLocator.ImapClient.MessageCount > 50 ? 50 : ViewModelLocator.ImapClient.MessageCount;
                int firstIndex = ViewModelLocator.ImapClient.MessageCount - msgCount + 1;
                int lastIndex = ViewModelLocator.ImapClient.MessageCount;
                //FoldersExtended[folderIndex].MailMessageCollection = await ViewModelLocator.ImapClient.DownloadMessageHeadersAsync(firstIndex + ":" + lastIndex, false);
                FoldersExtended[folderIndex].MailMessageCollection = await ViewModelLocator.ImapClient.DownloadMessageHeadersAsync(Imap.AllMessages, false);
                //var test = FoldersExtended[0].MailMessageCollection[0];
                RaisePropertyChanged("MailMessageCollection");
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
                    var folderExtended = new FolderExtended(item.Name, item.ShortName, folderInfo.MessageCount, folderInfo.UnseenCount);
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

    }
}
