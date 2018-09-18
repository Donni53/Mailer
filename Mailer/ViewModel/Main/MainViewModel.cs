using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using Mailer.Controls;
using Mailer.Messages;
using Mailer.UI.Extensions;

namespace Mailer.ViewModel.Main
{
    public class MainViewModel : ViewModelBase
    {
        private WindowState _windowState;

        public MainViewModel()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            InitiailizeCommands();
            InitializeMessageInterception();
        }

        public string Version { get; set; }

        public RelayCommand CloseWindowCommand { get; private set; }
        public RelayCommand MinimizeWindowCommand { get; private set; }
        public RelayCommand MaximizeWindowCommand { get; private set; }
        public RelayCommand RestartCommand { get; private set; }
        public RelayCommand GoBackCommand { get; private set; }
        public RelayCommand GoToSettingsCommand { get; private set; }

        public bool CanGoBack
        {
            get
            {
                var frame = Application.Current.MainWindow.GetVisualDescendents().OfType<Frame>().FirstOrDefault();
                return frame != null && frame.CanGoBack;
            }
        }

        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                if (_windowState == value)
                    return;
                _windowState = value;
                RaisePropertyChanged("WindowState");
            }
        }

        public bool IsWindowMaximized => WindowState == WindowState.Maximized;

        private void InitiailizeCommands()
        {
            CloseWindowCommand = new RelayCommand(() => Application.Current.MainWindow.Close());
            MinimizeWindowCommand = new RelayCommand(() => WindowState = WindowState.Minimized);
            MaximizeWindowCommand = new RelayCommand(() =>
                WindowState = IsWindowMaximized ? WindowState.Normal : WindowState.Maximized);
            GoToSettingsCommand = new RelayCommand(() =>
            {
                OnNavigateToPage(new NavigateToPageMessage {Page = "/Settings.SettingsView"});
            });
            GoBackCommand = new RelayCommand(() =>
            {
                var frame = Application.Current.MainWindow.GetVisualDescendents().OfType<Frame>()
                    .FirstOrDefault(f => f.Name == "RootFrame");
                if (frame == null)
                    return;
                if (frame.CanGoBack)
                    frame.GoBack();
                UpdateCanGoBack();
            });
            RestartCommand = new RelayCommand(Restart);
        }

        private void InitializeMessageInterception()
        {
            MessengerInstance.Register<NavigateToPageMessage>(this, OnNavigateToPage);
        }

        private void OnNavigateToPage(NavigateToPageMessage message)
        {
            var type = Type.GetType("Mailer.View." + message.Page.Substring(1), false);
            if (type == null)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                return;
            }

            var frame = Application.Current.MainWindow.GetVisualDescendents().OfType<Frame>().FirstOrDefault();
            if (frame == null)
                return;

            if (typeof(PageBase).IsAssignableFrom(type))
            {
                var page = (PageBase) Activator.CreateInstance(type);
                page.NavigationContext.Parameters = message.Parameters;
                frame.Navigate(page);
            }
            else if (typeof(PageBase).IsAssignableFrom(type))
            {
                var page = (PageBase) Activator.CreateInstance(type);
                page.NavigationContext.Parameters = message.Parameters;
                frame.Navigate(page);
            }
            else if (typeof(Page).IsAssignableFrom(type))
            {
                frame.Navigate(Activator.CreateInstance(type));
            }

            UpdateCanGoBack();
        }

        public void UpdateCanGoBack()
        {
            RaisePropertyChanged("CanGoBack");

            var frame = Application.Current.MainWindow.GetVisualDescendents().OfType<Frame>()
                .FirstOrDefault(f => f.Name == "RootFrame");
            if (frame != null && frame.Content != null)
            {
                var source = frame.Content.GetType().Name;
                //_canNavigate = false;
                //SelectedMainMenuItemIndex = _mainMenuItems.IndexOf(_mainMenuItems.FirstOrDefault(t => t.Page.EndsWith(source)));
                //_canNavigate = true;
            }
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