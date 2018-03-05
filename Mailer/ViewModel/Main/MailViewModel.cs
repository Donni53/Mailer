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
using Mailer.Services;

namespace Mailer.ViewModel.Main
{
    public class MailViewModel : ViewModelBase
    {
        private FolderCollection _folders;
        public RelayCommand GoToSettingsCommand { get; private set; }

        public MailViewModel()
        {
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
        }

        public FolderCollection Folders
        {
            get => _folders;
            set => Set(ref _folders, value);
        }

        public async Task LoadFolders()
        {
            try
            {
                Folders = await ViewModelLocator.ImapClient.DownloadFoldersAsync();
                var fldr = Folders[0];
                await ViewModelLocator.ImapClient.SelectFolderAsync(Folders[0].Name);
                //var msgs = ViewModelLocator.ImapClient
                var tst =
                    (ViewModelLocator.ImapClient.MessageCount - 9).ToString() + ":*";
                //var msgs = ViewModelLocator.ImapClient.DownloadEntireMessages(tst, false);
                //var msgs = ViewModelLocator.ImapClient

            }
            catch (Exception e)
            {
                LoggingService.Log(e);
            }
        }

    }
}
