using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mailer.Domain;
using Mailer.Model;

namespace Mailer.Services
{
    public static class AccountManager
    {
        private static Account _account;
        public static List<Account> Accounts => Settings.Instance.Accounts;

        static AccountManager()
        {
            //_account = Accounts[Settings.Instance.SelectedAccount];
        }


        public static async Task ImapAuth(ImapData imapData, bool newAccount)
        {

        }
    }
}
