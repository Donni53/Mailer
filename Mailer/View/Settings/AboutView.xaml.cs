using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mailer.View.Settings
{
    /// <summary>
    ///     Логика взаимодействия для About.xaml
    /// </summary>
    public partial class About : Page
    {
        public About()
        {
            InitializeComponent();
        }

        private void SiteLink_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/Donni53/");
        }
    }
}