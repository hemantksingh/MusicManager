namespace MusicManager.Shared
{
    public interface IPromptService
    {
        void ShowError(string errorMessage);
        string ShowFolderBrowserDialogue();
    }
}