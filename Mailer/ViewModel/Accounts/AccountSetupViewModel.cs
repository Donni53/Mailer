using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Mailer.Messages;
using Mailer.Model;
using Mailer.Services;
using Mailer.UI.Extensions;

namespace Mailer.ViewModel.Accounts
{
    public class AccountSetupViewModel : ViewModelBase
    {
        private bool _isError;
        private string _error;

        public string Login { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ImapServer { get; set; }
        public bool ImapSsl { get; set; }
        public bool WrongFormat { get; set; }
        public bool WrongCredentials { get; set; }
        public bool NewAccount { get; set; }
        public int Id { get; set; }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }

        public AccountSetupViewModel()
        {
            InitializeCommands();
            NewAccount = true;
            Id = -1;
        }

        private void InitializeCommands()
        {
            CloseCommand = new RelayCommand(Close);

            SaveCommand = new RelayCommand(Save);
        }

        public bool IsError
        {
            get => _isError;
            set => Set(ref _isError, value);
        }

        public string Error
        {
            get => _error;
            set => Set(ref _error, value);
        }

        private async void Save()
        {
            IsWorking = true;
            IsError = false;
            try
            {
                var imapData = new ImapData(Login, Password, ImapServer, true);
                await AccountManager.ImapAuth(UserName, imapData, NewAccount, Id);
                Messenger.Default.Send(new NavigateToPageMessage()
                {
                    Page = "/Main.MailView"
                });
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                IsError = true;
                LoggingService.Log(ex);
            }

            IsWorking = false;
            //CloseFlyOut();
        }

        public bool CanGoBack
        {
            get
            {
                var frame = Application.Current.MainWindow.GetVisualDescendents().OfType<Frame>().FirstOrDefault();
                if (frame == null)
                    return false;
                return frame.CanGoBack;
            }
        }

        private void CloseFlyOut()
        {
            /*if (Application.Current.MainWindow.GetVisualDescendents().FirstOrDefault(c => c is FlyoutControl) is FlyoutControl flyout)
                flyout.Close();*/
        }

        private void Close()
        {
           ViewModelLocator.MainViewModel.GoBackCommand.Execute(null);
        }

    }
}
