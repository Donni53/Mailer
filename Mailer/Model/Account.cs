using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailBee.ImapMail;

namespace Mailer.Model
{
    public class Account
    {
        public string UserName { get; set; }
        public bool AutoReplie { get; set; }
        public string AutoReplieText { get; set; }
        public ImapData ImapData { get; set; }
        public SmtpData SmtpData { get; set; }

        public Account(string userName, ImapData imapData)
        {
            UserName = userName;
            ImapData = imapData;
        }
    }
}
