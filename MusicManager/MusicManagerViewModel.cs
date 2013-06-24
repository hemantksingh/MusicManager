using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MusicManager
{
    class MusicManagerViewModel
    {
        private IDisplayService _displayService;
        private IFileCleaner _cleaner;

        public MusicManagerViewModel(IDisplayService displayService, IFileCleaner cleaner)
        {
            this._displayService = displayService;
            this._cleaner = cleaner;

            this.CleanCommand = new DelegateCommand<object>((o) => CleanUpFiles());
        }

        public ICommand CleanCommand { get; set; }

        private void CleanUpFiles()
        {
            string selectedPath = this._displayService.DisplayFolderBrowserDialogue();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                _cleaner.CleanFileProperties(Directory.GetFiles(selectedPath, "*.mp3"));

                DirectoryInfo d = new DirectoryInfo(selectedPath);
                FileInfo[] infos = d.GetFiles("*.mp3");
                _cleaner.CleanFileNames(infos);
            }
        }
    }
}
