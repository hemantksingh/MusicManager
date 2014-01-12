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
    class When_a_folder_with_music_files_selected : BaseTestFixture<MainViewModel>
    {
        private List<string> _musicFiles;
        private Mock<IDirectory> _mockedDirectory;
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
            _musicFiles = new List<string> { "AMusicFile.mp3", "BMusicfile.mp3" };
            _mockedDirectory = new Mock<IDirectory>();
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath,
                                                  SearchPattern,
                                                  SearchOption.AllDirectories))
                                                  .Returns(_musicFiles);

        }

        protected override void When()
        {
            SubjectUnderTest.SelectFilesCommand.Execute(null);
        }

        [Then]
        public void Then_the_selected_music_file_names_are_displayed()
        {
            Assert.IsNotNull(SubjectUnderTest.FileSelection);
        }

        [Then]
        public void Then_the_ok_cancel_panel_is_displayed()
        {
            Assert.IsNotNull(SubjectUnderTest.OkCancelPanel);
        }

        [Then]
        public void Then_the_selected_files_count_is_updated()
        {
            Assert.AreEqual(_musicFiles.Count, SubjectUnderTest.FileSelection.NoOfFiles);
        }

        [Then]
        // Todo This should possibly go into FileSelectionViewModel Tests
        public void Then_the_selected_files_count_change_is_notified()
        {
            SubjectUnderTest.FileSelection.ShouldNotifyOn(model => model.NoOfFiles)
                .When(model => model.LoadFiles(DefaultPath));
        }
    }
}
