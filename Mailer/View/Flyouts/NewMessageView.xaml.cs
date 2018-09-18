using System.Windows;
using System.Windows.Controls;
using Mailer.ViewModel.Flyouts;

namespace Mailer.View.Flyouts
{
    /// <summary>
    ///     Логика взаимодействия для NewMessageView.xaml
    /// </summary>
    public partial class NewMessageView : UserControl
    {
        private readonly NewMessageViewModel _viewModel;

        public NewMessageView()
        {
            InitializeComponent();
            _viewModel = new NewMessageViewModel();
            DataContext = _viewModel;
        }

        private void Rtb_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
            e.Handled = false;
        }

        private void Rtb_Drop(object sender, DragEventArgs e)
        {
            _viewModel.HandleDrop(e);
        }
    }
}