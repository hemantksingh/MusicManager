using NUnit.Framework;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;

namespace Test.UI.MusicManager.SelectingFiles
{
    [Scenario("Selecting mp3 files from the menu")]
    public class Given_menu_option_select_mp3_files_is_selected
    {
        private const string AppFileName = "C:/Code/MusicManager/MusicManager/bin/Debug/MusicManager.exe";
        private Application _application;
        private Window _mainWindow;
        
        [SetUp]
        public void When_a_folder_is_selected_with_no_music_files()
        {
            Given();
            When();
        }

        private void Given()
        {
            _application = Application.Launch(AppFileName);
            _mainWindow = _application.GetWindow("Music Manager", InitializeOption.NoCache);
            Menu selectFileMenuItem = _mainWindow.MenuBar.MenuItem("File", "Select Files (.mp3)");
            selectFileMenuItem.Click();
        }

        private void When()
        {
            Window dialogueBox = _mainWindow.ModalWindow("Browse For Folder");
            var okBtn = dialogueBox.Get<Button>(SearchCriteria.ByText("OK"));
            okBtn.Click();
        }

        [Test]
        public void Then_there_are_no_music_files_displayed_on_the_main_view()
        {
            var listBox = _mainWindow.Get<ListBox>();
            Assert.AreEqual(0, listBox.Items.Count);
        }

        [Test]
        public void Then_the_selected_files_count_is_displayed_as_0()
        {
            var selectedFilesLbl = _mainWindow.Get<Label>(SearchCriteria.ByText("0"));
            Assert.NotNull(selectedFilesLbl);
        }

        [Test]
        public void Then_Ok_Cancel_panel_is_displayed_to_get_back_to_the_main_view()
        {
            Assert.NotNull(_mainWindow.Get<Button>(SearchCriteria.ByText("Ok")));
            Assert.NotNull(_mainWindow.Get<Button>(SearchCriteria.ByText("Cancel")));
        }

        [TearDown]
        public void CleanUp()
        {
            _mainWindow.Close();
            _application.Dispose();
            
            _application = null;
            _mainWindow = null;
        }
    }
}
