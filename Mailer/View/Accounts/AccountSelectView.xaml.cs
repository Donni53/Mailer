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
using Mailer.Controls;
using Mailer.ViewModel.Accounts;

namespace Mailer.View.Accounts
{
    /// <summary>
    /// Логика взаимодействия для AccountSelectView.xaml
    /// </summary>
    public partial class AccountSelectView : PageBase
    {
        private AccountSelectViewModel _viewModel;
        public AccountSelectView()
        {
            InitializeComponent();
            _viewModel = new AccountSelectViewModel();
            DataContext = _viewModel;
        }
    }
}
