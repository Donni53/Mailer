using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
                _viewModel.AtListBottom = true;
                scrollViewer.PageDown();
            }
            else
            {
                if (_viewModel.AtListBottom)
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 50);
                _viewModel.AtListBottom = false;
            }
        }

        private void MarkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MarkButton.ContextMenu == null || MarkButton.ContextMenu.IsOpen) return;
            e.Handled = true;

            var mouseRightClickEvent = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right)
            {
                RoutedEvent = Mouse.MouseUpEvent,
                Source = sender,
            };
            InputManager.Current.ProcessInput(mouseRightClickEvent);
        }

        private void MoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MoveButton.ContextMenu == null || MoveButton.ContextMenu.IsOpen) return;
            e.Handled = true;

            var mouseRightClickEvent = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right)
            {
                RoutedEvent = Mouse.MouseUpEvent,
                Source = sender,
            };
            InputManager.Current.ProcessInput(mouseRightClickEvent);
        }
    }
}