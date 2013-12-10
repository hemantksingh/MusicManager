using System.Windows.Input;

namespace MusicManager.UI.Wpf
{
    public class OkCancelPanelViewModel
    {
        public OkCancelPanelViewModel()
        {
            this.OkCommand = new DelegateCommand<object>(o => { });
            this.CancelCommand = new DelegateCommand<object>(o => { });
        }
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
    }
}