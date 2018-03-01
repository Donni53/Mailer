using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Mailer.ViewModel.Settings;
using MailBee.ImapMail;
using Mailer.ViewModel.Accounts;
using Mailer.ViewModel.Main;

namespace Mailer.ViewModel
{
    public class ViewModelLocator
    {
        public static SettingsViewModel SettingsViewModel { get; } = new SettingsViewModel();
        public static MainViewModel MainViewModel { get; } = new MainViewModel();
        public static AccountSetupViewModel AccountSetupViewModel { get; } = new AccountSetupViewModel();
        public static Imap ImapClient { get; set; } = new Imap();
        public ViewModelLocator()
        {

        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}