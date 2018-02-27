using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Mailer.Controls;
using Mailer.Model;
using Mailer.Services;
using Mailer.UI.Extensions;

namespace Mailer.ViewModel.Account
{
    public class AccountSetupViewModel : ViewModelBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ImapServer { get; set; }
        public bool ImapSsl { get; set; }
        public bool WrongFormat { get; set; }
        public bool WrongCredentials { get; set; }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }

        public AccountSetupViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CloseCommand = new RelayCommand(Close);

            SaveCommand = new RelayCommand(Save);
        }

        private async void Save()
        {
            IsWorking = true;
            try
            {
                var imapData = new ImapData(Login, Password, ImapServer, true);
                await AccountManager.ImapAuth(imapData, true);
                Domain.Settings.Instance.Accounts.Add(new Model.Account(UserName, imapData));
                Domain.Settings.Instance.Save();

                //Установить в локаторе ссыку на хуету

                /*if (Editing)
                {
                    Domain.Settings.Instance.Accounts[Domain.Settings.Instance.Accounts.IndexOf(Account)] =
                        new Model.Account(UserName, MessagesName, imapData, null);
                }
                else
                {
                    Domain.Settings.Instance.Accounts.Add(new Model.Account(UserName, MessagesName, imapData, null));
                    Domain.Settings.Instance.Save();
                }*/
                /*var lyricsId = await DataService.EditAudio(_track.Id, _track.OwnerId.ToString(), Title, Artist, Lyrics);
                if (lyricsId != null)
                {
                    _track.Title = Title;
                    _track.Artist = Artist;
                    if (lyricsId != "0")
                        _track.LyricsId = long.Parse(lyricsId);
                    else
                        _track.LyricsId = 0;
                    Close();
                }*/
            }
            catch (Exception ex)
            {
                LoggingService.Log(ex);
            }

            IsWorking = false;
            CloseFlyOut();
        }

        private void CloseFlyOut()
        {
            if (Application.Current.MainWindow.GetVisualDescendents().FirstOrDefault(c => c is FlyoutControl) is FlyoutControl flyout)
                flyout.Close();
        }

        private void Close()
        {
                Application.Current.Shutdown(0);
        }

    }
}
