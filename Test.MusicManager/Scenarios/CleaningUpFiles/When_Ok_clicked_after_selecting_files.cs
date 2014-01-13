using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using MusicManager;
using MusicManager.Infrastructure;
using MusicManager.Shared;
using MusicManager.UI.Wpf;
using NUnit.Framework;

namespace Test.MusicManager.Scenarios.CleaningUpFiles
{
    class When_Ok_clicked_after_selecting_files : BaseTestFixture<MainViewModel>
    {
        private List<string> _musicFiles;
        private Mock<IFileCleaner> _mockedFileCleaner;
        private FileInfo[] _files;
        private Mock<IDirectory> _mockedDirectory;
        private const string DefaultPath = "C:/";
        private const string SearchPattern = "*.mp3";

        protected override void SetupDependencies()
        {
            OnDependency<IPromptService>().Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(DefaultPath);
            
            var eventAggregator = new EventAggregator();
            Func<FileSelectionViewModel> fileSelectionViewModelFactory = () =>
                new FileSelectionViewModel(_mockedDirectory.Object, _mockedFileCleaner.Object);
            Func<OkCancelPanelViewModel> okCancelViewModelFactory =
                () => new OkCancelPanelViewModel(eventAggregator);

            DoNotMock.Add(typeof(IEventAggregator), eventAggregator);
            DoNotMock.Add(typeof(Func<FileSelectionViewModel>), fileSelectionViewModelFactory);
            DoNotMock.Add(typeof(Func<OkCancelPanelViewModel>), okCancelViewModelFactory);
        }

        protected override void Given()
        {
            _mockedFileCleaner = new Mock<IFileCleaner>();            
            _mockedDirectory = new Mock<IDirectory>();            
            _musicFiles = new List<string> { "AMusicFile.mp3", "BMusicfile.mp3" };
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath,
                                                  SearchPattern,
                                                  SearchOption.AllDirectories))
                                                  .Returns(_musicFiles);
            _files = new FileInfo[] { };
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath, SearchPattern))
                            .Returns(_files);
        }

        protected override void When()
        {
            SubjectUnderTest.SelectFilesCommand.Execute(null);
            SubjectUnderTest.OkCancelPanel.OkCommand.Execute(null);
        }

        [Then]
        public void Selected_file_properties_are_cleaned_up()
        {
            _mockedFileCleaner.Verify(x => x.CleanFileProperties(_musicFiles));
        }

        [Then]
        public void Selected_file_names_are_cleaned_up()
        {
            _mockedFileCleaner.Verify(x => x.CleanFileNames(_files));
        }

        [Then]
        public void File_selection_is_cleared()
        {
            Assert.IsNull(SubjectUnderTest.FileSelection);
        }

        [Then]
        public void Ok_Cancel_panel_is_cleared()
        {
            Assert.IsNull(SubjectUnderTest.OkCancelPanel);
        }
    }
}
