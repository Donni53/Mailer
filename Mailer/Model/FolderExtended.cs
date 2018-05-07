using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
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


    public class FolderExtended
    {
        public FolderExtended(string name, string shortName, int messagesCount, int unreadedMessagesCount)
        {
            Name = name;
            ShortName = shortName;
            MessagesCount = messagesCount;
            LastLoadedIndex = MessagesCount;
            UnreadedMessagesCount = unreadedMessagesCount;
        }

        public string Name { get; set; }
        public string ShortName { get; set; }
        public int MessagesCount { get; set; }
        public int UnreadedMessagesCount { get; set; }
        public int LastLoadedIndex { get; set; }
        public ObservableCollection<EnvelopeWarpper> EnvelopeCollection { get; set; }

    }
}