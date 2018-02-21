using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Mailer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private WindowState _windowState;
        public string Version { get; set; }
        public MainViewModel()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            InitiailizeCommands();
        }

        public RelayCommand CloseCommand { get; private set; }
        public RelayCommand MinimizeWindowCommand { get; private set; }
        public RelayCommand RestartCommand { get; private set; }


        public WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                if (_windowState == value)
                    return;
                _windowState = value;
                RaisePropertyChanged("WindowState");
            }
        }

        private void InitiailizeCommands()
        {
            CloseCommand = new RelayCommand(() => Application.Current.MainWindow.Close());
            MinimizeWindowCommand = new RelayCommand(Minimize);
            RestartCommand = new RelayCommand(Restart);
        }

        private void Minimize()
        {
            WindowState = WindowState.Minimized;
        }

        private void Restart()
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}