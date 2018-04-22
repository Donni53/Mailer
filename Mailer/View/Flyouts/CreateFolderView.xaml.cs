using System.Windows.Controls;
using Mailer.ViewModel.Flyouts;

namespace Mailer.View.Flyouts
{
    /// <summary>
    ///     Логика взаимодействия для CreateFolderView.xaml
    /// </summary>
    public partial class CreateFolderView : UserControl
    {
        private readonly CreateFolderViewModel _viewModel;

        public CreateFolderView()
        {
            InitializeComponent();
            _viewModel = new CreateFolderViewModel();
            DataContext = _viewModel;
        }
    }
}