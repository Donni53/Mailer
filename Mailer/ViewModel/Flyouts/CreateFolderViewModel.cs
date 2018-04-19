using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Mailer.Controls;
using Mailer.UI.Extensions;

namespace Mailer.ViewModel.Flyouts
{
    public class CreateFolderViewModel : ViewModelBase
    {

        public string Name { get; set; }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }

        public CreateFolderViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CloseCommand = new RelayCommand(Close);
            SaveCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            try
            {
                ViewModelLocator.ImapClient.CreateFolderAsync(Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Close();
        }


        private void Close()
        {
            var flyout =
                Application.Current.MainWindow.GetVisualDescendents().FirstOrDefault(c => c is FlyoutControl) as
                    FlyoutControl;
            flyout?.Close();
        }

    }
}
