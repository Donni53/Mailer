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

        public RelayCommand CloseWindowCommand { get; private set; }
        public RelayCommand MinimizeWindowCommand { get; private set; }
        public RelayCommand MaximizeWindowCommand { get; private set; }
        public RelayCommand RestartCommand { get; private set; }
        public RelayCommand GoBackCommand { get; private set; }
        public RelayCommand GoToSettingsCommand { get; private set; }


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
            CloseWindowCommand = new RelayCommand(() => Application.Current.MainWindow.Close());
            MinimizeWindowCommand = new RelayCommand(() => WindowState = WindowState.Minimized);
            MaximizeWindowCommand = new RelayCommand(() => WindowState = IsWindowMaximized ? WindowState.Normal : WindowState.Maximized);
            GoToSettingsCommand = new RelayCommand(() =>
            {
                //TODO OnNavigateToPage(new NavigateToPageMessage() { Page = "/Settings.SettingsView" });
            });
            RestartCommand = new RelayCommand(Restart);
        }

        private void Minimize()
        {
            WindowState = WindowState.Minimized;
        }

        public bool IsWindowMaximized
        {
            get
            {
                return WindowState == WindowState.Maximized;
            }
        }

        private void Restart()
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}