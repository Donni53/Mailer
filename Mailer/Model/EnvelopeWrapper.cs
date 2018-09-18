using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MailBee.ImapMail;
using Mailer.Annotations;
using Mailer.Services;

namespace Mailer.Model
{
    public class EnvelopeWarpper : INotifyPropertyChanged
    {
        private bool _isChecked;

        public EnvelopeWarpper(Envelope envelope)
        {
            Envelope = envelope;
            AllFlags = new List<string>(Envelope.Flags.AllFlags);
        }

        public Envelope Envelope { get; }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        public List<string> AllFlags { get; set; }

        public string DisplayName => Envelope.MessagePreview.From.DisplayName;
        public string Subject => Envelope.MessagePreview.Subject;
        public DateTime DateReceived => Envelope.MessagePreview.DateReceived;

        public bool IsUnseenSilent
        {
            get => AllFlags.Contains("\\Unseen");
            set
            {
                if (!value && IsUnseen)
                    AllFlags[AllFlags.IndexOf("\\Unseen")] = "\\Seen";
                else
                    AllFlags[AllFlags.IndexOf("\\Seen")] = "\\Unseen";
                OnPropertyChanged(nameof(IsUnseen));
            }
        }

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
                else
                {
                    AllFlags[AllFlags.IndexOf("\\Seen")] = "\\Unseen";
                    SetMessageFlagsAsync(Envelope.Uid.ToString(), true, SystemMessageFlags.Seen,
                        MessageFlagAction.Remove);
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
                    SetMessageFlagsAsync(Envelope.Uid.ToString(), true, SystemMessageFlags.Flagged,
                        MessageFlagAction.Remove);
                }
                else
                {
                    AllFlags.Add("\\Flagged");
                    SetMessageFlagsAsync(Envelope.Uid.ToString(), true, SystemMessageFlags.Flagged,
                        MessageFlagAction.Add);
                }

                OnPropertyChanged();
            }
        }

        public bool IsAnswered => AllFlags.Contains("\\Answered");
        public long Uid => Envelope.Uid;

        public event PropertyChangedEventHandler PropertyChanged;


        public async void SetMessageFlagsAsync(string messageIndexSet, bool indexIsUid, SystemMessageFlags systemFlags,
            MessageFlagAction action)
        {
            try
            {
                await ImapSmtpService.ImapClient.SetMessageFlagsAsync(messageIndexSet, indexIsUid, systemFlags, action);
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}