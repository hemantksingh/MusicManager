using System.Windows;
using MusicManager.Shared;

namespace MusicManager.UI
{
    class PromptService : IPromptService
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
    }
}