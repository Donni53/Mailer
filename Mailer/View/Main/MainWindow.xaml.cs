using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;
using Mailer.Messages;
using Mailer.Services;
using Mailer.View.Accounts;
using Mailer.ViewModel;

namespace Mailer.View.Main
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _clearStack;

        public MainWindow()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || !(e.GetPosition(this).Y < 30)) return;
            if (WindowState == WindowState.Maximized)
            {
                Top = -e.GetPosition(this).Y / 2;
                WindowState = WindowState.Normal;
            }

            DragMove();
        }

        public void InitializeControls()
        {
            //BackgroundImage.Effect = new BlurEffect() { RenderingBias = RenderingBias.Quality, Radius = 35 };
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetBackground();
            RootFrame.Navigated += RootFrame_Navigated;
            RootFrame.Navigating += RootFrame_Navigating;
            if (Domain.Settings.Instance.Accounts.Count > 0)
                Messenger.Default.Send(new NavigateToPageMessage
                {
                    Page = "/Accounts.AccountSelectView"
                });
            else
                Messenger.Default.Send(new NavigateToPageMessage
                {
                    Page = "/Accounts.AccountSetupView"
                });
        }

        private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (RootFrame.Content is AccountSetupView) _clearStack = true;
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (_clearStack)
            {
                _clearStack = false;

                while (RootFrame.CanGoBack) RootFrame.RemoveBackEntry();
            }

            ViewModelLocator.MainViewModel.UpdateCanGoBack();
        }

        private void MainPage_OnContentRendered(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void SetBackground()
        {
            try
            {
                if (Domain.Settings.Instance.CustomBackground)
                    BackgroundImage.Source = new BitmapImage(new Uri(Domain.Settings.Instance.CustomBackgroundPath));
            }
            catch (Exception e)
            {
                LoggingService.Log(e);
            }
        }
    }
}