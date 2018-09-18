using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.Command;
using Mailer.Annotations;

namespace Mailer.Model
{
    public class ContextAction : INotifyPropertyChanged
    {
        public ContextAction(string folderName, RelayCommand<string> relayCommand)
        {
            FolderName = folderName;
            Action = relayCommand;
        }

        public string FolderName { get; set; }
        public RelayCommand<string> Action { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}