﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            get => (FlowDocument)GetValue(DocumentProperty);

            set => SetValue(DocumentProperty, value);
        }

        public static void OnDocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RichTextBox rtb = (RichTextBox)obj;
            rtb.Document = (FlowDocument)args.NewValue;
        }
    }
}
