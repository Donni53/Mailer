using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Mailer.Controls;
using Mailer.UI.Extensions;

namespace Mailer.ViewModel.Flyouts
{
    public class ConfirmViewModel : ViewModelBase
    {
        public ConfirmViewModel(string message)
        {
            Message = message;
            InitializeCommands();
        }

        public string Message { get; set; }

        public RelayCommand ApplyCommand { get; private set; }
        public RelayCommand<bool> CloseCommand { get; private set; }

        private void InitializeCommands()
        {
            ApplyCommand = new RelayCommand(Apply);
            CloseCommand = new RelayCommand<bool>(Close);
        }

        private void Apply()
        {
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