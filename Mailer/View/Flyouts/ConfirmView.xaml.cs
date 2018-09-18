using System.Windows.Controls;
using Mailer.ViewModel.Flyouts;

namespace Mailer.View.Flyouts
{
    /// <summary>
    ///     Логика взаимодействия для ConfirmView.xaml
    /// </summary>
    public partial class ConfirmView : UserControl
    {
        private readonly ConfirmViewModel _viewModel;

        public ConfirmView(string message)
        {
            InitializeComponent();
            _viewModel = new ConfirmViewModel(message);
            DataContext = _viewModel;
        }
    }
}