using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MusicManager
{
    public class MusicManagerViewModel
    {
        private IDisplayService _displayService;
        private IFileCleaner _cleaner;
        private const string MP3FileSearchPattern = "*.mp3"; 

        public MusicManagerViewModel(IDisplayService displayService, IFileCleaner cleaner)
        {
            _displayService = displayService;
            _cleaner = cleaner;

            CleanCommand = new DelegateCommand<object>((o) => CleanUpFiles());
        }

        public ICommand CleanCommand { get; set; }

        private void CleanUpFiles()
        {
            string selectedPath = this._displayService.DisplayFolderBrowserDialogue();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                _cleaner.CleanFileProperties(Directory.GetFiles(selectedPath, MP3FileSearchPattern));

                DirectoryInfo directory = new DirectoryInfo(selectedPath);
                FileInfo[] infos = directory.GetFiles(MP3FileSearchPattern);
                _cleaner.CleanFileNames(infos);
            }
        }
    }
}
