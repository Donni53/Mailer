using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Mailer.ViewModel;

namespace Mailer.Layout
{
    public class PageBase : Page
    {
        /// <summary>
        ///     Content property
        /// </summary>
        public new static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(List<object>), typeof(PageBase), new PropertyMetadata(new List<object>()));

        /// <summary>
        ///     Header property
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(PageBase), new PropertyMetadata(default(object)));

        /// <summary>
        ///     SubHeader property
        /// </summary>
        public static readonly DependencyProperty SubHeaderProperty = DependencyProperty.Register(
            "SubHeader", typeof(string), typeof(PageBase), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Header menu items property
        /// </summary>
        public static readonly DependencyProperty HeaderMenuItemsProperty = DependencyProperty.Register(
            "HeaderMenuItems", typeof(List<MenuItem>), typeof(PageBase), new PropertyMetadata(new List<MenuItem>()));

        /// <summary>
        ///     Selected tab index property
        /// </summary>
        public static readonly DependencyProperty SelectedTabIndexProperty = DependencyProperty.Register(
            "SelectedTabIndex", typeof(int), typeof(PageBase), new PropertyMetadata(default(int)));


        public PageBase()
        {
            Style = (Style) Application.Current.Resources["PageBaseStyle"];

            HeaderMenuItems = new List<MenuItem>();
            Content = new List<object>();

            NavigationContext = new NavigationContext();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        public new List<object> Content
        {
            get => (List<object>) GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public string SubHeader
        {
            get => (string) GetValue(SubHeaderProperty);
            set => SetValue(SubHeaderProperty, value);
        }

        public List<MenuItem> HeaderMenuItems
        {
            get => (List<MenuItem>) GetValue(HeaderMenuItemsProperty);
            set => SetValue(HeaderMenuItemsProperty, value);
        }

        public int SelectedTabIndex
        {
            get => (int) GetValue(SelectedTabIndexProperty);
            set => SetValue(SelectedTabIndexProperty, value);
        }

        /// <summary>
        ///     Navigation context
        /// </summary>
        public NavigationContext NavigationContext { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var vm = DataContext as ViewModelBase;
            if (vm != null)
                vm.Activate();

            OnNavigatedTo();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var vm = DataContext as ViewModelBase;
            if (vm != null)
                vm.Deactivate();

            OnNavigatedFrom();
        }

        public virtual void OnNavigatedTo()
        {
        }

        public virtual void OnNavigatedFrom()
        {
        }
    }

    public sealed class NavigationContext
    {
        private readonly Dictionary<string, object> _parameters;

        public NavigationContext()
        {
            _parameters = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Parameters
        {
            get => _parameters;
            set
            {
                if (value == null)
                    return;

                foreach (var kp in value) _parameters.Add(kp.Key, kp.Value);
            }
        }
    }
}