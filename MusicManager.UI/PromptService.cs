using System.Windows;
using System.Windows.Forms;
using MusicManager.Shared;
using MessageBox = System.Windows.MessageBox;

namespace MusicManager.UI
{
    public class PromptService : IPromptService
    {
        private readonly string _applicationTitle;

        public PromptService(string applicationTitle)
        {
            _applicationTitle = applicationTitle;
        }

        public void ShowError(string errorMessage)
        {
            if (!string.IsNullOrWhiteSpace(errorMessage))
                MessageBox.Show(errorMessage, _applicationTitle, MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        public string ShowFolderBrowserDialogue()
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

            if (!string.IsNullOrEmpty(path))
                Settings.Default.LastSelectedFolder = path;
            return path;
        }
    }
}