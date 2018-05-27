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
using Mailer.ViewModel.Flyouts;

namespace Mailer.View.Flyouts
{
    /// <summary>
    /// Логика взаимодействия для NewMessageView.xaml
    /// </summary>
    public partial class NewMessageView : UserControl
    {
        private NewMessageViewModel _viewModel;
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
