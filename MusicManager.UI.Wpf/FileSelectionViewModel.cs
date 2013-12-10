using System.Collections.Generic;
using System.IO;
using MusicManager.Infrastructure;
using MusicManager.Shared;

namespace MusicManager.UI.Wpf
{
    public class FileSelectionViewModel
    {
        private const string Mp3FileSearchPattern = "*.mp3";

        private readonly IDirectory _directory;
        private readonly IFileCleaner _fileCleaner;

        public FileSelectionViewModel(IDirectory directory, IFileCleaner fileCleaner)
        {
            _directory = directory;
            _fileCleaner = fileCleaner;
            Files = new List<string>();
        }

        public List<string> Files { get; private set; }

        public void CleanUpFiles(string selectedPath)
        {
            if (string.IsNullOrWhiteSpace(selectedPath)) return;

            _fileCleaner.CleanFileProperties(Files);

            FileInfo[] fileInfos = _directory.GetFiles(selectedPath, Mp3FileSearchPattern);
            _fileCleaner.CleanFileNames(fileInfos);
        }

        public void LoadFiles(string selectedPath)
        {
            if (string.IsNullOrWhiteSpace(selectedPath)) return;
            
            Files = _directory.GetFiles(
                selectedPath, Mp3FileSearchPattern, SearchOption.AllDirectories);
        }
    }
}