using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Mailer.Messages;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mailer.ViewModel.Main
{
    public class TextEditorViewModel : ViewModelBase
    {
        public List<string> FontList { get; set; } = new List<string>()
        {
            "Arial",
            "Times New Roman",
            "Lucida Console",
            "Tahoma"
        };

        public List<string> FontSizeList { get; set; } = new List<string>()
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
        public TextEditorViewModel()
        {
            InitiailizeCommands();
            Document = new FlowDocument();
        }

        private void InitiailizeCommands()
        {
            GoToSettingsCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NavigateToPageMessage()
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
