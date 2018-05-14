using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Mailer.ViewModel.Settings;
using MailBee.ImapMail;
using MailBee.SmtpMail;
using Mailer.ViewModel.Accounts;
using Mailer.ViewModel.Main;

namespace Mailer.ViewModel
{
    public class ViewModelLocator
    {
        public static SettingsViewModel SettingsViewModel { get; } = new SettingsViewModel();
        public static MainViewModel MainViewModel { get; } = new MainViewModel();
        public static AccountSetupViewModel AccountSetupViewModel { get; } = new AccountSetupViewModel();
        public static Smtp SmtpClient { get; set; } = new Smtp("MN110-7DB5B590B5C3B5D7B5F4B56BC8C8-0D68");
        public ViewModelLocator()
        {

        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}