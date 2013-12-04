using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using MusicManager.Infrastructure;
using MusicManager.Shared;

namespace MusicManager.UI.Wpf
{
    public class FileSelectionViewModel
    {
        private const string Mp3FileSearchPattern = "*.mp3";

        private readonly IDirectory _directory;
        private readonly IFileCleaner _fileCleaner;

        public FileSelectionViewModel(IDirectory directory, IFileCleaner fileCleaner, string selectedPath)
        {
            _directory = directory;
            _fileCleaner = fileCleaner;

            Files = _directory.GetFiles(
                        selectedPath, Mp3FileSearchPattern, SearchOption.AllDirectories);
            
            OkCommand = new DelegateCommand<object>(obj =>
                {
                    _fileCleaner.CleanFileProperties(Files);

                    FileInfo[] fileInfos = _directory.GetFiles(selectedPath, Mp3FileSearchPattern);
                    _fileCleaner.CleanFileNames(fileInfos);
                }, 
                canClean => !string.IsNullOrWhiteSpace(selectedPath));
            
            CancelCommand = new DelegateCommand<object>(obj => { });
        }
        
        public ICommand OkCommand { get; private set ; }
        public ICommand CancelCommand { get; private set; }

        public List<string> Files { get; private set; }
    }
}