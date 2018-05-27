using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using MailBee;
using MailBee.Security;
using MailBee.SmtpMail;
using Mailer.Controls;
using Mailer.Services;
using Mailer.UI.Extensions;

namespace Mailer.ViewModel.Flyouts
{
    public class NewMessageViewModel : ViewModelBase
    {
        private string _error;
        private bool _isError;
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public RelayCommand SendMessageCommand { get; private set; }
        public RelayCommand CloseMessageCommand { get; private set; }

        public NewMessageViewModel()
        {
            InitializeCommands();
        }

        public string Error
        {
            get => _error;
            set => Set(ref _error, value);
        }

        public bool IsError
        {
            get => _isError;
            set => Set(ref _isError, value);
        }

        public void InitializeCommands()
        {
            SendMessageCommand = new RelayCommand(SendMessage);
            CloseMessageCommand = new RelayCommand(CloseMessage);
        }

        private async void SendMessage()
        {
            IsWorking = true;
            try
            {
                if (ImapSmtpService.SmtpClient.IsConnected)
                    await ImapSmtpService.SmtpClient.DisconnectAsync();
                SmtpServer server = new SmtpServer(ImapSmtpService.Account.SmtpData.Address, ImapSmtpService.Account.Email, ImapSmtpService.Account.Password);
                server.SslMode = SslStartupMode.UseStartTls;
                ImapSmtpService.SmtpClient.SmtpServers.Add(server);
                await ImapSmtpService.SmtpClient.ConnectAsync();
                await ImapSmtpService.SmtpClient.HelloAsync();
                await ImapSmtpService.SmtpClient.LoginAsync();
                ImapSmtpService.SmtpClient.From.DisplayName = ImapSmtpService.Account.UserName;
                ImapSmtpService.SmtpClient.From.Email = ImapSmtpService.Account.Email;
                ImapSmtpService.SmtpClient.To.AddFromString(To);
                ImapSmtpService.SmtpClient.Subject = Subject;
                ImapSmtpService.SmtpClient.BodyHtmlText = Message;
                await ImapSmtpService.SmtpClient.SendAsync();
                IsError = false;
                CloseMessage();
            }
            catch (Exception e)
            {
                IsError = true;
                Error = e.Message;
                LoggingService.Log(e);
            }
            IsWorking = false;
        }

        private void CloseMessage()
        {
            var flyout =
                Application.Current.MainWindow.GetVisualDescendents().FirstOrDefault(c => c is FlyoutControl) as
                    FlyoutControl;
            flyout?.Close();
        }
    }
}
