using System.Windows;
using System.Windows.Controls;

namespace Mailer.Controls
{
    public class TextBoxWithHint : TextBox
    {
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register("Hint", typeof(object), typeof(TextBoxWithHint),
                new PropertyMetadata(default(object)));

        public static readonly DependencyProperty HintStyleProperty =
            DependencyProperty.Register("HintStyle", typeof(Style), typeof(TextBoxWithHint),
                new PropertyMetadata(default(Style)));

        private ContentControl _hintContent;

        public TextBoxWithHint()
        {
            DefaultStyleKey = typeof(TextBoxWithHint);
        }

        public Style HintStyle
        {
            get => (Style) GetValue(HintStyleProperty);
            set => SetValue(HintStyleProperty, value);
        }

        public object Hint
        {
            get => GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _hintContent = GetTemplateChild("HintContent") as ContentControl;
            DetermineHintVisibility();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            DetermineHintVisibility();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            DetermineHintVisibility();
            base.OnLostFocus(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            DetermineHintVisibility();
            base.OnTextChanged(e);
        }

        private void DetermineHintVisibility()
        {
            if (_hintContent != null)
                if (string.IsNullOrEmpty(Text) && !IsFocused)
                    _hintContent.Visibility = Visibility.Visible;
                else
                    _hintContent.Visibility = Visibility.Collapsed;
        }
    }
}