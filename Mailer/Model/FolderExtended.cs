﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Mailer.Annotations;

namespace Mailer.Model
{
    public class FolderExtended : INotifyPropertyChanged
    {
        private int _messagesCount;
        private int _unreadedMessagesCount;

        public FolderExtended(string name, string shortName, int messagesCount, int unreadedMessagesCount)
        {
            Name = name;
            ShortName = shortName;
            MessagesCount = messagesCount;
            LastLoadedIndex = MessagesCount;
            UnreadedMessagesCount = unreadedMessagesCount;
            EnvelopeCollection = new ObservableCollection<EnvelopeWarpper>();
        }

        public ObservableCollection<EnvelopeWarpper> EnvelopeCollection { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}