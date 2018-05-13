namespace Mailer.Model
{
    public class Account
    {
        public Account(string userName, string email, string password, ImapData imapData, SmtpData smtpData)
        {
            UserName = userName;
            Email = email;
            Password = password;
            ImapData = imapData;
            SmtpData = smtpData;
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ImapData ImapData { get; set; }
        public SmtpData SmtpData { get; set; }
    }
}