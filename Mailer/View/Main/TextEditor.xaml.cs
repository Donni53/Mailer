using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mailer.ViewModel.Main;

namespace Mailer.View.Main
{
    /// <summary>
    /// Логика взаимодействия для TextEditor.xaml
    /// </summary>
    public partial class TextEditor : Page
    {
        private TextEditorViewModel _viewModel;
        public TextEditor()
        {
            InitializeComponent();
            Rtb.AddHandler(DragOverEvent, new DragEventHandler(BindableRichTextBox_DragOver), true);
            Rtb.AddHandler(DropEvent, new DragEventHandler(BindableRichTextBox_Drop), true);
            _viewModel = new TextEditorViewModel();
            DataContext = _viewModel;
        }

        private void BindableRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.UpdateSymbolsCount();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.UpdateStyle();
        }

        private void BindableRichTextBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
            e.Handled = false;
        }

        private void BindableRichTextBox_Drop(object sender, DragEventArgs e)
        {
            _viewModel.HandleDrop(e);
        }
    }
}
