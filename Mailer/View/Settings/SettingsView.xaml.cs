using System.Windows;
using System.Windows.Navigation;
using Mailer.Controls;
using Mailer.ViewModel;

namespace Mailer.View.Settings
{
    /// <summary>
    ///     Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : PageBase
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void SettingsFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (!(SettingsFrame.Content is FrameworkElement content))
                return;
            content.DataContext = ViewModelLocator.SettingsViewModel;
        }

        private void SettingsView_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.SettingsViewModel.Activate();
        }
    }
}