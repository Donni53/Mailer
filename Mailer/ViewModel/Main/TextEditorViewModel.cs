using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Mailer.Messages;
using Microsoft.Win32;

namespace Mailer.ViewModel.Main
{
    public class TextEditorViewModel : ViewModelBase
    {
        public TextEditorViewModel()
        {
            InitiailizeCommands();
            Document = new FlowDocument();
        }

        public List<string> FontList { get; set; } = new List<string>
        {
            "Arial",
            "Times New Roman",
            "Lucida Console",
            "Tahoma"
        };

        public List<string> FontSizeList { get; set; } = new List<string>
        {
            "8",
            "12",
            "16",
            "32",
            "82"
        };

        public bool Bold { get; set; }
        public bool Underline { get; set; }
        public bool Italic { get; set; }
        public int SelectedFont { get; set; }
        public int SelectedFontSize { get; set; }
        public int SymbolsCount { get; set; }
        public FlowDocument Document { get; set; }
        public RelayCommand GoToSettingsCommand { get; private set; }
        public RelayCommand LoadCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand NewCommand { get; private set; }

        private void InitiailizeCommands()
        {
            GoToSettingsCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NavigateToPageMessage
                {
                    Page = "/Settings.SettingsView"
                });
            });
            LoadCommand = new RelayCommand(Load);
            SaveCommand = new RelayCommand(Save);
            NewCommand = new RelayCommand(New);
        }

        public void UpdateSymbolsCount()
        {
            SymbolsCount = new TextRange(Document.ContentStart, Document.ContentEnd).Text.Length;
            RaisePropertyChanged("SymbolsCount");
        }

        public void UpdateStyle()
        {
            Document.FontFamily = new FontFamily(FontList[SelectedFont]);
        }

        public void HandleDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] docPath = (string[]) e.Data.GetData(DataFormats.FileDrop);

            var dataFormat = DataFormats.Rtf;

            if (e.KeyStates == DragDropKeyStates.ShiftKey) dataFormat = DataFormats.Text;

            TextRange range;
            FileStream fStream;
            if (!File.Exists(docPath[0])) return;
            try
            {
                range = new TextRange(Document.ContentStart, Document.ContentEnd);
                fStream = new FileStream(docPath[0], FileMode.OpenOrCreate);
                range.Load(fStream, dataFormat);
                fStream.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("File could not be opened. Make sure the file is a text file.");
            }
        }

        private void New()
        {
            Document = new FlowDocument();
        }

        private void Load()
        {
            OpenFileDialog dlg = new OpenFileDialog {Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*"};
            if (dlg.ShowDialog() != true) return;
            FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
            TextRange range = new TextRange(Document.ContentStart, Document.ContentEnd);
            range.Load(fileStream, DataFormats.Rtf);
        }

        private void Save()
        {
            SaveFileDialog dlg = new SaveFileDialog {Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*"};
            if (dlg.ShowDialog() != true) return;
            FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
            TextRange range = new TextRange(Document.ContentStart, Document.ContentEnd);
            range.Save(fileStream, DataFormats.Rtf);
        }
    }
}