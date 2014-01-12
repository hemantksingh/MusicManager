using MusicManager;
using MusicManager.Shared;

namespace Test.MusicManager.Scenarios.SelectingFiles
{
    public class When_selecting_files : BaseTestFixture<MainViewModel>
    {
        protected override void When()
        {
            SubjectUnderTest.SelectFilesCommand.Execute(null);
        }

        [Then]
        public void Then_the_folder_browser_dialouge_is_displayed()
        {
            OnDependency<IPromptService>().Verify(x => x.ShowFolderBrowserDialogue());
        }
    }
}
