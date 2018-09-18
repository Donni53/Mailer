using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Mailer.Controls
{
    public class BindableRichTextBox : RichTextBox
    {
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document",
            typeof(FlowDocument), typeof(BindableRichTextBox), new FrameworkPropertyMetadata(null, OnDocumentChanged));

        public new FlowDocument Document
        {
            get => (FlowDocument) GetValue(DocumentProperty);

            set => SetValue(DocumentProperty, value);
        }

        public static void OnDocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var rtb = (RichTextBox) obj;
            rtb.Document = (FlowDocument) args.NewValue;
        }
    }
}