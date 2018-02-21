using System;
using System.Windows;
using System.Windows.Input;

namespace Mailer.View.Main
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            MainFrame.Navigate(new Uri("View/Settings/SettingsView.xaml", UriKind.Relative));
            //BackgroundArtControl.Effect = new BlurEffect() { RenderingBias = RenderingBias.Quality, Radius = 35 };
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void MainPage_OnContentRendered(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
