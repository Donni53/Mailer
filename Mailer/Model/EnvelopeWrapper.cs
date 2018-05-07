using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MailBee.ImapMail;
using Mailer.Annotations;
using Mailer.Services;
using Mailer.ViewModel;

namespace Mailer.Model
{
    public class EnvelopeWarpper : INotifyPropertyChanged
    {
        public Envelope Envelope { get; }
        public bool IsChecked { get; set; }
        public List<string> AllFlags { get; set; }

        public EnvelopeWarpper(Envelope envelope)
        {
            Envelope = envelope;
            AllFlags = new List<string>(Envelope.Flags.AllFlags);
        }

        public string DisplayName => Envelope.MessagePreview.From.DisplayName;
        public string Subject => Envelope.MessagePreview.Subject;
        public DateTime DateReceived => Envelope.MessagePreview.DateReceived;

        public bool IsUnseen
        {
            get => AllFlags.Contains("\\Unseen");
            set
            {
                if (!value && IsUnseen)
                {
                    AllFlags[AllFlags.IndexOf("\\Unseen")] = "\\Seen";
                    SetMessageFlagsAsync(Envelope.Uid.ToString(), true, SystemMessageFlags.Seen, MessageFlagAction.Add);
                }
                OnPropertyChanged();
            }
        }

        public bool IsFlagged
        {
            get => AllFlags.Contains("\\Flagged");
            set
            {
                if (!value && IsFlagged)
                {
                    AllFlags.Remove("\\Flagged");
                    SetMessageFlagsAsync(Envelope.Uid.ToString(), true, SystemMessageFlags.Flagged, MessageFlagAction.Remove);
                }
                else
                {
                    AllFlags.Add("\\Flagged");
                    SetMessageFlagsAsync(Envelope.Uid.ToString(), true, SystemMessageFlags.Flagged, MessageFlagAction.Add);
                }
                OnPropertyChanged();
            }
        }
        public bool IsAnswered => AllFlags.Contains("\\Answered");
        public long Uid => Envelope.Uid;



        public async void SetMessageFlagsAsync(string messageIndexSet, bool indexIsUid, SystemMessageFlags systemFlags, MessageFlagAction action)
        {
            try
            {
                await ViewModelLocator.ImapClient.SetMessageFlagsAsync(messageIndexSet, indexIsUid, systemFlags, action);
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
