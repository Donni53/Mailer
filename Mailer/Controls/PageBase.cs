using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Mailer.Controls
{
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

    public class PageBase : Page
    {
        public PageBase()
        {
            NavigationContext = new NavigationContext();

            Loaded += PageBase_Loaded;
            Unloaded += PageBase_Unloaded;
        }

        public NavigationContext NavigationContext { get; set; }

        public virtual void OnNavigatedTo()
        {
        }

        public virtual void OnNavigatedFrom()
        {
        }

        private void PageBase_Unloaded(object sender, RoutedEventArgs e)
        {
            Loaded -= PageBase_Loaded;
            Unloaded -= PageBase_Unloaded;

            OnNavigatedFrom();
        }

        private void PageBase_Loaded(object sender, RoutedEventArgs e)
        {
            OnNavigatedTo();
        }
    }
}