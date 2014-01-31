using System.Collections.Generic;

namespace MusicManager.Commands
{
    public class CleanUpFilesCommand : Command
    {
        public CleanUpFilesCommand(string selectedPath, List<string> selectedFiles)
        {
            SelectedPath = selectedPath;
            SelectedFiles = selectedFiles;
        }

        public string SelectedPath { get; private set; }
        public List<string> SelectedFiles { get; private set; }
    }
}
