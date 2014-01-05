using System.ComponentModel;
using System.Runtime.CompilerServices;
using MusicManager.UI.Wpf.Annotations;

namespace MusicManager.UI.Wpf
{
    public class NotifyPropertyChangedViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
