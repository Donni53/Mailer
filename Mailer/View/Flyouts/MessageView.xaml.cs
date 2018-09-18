using System.Windows.Controls;
using Mailer.Model;
using Mailer.ViewModel.Flyouts;

namespace Mailer.View.Flyouts
{
    /// <summary>
    ///     Логика взаимодействия для MessageView.xaml
    /// </summary>
    public partial class MessageView : UserControl
    {
        private readonly MessageViewModel _viewModel;

        public MessageView(EnvelopeWarpper envelope)
        {
            InitializeComponent();
            _viewModel = new MessageViewModel(envelope);
            DataContext = _viewModel;
        }
    }
}