using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailBee.ImapMail;
using Mailer.Services;

namespace Mailer.ViewModel.Main
{
    public class MailViewModel : ViewModelBase
    {
        private FolderCollection _folders;

        public MailViewModel()
        {
            //IsWorking = true;
            LoadFolders();
        }

        public FolderCollection Folders
        {
            get => _folders;
            set => Set(ref _folders, value);
        }

        public async void LoadFolders()
        {
            try
            {
                Folders = await ViewModelLocator.ImapClient.DownloadFoldersAsync();
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
            }
        }

    }
}
