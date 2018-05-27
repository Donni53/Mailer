using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
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
        public FlowDocument Document { get; set; }

        public List<string> FontList { get; set; } = new List<string>
        {
            "Arial",
            "Times New Roman",
            "Lucida Console",
            "Tahoma"
        };

        public List<string> FontSizeList { get; set; } = new List<string>
        {
            "8",
            "12",
            "16",
            "32",
            "82"
        };

        public RelayCommand SendMessageCommand { get; private set; }
        public RelayCommand CloseMessageCommand { get; private set; }

        public NewMessageViewModel()
        {
            InitializeCommands();
            Document = new FlowDocument();
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

        public void HandleDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] docPath = (string[])e.Data.GetData(DataFormats.FileDrop);

            var dataFormat = DataFormats.Rtf;

            if (e.KeyStates == DragDropKeyStates.ShiftKey) dataFormat = DataFormats.Text;

            TextRange range;
            FileStream fStream;
            if (!File.Exists(docPath[0])) return;
            try
            {
                range = new TextRange(Document.ContentStart, Document.ContentEnd);
                fStream = new FileStream(docPath[0], FileMode.OpenOrCreate);
                range.Load(fStream, dataFormat);
                fStream.Close();
            }
            catch (Exception)
            {
                LoggingService.Log("File could not be opened. Make sure the file is a text file.");
            }
        }

        private async void SendMessage()
        {
            IsWorking = true;
            try
            {
                if (ImapSmtpService.SmtpClient.IsConnected)
                    await ImapSmtpService.SmtpClient.DisconnectAsync();
                ImapSmtpService.SmtpClient.SmtpServers.Clear();
                ImapSmtpService.SmtpClient.ResetMessage();
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
                ImapSmtpService.SmtpClient.BodyHtmlText = new TextRange(Document.ContentStart, Document.ContentEnd).Text;
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
