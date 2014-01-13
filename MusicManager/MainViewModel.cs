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

        public MainViewModel(IPromptService promptService, 
                             IEventAggregator eventAggregator,
                             Func<FileSelectionViewModel> fileSelectionViewModelfactory,
                             Func<OkCancelPanelViewModel> okCancelViewModelfactory)
        {
            string selectedPath = null;
            SelectFilesCommand = new DelegateCommand<object>(o =>
                {
                    selectedPath = promptService.ShowFolderBrowserDialogue();
                    if (string.IsNullOrEmpty(selectedPath)) return;
                    FileSelection = fileSelectionViewModelfactory();
                    FileSelection.LoadFiles(selectedPath);
                    OkCancelPanel = okCancelViewModelfactory();
                });

            eventAggregator.Subscribe<CleanUpFilesMessage>(this, message =>
                {
                    try
                    {
                        if(FileSelection == null) return;
                        FileSelection.CleanUpFiles(selectedPath);
                    }
                    catch (UnauthorizedAccessException exception)
                    {
                        var errorMessage = string.Format("Failed to clean up files. {0} " +
                                                         "This may be due to the file being marked as 'Read Only'",
                                                          exception.Message);
                        promptService.ShowError(errorMessage);
                    }
                    ClearFileSelection();
                });

            eventAggregator.Subscribe<ClearFileSelectionMessage>(this, message =>
                ClearFileSelection());
        }

        private void ClearFileSelection()
        {
            FileSelection = null;
            OkCancelPanel = null;
        }

        public ICommand SelectFilesCommand { get; private set; }

        public OkCancelPanelViewModel OkCancelPanel
        {
            get { return _okCancelPanel; }
            private set { _okCancelPanel = value; OnPropertyChanged(); }
        }

        public FileSelectionViewModel FileSelection
        {
            get { return _fileSelection; }
            private set { _fileSelection = value; OnPropertyChanged(); }
        }
    }
}