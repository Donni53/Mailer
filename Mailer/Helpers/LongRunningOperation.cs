using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mailer.Helpers
{
    public class LongRunningOperation : INotifyPropertyChanged
    {
        private string _error;
        private bool _isWorking;

        public bool IsWorking
        {
            get => _isWorking;
            set
            {
                if (_isWorking == value)
                    return;

                _isWorking = value;
                OnPropertyChanged();
            }
        }

        public string Error
        {
            get => _error;
            set
            {
                if (_error == value)
                    return;

                _error = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}