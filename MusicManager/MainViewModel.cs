using System.IO;
using System.Windows.Input;

namespace MusicManager
{
    public class MainViewModel
    {
        private readonly IDisplayService _displayService;
        private readonly IFileCleaner _cleaner;
        private const string Mp3FileSearchPattern = "*.mp3"; 

        public MainViewModel(IDisplayService displayService, IFileCleaner cleaner)
        {
            _displayService = displayService;
            _cleaner = cleaner;

            CleanCommand = new DelegateCommand<object>(o => CleanUpFiles());
        }

        public ICommand CleanCommand { get; set; }

        private void CleanUpFiles()
        {
            string selectedPath = _displayService.DisplayFolderBrowserDialogue();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                _cleaner.CleanFileProperties(Directory.GetFiles(selectedPath, Mp3FileSearchPattern));

                var directory = new DirectoryInfo(selectedPath);
                FileInfo[] infos = directory.GetFiles(Mp3FileSearchPattern);
                _cleaner.CleanFileNames(infos);
            }
        }
    }
}
