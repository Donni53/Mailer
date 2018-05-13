using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailer.Model
{
    [Serializable]
    public class Contact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Information { get; set; }

        public Contact(string name, string email, string information)
        {
            Name = name;
            Email = email;
            Information = information;
        }
    }
}
