namespace Mailer.Model
{
    public class SmtpData
    {
        public SmtpData(string address, bool useSsl, bool auth)
        {
            Address = address;
            UseSsl = useSsl;
            Auth = auth;
        }

        public string Address { get; set; }
        public bool UseSsl { get; set; }
        public bool Auth { get; set; }
    }
}