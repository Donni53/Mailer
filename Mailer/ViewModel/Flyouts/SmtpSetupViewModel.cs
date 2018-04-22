using System;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Mailer.Controls;
using Mailer.Model;
using Mailer.Services;
using Mailer.UI.Extensions;

namespace Mailer.ViewModel.Flyouts
{
    public class SmtpSetupViewModel : ViewModelBase
    {
        public SmtpSetupViewModel()
        {
            InitializeCommands();
        }

        public Account Account { get; set; }
        public int Id { get; set; }
        public string SmtpAddress { get; set; }
        public bool SmtpSsl { get; set; }
        public bool SmtpAuth { get; set; }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }

        private void InitializeCommands()
        {
            CloseCommand = new RelayCommand(Close);

            SaveCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            try
            {
                /*var smtp = new SmtpData(Account.ImapData.Login, Account.ImapData.Password, SmtpAddress, SmtpSsl, SmtpAuth);
                Account.SmtpData = smtp;
                Domain.Settings.Instance.Accounts[Id] = Account;
                Domain.Settings.Instance.Save();*/
            }
            catch (Exception ex)
            {
                LoggingService.Log(ex);
            }

            Close();
        }

        private void Close()
        {
            if (Application.Current.MainWindow.GetVisualDescendents().FirstOrDefault(c => c is FlyoutControl) is
                FlyoutControl flyout)
                flyout.Close();
        }
    }
}