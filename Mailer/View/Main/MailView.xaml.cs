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
using Mailer.Controls;
using Mailer.ViewModel.Main;

namespace Mailer.View.Main
{
    /// <summary>
    /// Логика взаимодействия для MailView.xaml
    /// </summary>
    public partial class MailView : PageBase
    {
        private MailViewModel _viewModel;
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
