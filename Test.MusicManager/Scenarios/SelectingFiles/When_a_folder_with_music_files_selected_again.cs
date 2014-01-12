using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using MusicManager;
using MusicManager.Infrastructure;
using MusicManager.Shared;
using MusicManager.UI.Wpf;
using NUnit.Framework;

namespace Test.MusicManager.Scenarios.SelectingFiles
{
    class When_a_folder_with_music_files_selected_again: BaseTestFixture<MainViewModel>
    {
        private List<string> _originallySelectedMusicFiles;
        private Mock<IDirectory> _mockedDirectory;
        private List<string> _newlySelectedMusicFiles;
        private const string DefaultPath = "C:/";
        private const string SearchPattern = "*.mp3";

        protected override void SetupDependencies()
        {
            OnDependency<IPromptService>().Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(DefaultPath);
            
            Func<FileSelectionViewModel> fileSelectionViewModelFactory = () =>
                new FileSelectionViewModel(_mockedDirectory.Object, Mock.Of<IFileCleaner>());
            Func<OkCancelPanelViewModel> okCancelViewModelFactory =
                () => new OkCancelPanelViewModel(Mock.Of<IEventAggregator>());

            DoNotMock.Add(typeof(Func<FileSelectionViewModel>), fileSelectionViewModelFactory);
            DoNotMock.Add(typeof(Func<OkCancelPanelViewModel>), okCancelViewModelFactory);
        }

        protected override void Given()
        {
            _originallySelectedMusicFiles = new List<string> { "AMusicFile.mp3", "BMusicfile.mp3" };
            _mockedDirectory = new Mock<IDirectory>();
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath,
                                                  SearchPattern,
                                                  SearchOption.AllDirectories))
                                                  .Returns(_originallySelectedMusicFiles);
        }

        protected override void When()
        {
            SubjectUnderTest.SelectFilesCommand.Execute(null);

            _newlySelectedMusicFiles = new List<string> { "AMusic.mp3", "BMusic.mp3", "CMusic.mp3" };
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath,
                                                   It.IsAny<string>(),
                                                   SearchOption.AllDirectories))
                            .Returns(_newlySelectedMusicFiles);

            SubjectUnderTest.SelectFilesCommand.Execute(null);
        }

        [Then]
        public void Selected_file_count_is_updated()
        {
            Assert.AreNotEqual(_originallySelectedMusicFiles.Count, 
                               SubjectUnderTest.FileSelection.NoOfFiles);
            Assert.AreEqual(_newlySelectedMusicFiles.Count, 
                            SubjectUnderTest.FileSelection.NoOfFiles);
        }
    }
}
