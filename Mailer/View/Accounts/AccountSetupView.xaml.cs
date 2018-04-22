using Mailer.Controls;
using Mailer.ViewModel;

namespace Mailer.View.Accounts
{
    /// <summary>
    ///     Логика взаимодействия для AccountSettingsView.xaml
    /// </summary>
    public partial class AccountSetupView : PageBase
    {
        public AccountSetupView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.AccountSetupViewModel;
        }
    }
}