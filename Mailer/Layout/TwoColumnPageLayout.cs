using System.Windows;

namespace Mailer.Layout
{
    public class TwoColumnPageLayout : LayoutBase
    {
        public static readonly DependencyProperty MainContentProperty = DependencyProperty.Register(
            "MainContent", typeof(object), typeof(TwoColumnPageLayout), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty RightContentProperty = DependencyProperty.Register(
            "RightContent", typeof(object), typeof(TwoColumnPageLayout), new PropertyMetadata(default(object)));

        public TwoColumnPageLayout()
        {
            Style = (Style) Application.Current.Resources["TwoColumnPageLayoutStyle"];
        }

        public object MainContent
        {
            get => GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }

        public object RightContent
        {
            get => GetValue(RightContentProperty);
            set => SetValue(RightContentProperty, value);
        }
    }
}