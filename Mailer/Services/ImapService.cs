using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Mailer.Domain;
using Mailer.Messages;
using Mailer.Model;
using Mailer.ViewModel;

namespace Mailer.Services
{
    public static class ImapService
    {

        public static async Task ChangeFolder(string newFolder)
        {
            await ViewModelLocator.ImapClient.SelectFolderAsync(newFolder);
        }


        public static Account Account { get; set; }
        public static async Task ImapAuth(Account account, bool newAccount, int id)
        {
            await ViewModelLocator.ImapClient.ConnectAsync(account.ImapData.Address, account.ImapData.UseSsl ? 993 : 143);
            await ViewModelLocator.ImapClient.LoginAsync(account.Email, account.Password);
            if (newAccount)
            {
                Settings.Instance.Accounts.Add(account);
                Settings.Instance.SelectedAccount = Settings.Instance.Accounts.Count - 1;
            }
            else
            {
                if (id != -1)
                {
                    Settings.Instance.Accounts[id] = account;
                    Settings.Instance.SelectedAccount = id;
                }
            }

            Account = account;
            Settings.Instance.Save();
        }

        public static void ImapLogout(int id)
        {
            if (ViewModelLocator.ImapClient.IsConnected)
                ViewModelLocator.ImapClient.Disconnect();
            Settings.Instance.Accounts.RemoveAt(id);
            Settings.Instance.SelectedAccount = Settings.Instance.Accounts.Count - 1;
            if (Settings.Instance.Accounts.Count == 0)
                Messenger.Default.Send(new NavigateToPageMessage
                {
                    Page = "/Account.AccountSetupView"
                });
            else
                Messenger.Default.Send(new NavigateToPageMessage
                {
                    Page = "/Account.AccountSelectView"
                });
        }
    }
}
