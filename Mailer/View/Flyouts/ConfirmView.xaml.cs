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
    /// Логика взаимодействия для ConfirmView.xaml
    /// </summary>
    public partial class ConfirmView : UserControl
    {
        private ConfirmViewModel _viewModel;
        public ConfirmView(string message)
        {
            InitializeComponent();
            _viewModel = new ConfirmViewModel(message);
            DataContext = _viewModel;
        }
    }
}
