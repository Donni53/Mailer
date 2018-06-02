using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using MailBee.Mime;
using Mailer.Controls;
using Mailer.Helpers;
using Mailer.Model;
using Mailer.Services;
using Mailer.UI.Extensions;

namespace Mailer.ViewModel.Flyouts
{
    public class MessageViewModel : ViewModelBase
    {
        private MailMessage _message;

        public MailMessage Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        public RelayCommand CloseMessageCommand { get; private set; }

        public MessageViewModel(EnvelopeWarpper envelope)
        {
            InitializeCommands();
            IsWorking = true;
            Message = new MailMessage();
            LoadEmailAsync(envelope);
        }


        private void InitializeCommands()
        {
            CloseMessageCommand = new RelayCommand(CloseMessage);
        }

        public async void LoadEmailAsync(EnvelopeWarpper envelope)
        {
            await LoadEmail(envelope);
            IsWorking = false;
        }

        public async Task LoadEmail(EnvelopeWarpper envelope)
        {
            try
            {
                var messagefilenameMd5 = Md5Helper.Md5(ImapSmtpService.Account.Email + envelope.Uid);
                var cacheFilePath = @"Cache\" + messagefilenameMd5 + ".xml";
                var htmlFilePath = @"Cache\" + messagefilenameMd5 + ".htm";
                if (File.Exists(cacheFilePath))
                {
                    MailMessage message = new MailMessage();
                    await message.DeserializeAsync(cacheFilePath);
                    Message = message;
                }
                else
                {
                    var message = await ImapSmtpService.ImapClient.DownloadEntireMessageAsync(Convert.ToInt64(envelope.Uid), true);
                    if (message.BodyPlainText == "")
                        message.MakePlainBodyFromHtmlBody();
                    Message = message;
                    message.EncodeAllHeaders(Encoding.Default, HeaderEncodingOptions.None);
                    if (Domain.Settings.Instance.SaveMessagesToCache)
                        await message.SerializeAsync(cacheFilePath);
                }

                if (Domain.Settings.Instance.LoadFullVersions)
                {
                    await Message.SaveHtmlAndRelatedFilesAsync(htmlFilePath);
                    Process.Start(htmlFilePath);
                }

                if (envelope.IsUnseen)
                    envelope.IsUnseen = false;
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
                CloseMessage();
            }
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
