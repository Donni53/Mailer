using Mailer.Controls;
using Mailer.ViewModel.Accounts;

namespace Mailer.View.Accounts
{
    /// <summary>
    ///     Логика взаимодействия для AccountSelectView.xaml
    /// </summary>
    public partial class AccountSelectView : PageBase
    {
        private readonly AccountSelectViewModel _viewModel;

        public AccountSelectView()
        {
            InitializeComponent();
            _viewModel = new AccountSelectViewModel();
            DataContext = _viewModel;
        }
    }
}