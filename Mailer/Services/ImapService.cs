using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using MailBee.ImapMail;
using MailBee.SmtpMail;
using Mailer.Domain;
using Mailer.Messages;
using Mailer.Model;
using Mailer.ViewModel;

namespace Mailer.Services
{
    public static class ImapSmtpService
    {
        private const string Key = "MN110-7DB5B590B5C3B5D7B5F4B56BC8C8-0D68";
        public static Account Account { get; set; }
        public static Imap ImapClient { get; set; }
        public static Smtp SmtpClient { get; set; }

        static ImapSmtpService()
        {
            ImapClient = new Imap(Key);
            SmtpClient = new Smtp(Key);
        }

        public static async Task MoveMessageAsync(List<string> messages, string newFolder)
        {
            var messagesSet = messages.Aggregate("", (current, t) => current + (t + ","));
            await ImapClient.MoveMessagesAsync(messagesSet, true, newFolder);
        }

        public static async Task MoveMessageAsync(string messageUid, string newFolder)
        {
            await ImapClient.MoveMessagesAsync(messageUid, true, newFolder);
        }

        public static async Task DeleteMessageAsync(List<string> messages)
        {
            var messagesSet = messages.Aggregate("", (current, t) => current + (t + ","));
            await ImapClient.DeleteMessagesAsync(messagesSet, true);
        }

        public static async Task DeleteMessageAsync(string messageUid)
        {
            await ImapClient.DeleteMessagesAsync(messageUid, true);
        }

        public static async Task MarkMessages(List<string> messages, SystemMessageFlags systemMessageFlags, MessageFlagAction messageFlagAction)
        {
            var messagesSet = messages.Aggregate("", (current, t) => current + (t + ","));
            await ImapClient.SetMessageFlagsAsync(messagesSet, true, systemMessageFlags, messageFlagAction);
        }

        public static async Task ImapAuth(Account account, bool newAccount, int id)
        {
            var imap = new Imap(Key);

            await imap.ConnectAsync(account.ImapData.Address, account.ImapData.UseSsl ? 993 : 143);
            await imap.LoginAsync(account.Email, account.Password);
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
            ImapClient = imap;
            Settings.Instance.Save();
        }

        public static void ImapLogout(int id)
        {
            if (ImapClient.IsConnected)
                ImapClient.Disconnect();
            Settings.Instance.Accounts.RemoveAt(id);
            Settings.Instance.SelectedAccount = Settings.Instance.Accounts.Count - 1;
            if (Settings.Instance.Accounts.Count == 0)
                Messenger.Default.Send(new NavigateToPageMessage
                {
                    Page = "/Accounts.AccountSetupView"
                });
            else
                Messenger.Default.Send(new NavigateToPageMessage
                {
                    Page = "/Accounts.AccountSelectView"
                });
        }
    }
}
