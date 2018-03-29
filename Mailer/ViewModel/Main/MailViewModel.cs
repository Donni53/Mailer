using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MailBee.ImapMail;
using MailBee.Mime;
using Mailer.Messages;
using Mailer.Model;
using Mailer.Services;

namespace Mailer.ViewModel.Main
{
    public class MailViewModel : ViewModelBase
    {
        private FolderCollection _folders;
        private List<FolderExtended> _foldersExtended;
        private int _selectedFolder;

        public RelayCommand GoToSettingsCommand { get; private set; }

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
        }

        private async void LoadInfo()
        {
            await LoadFolders();
            IsWorking = false;
        }

        private async void LoadFolderMessages()
        {
            await LoadMessages(_selectedFolder);
        }

        public int SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                _selectedFolder = value;
                LoadFolderMessages();
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

        public MailMessageCollection MailMessageCollection => FoldersExtended[0].MailMessageCollection;

        public async Task LoadMessages(int folderIndex)
        {
            try
            {
                await ViewModelLocator.ImapClient.SelectFolderAsync(FoldersExtended[folderIndex].Name);
                FoldersExtended[folderIndex].MailMessageCollection = await ViewModelLocator.ImapClient.DownloadMessageHeadersAsync(ViewModelLocator.ImapClient.MessageCount - 24 + ":*", false);
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


        public async Task LoadFoldersInfo()
        {
            
        }
    }
}
