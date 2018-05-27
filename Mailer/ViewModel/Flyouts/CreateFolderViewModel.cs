using System;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Mailer.Controls;
using Mailer.Services;
using Mailer.UI.Extensions;

namespace Mailer.ViewModel.Flyouts
{
    public class CreateFolderViewModel : ViewModelBase
    {
        public CreateFolderViewModel()
        {
            InitializeCommands();
        }

        public string Name { get; set; }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand<bool> CloseCommand { get; private set; }

        private void InitializeCommands()
        {
            CloseCommand = new RelayCommand<bool>(Close);
            SaveCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            try
            {
                ImapSmtpService.ImapClient.CreateFolderAsync(Name);
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
            }
            Close(true);
        }


        private void Close(bool result)
        {
            var flyout =
                Application.Current.MainWindow.GetVisualDescendents().FirstOrDefault(c => c is FlyoutControl) as
                    FlyoutControl;
            flyout?.Close(result);
        }
    }
}