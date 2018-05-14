using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using MailBee.ImapMail;
using MailBee.Mime;
using Mailer.Annotations;

namespace Mailer.Model
{

    public class FolderExtended : INotifyPropertyChanged
    {
        private int _unreadedMessagesCount;
        private int _messagesCount;

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

        public int MessagesCount
        {
            get => _messagesCount;
            set
            {
                _messagesCount = value;
                OnPropertyChanged();
            } 
        }

        public int UnreadedMessagesCount
        {
            get => _unreadedMessagesCount;
            set
            {
                _unreadedMessagesCount = value;
                OnPropertyChanged();
            }
        }

        public int LastLoadedIndex { get; set; }
        public ObservableCollection<EnvelopeWarpper> EnvelopeCollection { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}