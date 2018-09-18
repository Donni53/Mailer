using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#if DESKTOP
using System.Windows.Data;
#elif MODERN
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

#elif PHONE
using System.Windows.Data;
#endif

namespace Mailer.UI.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
#if MODERN
        public object Convert(object value, Type targetType, object parameter, string culture)
#else
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
            var invert = false;
            if (parameter != null) bool.TryParse(parameter.ToString(), out invert);
            if (value == null) return invert ? Visibility.Visible : Visibility.Collapsed;

            if (value is string)
                return string.IsNullOrWhiteSpace((string) value) || invert ? Visibility.Collapsed : Visibility.Visible;

            if (value is IList)
            {
                var empty = ((IList) value).Count == 0;
                if (invert)
                    empty = !empty;
                if (empty)
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }

            decimal number;
            if (decimal.TryParse(value.ToString(), out number))
                if (!invert)
                    return number > 0 ? Visibility.Visible : Visibility.Collapsed;
                else
                    return number > 0 ? Visibility.Collapsed : Visibility.Visible;

            return Visibility.Visible;
        }

#if MODERN
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
#else
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#endif

        {
            throw new NotImplementedException();
        }
    }
}