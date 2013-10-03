using MusicManager.Properties;
using System.Windows.Forms;

namespace MusicManager
{
    class DisplayService : IDisplayService
    {
        public string DisplayFolderBrowserDialogue()
        {
            Settings.Default.Reload();
            var dialog = new FolderBrowserDialog
                {
                    SelectedPath = Settings.Default.LastSelectedFolder,
                    Description = @"Select directory to choose from",
                    ShowNewFolderButton = true
                };
            //dlg.FileName = "Music Files";
            //dlg.DefaultExt = ".mp3";
            //dlg.Filter = "Music files (.mp3)| *.mp3";

            var result = dialog.ShowDialog();

            string path = string.Empty;

            if (result == DialogResult.OK)
            {
                path = dialog.SelectedPath;
            }

            if(!string.IsNullOrEmpty(path))
                Settings.Default.LastSelectedFolder = path;
            return path;
        }
    }
}
