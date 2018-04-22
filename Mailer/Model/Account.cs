namespace Mailer.Model
{
    public class Account
    {
        public Account(string userName, ImapData imapData)
        {
            UserName = userName;
            ImapData = imapData;
        }

        public string UserName { get; set; }
        public bool AutoReplie { get; set; }
        public string AutoReplieText { get; set; }
        public ImapData ImapData { get; set; }
        public SmtpData SmtpData { get; set; }
    }
}