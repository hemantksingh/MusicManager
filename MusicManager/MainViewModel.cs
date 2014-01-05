using System;
using System.Windows.Input;
using MusicManager.Shared;
using MusicManager.UI.Wpf;

namespace MusicManager
{
    public class MainViewModel : NotifyPropertyChangedViewModel
    {
        private FileSelectionViewModel _fileSelection;
        private OkCancelPanelViewModel _okCancelPanel;

        public MainViewModel(IPromptService promptService, IEventAggregator eventAggregator,
                             Func<FileSelectionViewModel> fileSelectionViewModelfactory,
                             Func<OkCancelPanelViewModel> okCancelViewModelfactory)
        {
            string selectedPath = null;
            SelectFilesCommand = new DelegateCommand<object>(o =>
                {
                    selectedPath = promptService.ShowFolderBrowserDialogue();
                    FileSelection = fileSelectionViewModelfactory();
                    if (!string.IsNullOrEmpty(selectedPath))
                    {
                        FileSelection.LoadFiles(selectedPath);
                        OkCancelPanel = okCancelViewModelfactory();
                    }
                });

            eventAggregator.Subscribe<CleanUpFilesMessage>(this, message =>
                                                                 this.FileSelection.CleanUpFiles(selectedPath));
        }

        public ICommand SelectFilesCommand { get; private set; }

        public OkCancelPanelViewModel OkCancelPanel
        {
            get { return _okCancelPanel; }
            set
            {
                _okCancelPanel = value;
                OnPropertyChanged();
            }
        }

        public FileSelectionViewModel FileSelection
        {
            get { return _fileSelection; }
            set
            {
                _fileSelection = value;
                OnPropertyChanged();
            }
        }
    }
}