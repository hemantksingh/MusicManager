using System;
using System.Windows.Input;
using MusicManager.Commands;
using MusicManager.Infrastructure.Bus;
using MusicManager.Shared;
using MusicManager.UI.Wpf;

namespace MusicManager
{
    public class MainViewModel : NotifyPropertyChangedViewModel
    {
        private readonly IBus _bus;
        private FileSelectionViewModel _fileSelection;
        private OkCancelPanelViewModel _okCancelPanel;

        public MainViewModel(IPromptService promptService, 
                             IEventAggregator eventAggregator,
                             Func<FileSelectionViewModel> fileSelectionViewModelfactory,
                             Func<OkCancelPanelViewModel> okCancelViewModelfactory, 
                             IBus bus)
        {
            _bus = bus;
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
                        if (FileSelection == null) return;
                        _bus.Publish(new CleanUpFilesCommand(selectedPath, FileSelection.Files));
                        _bus.Commit();
                    }
                    catch (UnauthorizedAccessException exception)
                    {
                        string errorMessage = string.Format("Failed to clean up files. {0} " +
                                                            "This may be due to the file being marked as 'Read Only'",
                                                            exception.Message);
                        promptService.ShowError(errorMessage);
                    }
                    ClearFileSelection();
                });

            eventAggregator.Subscribe<ClearFileSelectionMessage>(this, message =>
                                                                       ClearFileSelection());
        }

        public ICommand SelectFilesCommand { get; private set; }

        public OkCancelPanelViewModel OkCancelPanel
        {
            get { return _okCancelPanel; }
            private set
            {
                _okCancelPanel = value;
                OnPropertyChanged();
            }
        }

        public FileSelectionViewModel FileSelection
        {
            get { return _fileSelection; }
            private set
            {
                _fileSelection = value;
                OnPropertyChanged();
            }
        }

        private void ClearFileSelection()
        {
            FileSelection = null;
            OkCancelPanel = null;
        }
    }
}