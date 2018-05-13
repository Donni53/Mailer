namespace Mailer.Model
{
    public class ImapData
    {
        public ImapData(string address, bool useSsl)
        {
            Address = address;
            UseSsl = useSsl;
        }
        public string Address { get; set; }
        public bool UseSsl { get; set; }
    }
}