using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailer.Model
{
    public class Account
    {
        public string UserName { get; set; }
        public string MessagesName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ImapServer { get; set; }
        public string SmtpServer { get; set; }
        public bool ImapSsl { get; set; }
        public bool SmtpSsl { get; set; }
        public bool SmtpAuth { get; set; }

        public Account(string userName, string messagesName, string login, string password, string imapServer, string smtpServer, bool imapSsl, bool smtpSsl, bool smtpAuth)
        {
            UserName = userName;
            MessagesName = messagesName;
            Login = login;
            Password = password;
            ImapServer = imapServer;
            SmtpServer = smtpServer;
            ImapSsl = imapSsl;
            SmtpSsl = smtpSsl;
            SmtpAuth = smtpAuth;
        }
    }
}
