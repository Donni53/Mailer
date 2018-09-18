using System.Windows;
using System.Windows.Navigation;
using Mailer.Controls;
using Mailer.ViewModel.Settings;

namespace Mailer.View.Settings
{
    /// <summary>
    ///     Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : PageBase
    {
        private readonly SettingsViewModel _viewModel;

        public SettingsView()
        {
            InitializeComponent();
            _viewModel = new SettingsViewModel();
            DataContext = _viewModel;
        }

        private void SettingsFrame_Navigated(object sender, NavigationEventArgs e)
        {
            SettingsFrame.RemoveBackEntry();

            if (!(SettingsFrame.Content is FrameworkElement content))
                return;

            content.DataContext = _viewModel;
        }

        private void SettingsView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Activate();
        }
    }
}