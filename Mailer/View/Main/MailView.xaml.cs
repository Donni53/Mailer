using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Mailer.Controls;
using Mailer.UI.Extensions;
using Mailer.ViewModel.Main;

namespace Mailer.View.Main
{
    /// <summary>
    ///     Логика взаимодействия для MailView.xaml
    /// </summary>
    public partial class MailView : PageBase
    {
        private readonly MailViewModel _viewModel;
        private bool needScrollToEnd = true;

        public MailView()
        {
            InitializeComponent();
            _viewModel = new MailViewModel();
            DataContext = _viewModel;
        }


        private void MessagesListBox_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = VisualTreeHelperExtensions.GetDescendantByType((ListBox)sender, typeof(ScrollViewer)) as ScrollViewer;
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                _viewModel.IsLoadMoreButtonVisible = true;
                scrollViewer.PageDown();
            }
            else
            {
                if (_viewModel.IsLoadMoreButtonVisible)
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 50);
                _viewModel.IsLoadMoreButtonVisible = false;
            }
        }
    }
}