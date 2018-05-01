using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailBee.ImapMail;
using MailBee.Mime;

namespace Mailer.Model
{
    public class EnvelopeWrapper
    {
        public Envelope Envelope { get; set; }
        public string Subject => Envelope.Subject;
        public MailMessage MailMessage => Envelope.MessagePreview;
        public DateTime DateTime => Envelope.Date;
        public long Uid => Envelope.Uid;
        public bool Unseen => Envelope.Flags.AllFlags.Contains("Unseen");
        public bool Flagged => Envelope.Flags.AllFlags.Contains("Flagged");
    }
}
