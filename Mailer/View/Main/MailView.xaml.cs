using System.Windows.Controls;
using Mailer.Controls;
using Mailer.ViewModel.Main;

namespace Mailer.View.Main
{
    /// <summary>
    ///     Логика взаимодействия для MailView.xaml
    /// </summary>
    public partial class MailView : PageBase
    {
        private readonly MailViewModel _viewModel;

        public MailView()
        {
            InitializeComponent();
            _viewModel = new MailViewModel();
            DataContext = _viewModel;
        }

        private void LocalSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}