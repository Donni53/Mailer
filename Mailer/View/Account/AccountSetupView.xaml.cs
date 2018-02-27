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
using Mailer.ViewModel.Account;

namespace Mailer.View.Account
{
    /// <summary>
    /// Логика взаимодействия для AccountSettingsView.xaml
    /// </summary>
    public partial class AccountSetupView : UserControl
    {
        private AccountSetupViewModel _accountSettingsViewModel;
        public AccountSetupView()
        {
            InitializeComponent();
            _accountSettingsViewModel = new AccountSetupViewModel();
            DataContext = _accountSettingsViewModel;
        }
    }
}
