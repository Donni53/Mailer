using System.Windows;
using System.Windows.Controls;
using Mailer.ViewModel.Main;

namespace Mailer.View.Main
{
    /// <summary>
    ///     Логика взаимодействия для TextEditor.xaml
    /// </summary>
    public partial class TextEditor : Page
    {
        private readonly TextEditorViewModel _viewModel;

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