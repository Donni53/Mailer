using System.Collections.Generic;

namespace Mailer.Messages
{
    public class NavigateToPageMessage
    {
        public string Page { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}