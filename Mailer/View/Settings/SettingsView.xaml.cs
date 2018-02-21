using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mailer.ViewModel;

namespace Mailer.View.Settings
{
    /// <summary>
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Page
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void SettingsFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var content = SettingsFrame.Content as FrameworkElement;
            if (content == null)
                return;
            content.DataContext = ViewModelLocator.SettingsViewModel;
        }
    }
}
