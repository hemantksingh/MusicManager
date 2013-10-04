using System.IO;
using System.Windows.Input;
using MusicManager.Shared;
using MusicManager.UI;

namespace MusicManager
{
    public class MainViewModel
    {
        private readonly IPromptService _promptService;
        private readonly IFileCleaner _cleaner;
        private const string Mp3FileSearchPattern = "*.mp3"; 

        public MainViewModel(IPromptService promptService, IFileCleaner cleaner)
        {
            _promptService = promptService;
            _cleaner = cleaner;

            CleanCommand = new DelegateCommand<object>(o => CleanUpFiles());
        }

        public ICommand CleanCommand { get; set; }

        private void CleanUpFiles()
        {
            string selectedPath = _promptService.ShowFolderBrowserDialogue();
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
