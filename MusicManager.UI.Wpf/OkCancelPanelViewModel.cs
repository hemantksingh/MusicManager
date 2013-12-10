using System.Windows.Input;

namespace MusicManager.UI.Wpf
{
    public class OkCancelPanelViewModel
    {
        public OkCancelPanelViewModel(IEventAggregator eventAggregator)
        {
            this.OkCommand = new DelegateCommand<object>(
                o => eventAggregator.Publish(new CleanUpFilesMessage()));
            
            this.CancelCommand = new DelegateCommand<object>(o => { });
        }
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
    }
}