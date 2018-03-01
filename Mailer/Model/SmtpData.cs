﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailer.Model
{
    public class SmtpData
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public bool UseSsl { get; set; }
        public bool Auth { get; set; }

        public SmtpData(string login, string password, string address, bool useSsl, bool auth)
        {
            Login = login;
            Password = password;
            Address = address;
            UseSsl = useSsl;
            Auth = auth;
        }
    }
}
