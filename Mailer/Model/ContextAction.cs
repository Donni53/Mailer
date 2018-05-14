using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using Mailer.Annotations;

namespace Mailer.Model
{
    public class ContextAction : INotifyPropertyChanged
    {
        public string FolderName { get; set; }
        public RelayCommand<string> Action { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ContextAction(string folderName, RelayCommand<string> relayCommand)
        {
            FolderName = folderName;
            Action = relayCommand;
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
