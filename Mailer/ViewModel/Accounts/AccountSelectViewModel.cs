using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Mailer.Messages;
using Mailer.Services;

namespace Mailer.ViewModel.Accounts
{
    public class AccountSelectViewModel : ViewModelBase
    {
        private int _selectedAccount;
        private bool _isError;
        private string _error;
        public List<Model.Account> Accounts => Domain.Settings.Instance.Accounts;
        public bool WrongFormat { get; set; }
        public bool WrongCredentials { get; set; }
        public RelayCommand OkCommand { get; private set; }
        public RelayCommand AddNewCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }

        public AccountSelectViewModel()
        {
            _selectedAccount = Domain.Settings.Instance.SelectedAccount;
            InitiailizeCommands();
        }

        private void InitiailizeCommands()
        {
            OkCommand = new RelayCommand(Ok);
            AddNewCommand = new RelayCommand(AddNew);
            EditCommand = new RelayCommand(Edit);
        }

        public int SelectedAccount
        {
            get => _selectedAccount;
            set => Set(ref _selectedAccount, value);
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

        private async void Ok()
        {
            IsWorking = true;
            IsError = false;
            try
            {
                await AccountManager.ImapAuth(
                    Domain.Settings.Instance.Accounts[SelectedAccount].UserName,
                    Domain.Settings.Instance.Accounts[SelectedAccount].ImapData,
                    false, -1);
                Domain.Settings.Instance.SelectedAccount = SelectedAccount;
                Domain.Settings.Instance.Save();
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
        }

        private void AddNew()
        {
            ViewModelLocator.AccountSetupViewModel.UserName = "";
            ViewModelLocator.AccountSetupViewModel.Login = "";
            ViewModelLocator.AccountSetupViewModel.Password = "";
            ViewModelLocator.AccountSetupViewModel.ImapServer = "";
            ViewModelLocator.AccountSetupViewModel.ImapSsl = false;
            ViewModelLocator.AccountSetupViewModel.NewAccount = true;
            ViewModelLocator.AccountSetupViewModel.Id = -1;
            Messenger.Default.Send(new NavigateToPageMessage()
            {
                Page = "/Accounts.AccountSetupView"
            });
        }

        private void Edit()
        {
            //sorry for that ((
            ViewModelLocator.AccountSetupViewModel.UserName =
                Domain.Settings.Instance.Accounts[SelectedAccount].UserName;
            ViewModelLocator.AccountSetupViewModel.Login =
                Domain.Settings.Instance.Accounts[SelectedAccount].ImapData.Login;
            ViewModelLocator.AccountSetupViewModel.Password =
                Domain.Settings.Instance.Accounts[SelectedAccount].ImapData.Password;
            ViewModelLocator.AccountSetupViewModel.ImapServer =
                Domain.Settings.Instance.Accounts[SelectedAccount].ImapData.Address;
            ViewModelLocator.AccountSetupViewModel.ImapSsl =
                Domain.Settings.Instance.Accounts[SelectedAccount].ImapData.UseSsl;
            ViewModelLocator.AccountSetupViewModel.NewAccount = false;
            ViewModelLocator.AccountSetupViewModel.Id = SelectedAccount;
            Messenger.Default.Send(new NavigateToPageMessage()
            {
                Page = "/Accounts.AccountSetupView"
            });
        }
    }
}
