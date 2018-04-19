using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailBee.ImapMail;
using MailBee.Mime;

namespace Mailer.Model
{
    public class Message
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
    }

    public class MessagesRange
    {
        public int From { get; set; }
        public int To { get; set; }
    }


    public class FolderExtended
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int MessagesCount { get; set; }
        public int UnreadedMessagesCount { get; set; }
        public MessagesRange Range { get; set; }
        public MailMessageCollection MailMessageCollection { get; set; }
        public FolderExtended(string name, string shortName, int messagesCount, int unreadedMessagesCount)
        {
            Name = name;
            ShortName = shortName;
            MessagesCount = messagesCount;
            UnreadedMessagesCount = unreadedMessagesCount;
            Range = new MessagesRange();
        }

        public MailMessageCollection MailMessageCollectionReversed
        {
            get
            {
                var tmp = MailMessageCollection;
                tmp.Reverse();
                return tmp;
            }
        }

    }
}
