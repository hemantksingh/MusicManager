using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MusicManager.Shared;
using MusicManager.UI.Wpf;

namespace MusicManager
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Func<string, FileSelectionViewModel> _fileSelectionViewModelfactory;
        private readonly IPromptService _promptService;
        private FileSelectionViewModel _fileSelection;

        public MainViewModel(IPromptService promptService,
                             Func<string, FileSelectionViewModel> fileSelectionViewModelfactory)
        {
            _promptService = promptService;
            _fileSelectionViewModelfactory = fileSelectionViewModelfactory;

            SelectFilesCommand = new DelegateCommand<object>(o =>
                {
                    string selectedPath = _promptService.ShowFolderBrowserDialogue();
                    if (!string.IsNullOrEmpty(selectedPath))
                    {
                        FileSelection = _fileSelectionViewModelfactory(selectedPath);
                    }
                });
        }

        public ICommand SelectFilesCommand { get; private set; }

        public FileSelectionViewModel FileSelection
        {
            get { return _fileSelection; }
            set
            {
                _fileSelection = value;
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