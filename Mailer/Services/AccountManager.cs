using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using MailBee.ImapMail;
using Mailer.Domain;
using Mailer.Messages;
using Mailer.Model;
using Mailer.ViewModel;

namespace Mailer.Services
{
    public static class AccountManager
    {
        static AccountManager()
        {

        }


        public static async Task ImapAuth(string userName, ImapData imapData, bool newAccount, int id)
        {
            var imapClient = new Imap();
            await imapClient.ConnectAsync(imapData.Address, imapData.UseSsl ? 993 : 143);
            await imapClient.LoginAsync(imapData.Login, imapData.Password);
            ViewModelLocator.ImapClient = imapClient;
            if (newAccount)
            {
                Settings.Instance.Accounts.Add(new Account(userName, imapData));
                Settings.Instance.SelectedAccount = Settings.Instance.Accounts.Count - 1;
            }
            else
            {
                if (id != -1)
                {
                    Settings.Instance.Accounts[id] = new Account(userName, imapData);
                    Settings.Instance.SelectedAccount = id;
                }
            }
            Settings.Instance.Save();
        }

        public static void ImapLogout(int id)
        {
            if (ViewModelLocator.ImapClient.IsConnected)
                ViewModelLocator.ImapClient.Disconnect();
            Settings.Instance.Accounts.RemoveAt(id);
            Settings.Instance.SelectedAccount = Settings.Instance.Accounts.Count - 1;
            if (Settings.Instance.Accounts.Count == 0)
            {
                Messenger.Default.Send(new NavigateToPageMessage()
                {
                    Page = "/Account.AccountSetupView"
                });
            }
            else
            {
                Messenger.Default.Send(new NavigateToPageMessage()
                {
                    Page = "/Account.AccountSelectView"
                });
            }
        }
    }
}
