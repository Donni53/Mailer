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
        public bool AutoReplies { get; set; }
        public string AutoRepliesText { get; set; }
        public ImapData ImapData { get; set; }
        public SmtpData SmtpData { get; set; }

        public Account(string userName, ImapData imapData)
        {
            UserName = userName;
            ImapData = imapData;
        }
    }
}
