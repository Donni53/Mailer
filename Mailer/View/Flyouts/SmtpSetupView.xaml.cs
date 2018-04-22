using System.Windows.Controls;
using Mailer.Model;
using Mailer.ViewModel.Flyouts;

namespace Mailer.View.Flyouts
{
    /// <summary>
    ///     Логика взаимодействия для SmtpSetup.xaml
    /// </summary>
    public partial class SmtpSetupView : UserControl
    {
        private readonly SmtpSetupViewModel _viewModel;

        public SmtpSetupView(Account account, int id)
        {
            InitializeComponent();
            _viewModel = new SmtpSetupViewModel();
            _viewModel.Account = account;
            _viewModel.Id = id;
            DataContext = _viewModel;
        }
    }
}