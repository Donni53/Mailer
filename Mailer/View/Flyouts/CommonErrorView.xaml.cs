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
using Mailer.UI.Extensions;

namespace Mailer.View.Flyouts
{
    /// <summary>
    /// Interaction logic for CommonErrorView.xaml
    /// </summary>
    public partial class CommonErrorView : UserControl
    {
        public CommonErrorView()
        {
            InitializeComponent();
        }

        public CommonErrorView(string title, string description) : this()
        {
            TitleTextBlock.Text = title;
            DescriptionTextBlock.Text = description;
        }

        private void RestartButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close(true, true);
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Close(bool now = false, bool restart = false)
        {
            if (!(Application.Current.MainWindow.GetVisualDescendents().FirstOrDefault(c => c is FlyoutControl) is
                FlyoutControl flyout)) return;
            if (now)
                flyout.CloseNow(restart);
            else
                flyout.Close(restart);
        }
    }
}
