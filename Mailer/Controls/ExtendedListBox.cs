using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Mailer.Controls
{
    public class ExtendedListBox : ListBox
    {
        public static readonly DependencyProperty LoadMoreCommandProperty =
            DependencyProperty.Register("LoadMoreCommand", typeof(ICommand), typeof(ExtendedListBox),
                new PropertyMetadata(default(ICommand)));

        private ScrollViewer _scrollViewer;

        public ExtendedListBox()
        {
            Unloaded += ExtendedListBox_Unloaded;
        }

        public ICommand LoadMoreCommand
        {
            get => (ICommand) GetValue(LoadMoreCommandProperty);
            set => SetValue(LoadMoreCommandProperty, value);
        }

        private void ExtendedListBox_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_scrollViewer != null)
                _scrollViewer.ScrollChanged -= _scrollViewer_ScrollChanged;
        }

        public override void OnApplyTemplate()
        {
            _scrollViewer = (ScrollViewer) FindElementRecursive(this, typeof(ScrollViewer));
            _scrollViewer.ScrollChanged += _scrollViewer_ScrollChanged;

            base.OnApplyTemplate();
        }

        private void _scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_scrollViewer == null || LoadMoreCommand == null)
                return;

            if (e.VerticalOffset == _scrollViewer.ScrollableHeight && e.VerticalChange != 0)
                LoadMoreCommand.Execute(null);
        }

        private UIElement FindElementRecursive(FrameworkElement parent, Type targetType)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            UIElement returnElement = null;
            if (childCount > 0)
                for (int i = 0; i < childCount; i++)
                {
                    var element = VisualTreeHelper.GetChild(parent, i);
                    if (element.GetType() == targetType)
                        return element as UIElement;
                    returnElement = FindElementRecursive(VisualTreeHelper.GetChild(parent, i) as FrameworkElement,
                        targetType);
                }

            return returnElement;
        }
    }
}