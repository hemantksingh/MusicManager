using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using MusicManager.Infrastructure;
using MusicManager.Shared;
using MusicManager.UI.Wpf;
using NUnit.Framework;

namespace MusicManager.Test
{
    [TestFixture]
    public class WhenSelectFilesCommandIsIssued
    {
        private const string DefaultPath = "C:/";
        private Mock<IPromptService> _mockedPromptService;
        private Func<MainViewModel> _createMainViewModel;
        private Mock<IDirectory> _mockedDirectory;
        
        [SetUp]
        public void Initialize()
        {
            _mockedPromptService = new Mock<IPromptService>();
            _mockedDirectory = new Mock<IDirectory>();

            _mockedPromptService.Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(DefaultPath);
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath, "*.mp3", SearchOption.AllDirectories))
                            .Returns(new List<string>());

            Func<FileSelectionViewModel> fileSelectionViewModelfactory =
                () => new FileSelectionViewModel(_mockedDirectory.Object, Mock.Of<IFileCleaner>());

            _createMainViewModel = () => new MainViewModel(_mockedPromptService.Object, Mock.Of<IEventAggregator>(),
                                                           fileSelectionViewModelfactory,
                                                           () => new OkCancelPanelViewModel(
                                                                     Mock.Of<IEventAggregator>()));
        }

        [Test]
        public void FolderBroswerDialougeIsDisplayed()
        {
            var mockedViewModel = _createMainViewModel();

            mockedViewModel.SelectFilesCommand.Execute(null);

           _mockedPromptService.Verify(x => x.ShowFolderBrowserDialogue());
        }

        [Test]
        public void IfFolderSelectedFileNamesDisplayed()
        {
            var musicFiles = new List<string> {"AMusicFile.mp3", "BMusicfile.mp3"};
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath,
                                                   It.IsAny<string>(),
                                                   SearchOption.AllDirectories))
                            .Returns(musicFiles);

            MainViewModel mainViewModel = _createMainViewModel();
            mainViewModel.SelectFilesCommand.Execute(null);

            Assert.IsNotNull(mainViewModel.FileSelection);
        }

        [Test]
        public void IfFolderSelectedFileCountUpdated()
        {
            var musicFiles = new List<string> { "AMusicFile.mp3", "BMusicfile.mp3" };
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath,
                                                   It.IsAny<string>(),
                                                   SearchOption.AllDirectories))
                            .Returns(musicFiles);

            MainViewModel mainViewModel = _createMainViewModel();
            
            mainViewModel.SelectFilesCommand.Execute(null);

            Assert.AreEqual(musicFiles.Count, mainViewModel.FileSelection.NoOfFiles);
        }

        [Test]
        public void IfFolderSelectedFileCountChangeNotified()
        {
            MainViewModel mainViewModel = _createMainViewModel();
            
            mainViewModel.SelectFilesCommand.Execute(null);

            mainViewModel.FileSelection.ShouldNotifyOn(model => model.NoOfFiles)
                .When(model => model.LoadFiles(DefaultPath));
        }

        [Test]
        public void IfFolderSelectedAgainFileCountUpdated()
        {
            var originallySelectedMusicFiles = new List<string> { "AMusic.mp3", "BMusic.mp3" };
            var newlySelectedMusicFiles = new List<string> { "AMusic.mp3", "BMusic.mp3", "CMusic.mp3" };
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath,
                                                   It.IsAny<string>(),
                                                   SearchOption.AllDirectories))
                            .Returns(originallySelectedMusicFiles);

            MainViewModel mainViewModel = _createMainViewModel();
            mainViewModel.SelectFilesCommand.Execute(null);
            
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath,
                                                   It.IsAny<string>(),
                                                   SearchOption.AllDirectories))
                            .Returns(newlySelectedMusicFiles);
            mainViewModel.SelectFilesCommand.Execute(null);
            
            Assert.AreNotEqual(originallySelectedMusicFiles.Count, mainViewModel.FileSelection.NoOfFiles);
            Assert.AreEqual(newlySelectedMusicFiles.Count, mainViewModel.FileSelection.NoOfFiles);
        }

        [Test]
        public void IfFolderSelectedOkCancelPanelDisplayed()
        {
            MainViewModel mainViewModel = _createMainViewModel();
            mainViewModel.SelectFilesCommand.Execute(null);

            Assert.IsNotNull(mainViewModel.OkCancelPanel);
        }

        [Test]
        public void IfNoFolderSelectedNoFileNamesDisplayed()
        {
            _mockedPromptService.Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(() => null);

            MainViewModel mainViewModel = _createMainViewModel();
            mainViewModel.SelectFilesCommand.Execute(null);

            Assert.IsNull(mainViewModel.FileSelection);
        }
    }

    public class WhenOkCommandIsIssuedAfterFileSelection
    {
        private const string DefaultPath = "C:/";
        private const string SearchPattern = "*.mp3";
        private Func<MainViewModel> _createMainViewModel;
        private Mock<IDirectory> _mockedDirectory;
        private Mock<IFileCleaner> _mockedFileCleaner;
        private Mock<IPromptService> _mockedPromptService;

        [SetUp]
        public void Initialize()
        {
            _mockedPromptService = new Mock<IPromptService>();
            _mockedDirectory = new Mock<IDirectory>();
            _mockedFileCleaner = new Mock<IFileCleaner>();

            _mockedPromptService.Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(DefaultPath);
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath, SearchPattern, SearchOption.AllDirectories))
                            .Returns(new List<string>());

            Func<FileSelectionViewModel> fileSelectionViewModelfactory =
                () => new FileSelectionViewModel(_mockedDirectory.Object,
                                                 _mockedFileCleaner.Object);

            var eventAggregator = new EventAggregator();
            _createMainViewModel = () => new MainViewModel(_mockedPromptService.Object, eventAggregator,
                                                           fileSelectionViewModelfactory,
                                                           () => new OkCancelPanelViewModel(
                                                                     eventAggregator));
        }

        [Test]
        public void SelectedFilePropertiesAreCleanedUp()
        {
            var musicFiles = new List<string> {"AMusicFile.mp3", "BMusicfile.mp3"};
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath, SearchPattern, SearchOption.AllDirectories))
                            .Returns(musicFiles);
            MainViewModel mainViewModel = _createMainViewModel();

            mainViewModel.SelectFilesCommand.Execute(null);
            mainViewModel.OkCancelPanel.OkCommand.Execute(null);

            _mockedFileCleaner.Verify(x => x.CleanFileProperties(musicFiles));
        }

        [Test]
        public void SelectedFileNamesAreCleanedUp()
        {
            var files = new FileInfo[] {};
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath, SearchPattern))
                            .Returns(files);
            MainViewModel mainViewModel = _createMainViewModel();

            mainViewModel.SelectFilesCommand.Execute(null);
            mainViewModel.OkCancelPanel.OkCommand.Execute(null);

            _mockedFileCleaner.Verify(x => x.CleanFileNames(files));
        }

        [Test]
        public void FileSelectionIsCleared()
        {
            MainViewModel mainViewModel = _createMainViewModel();
            
            mainViewModel.SelectFilesCommand.Execute(null);
            mainViewModel.OkCancelPanel.OkCommand.Execute(null);

            Assert.IsNull(mainViewModel.FileSelection);
            Assert.IsNull(mainViewModel.OkCancelPanel);
        }
    }

    public class WhenCancelCommandIsIssuedAfterFileSelection
    {
        private const string DefaultPath = "C:/";
        private const string SearchPattern = "*.mp3";
        private Func<MainViewModel> _createMainViewModel;
        private Mock<IDirectory> _mockedDirectory;
        private Mock<IFileCleaner> _mockedFileCleaner;
        private Mock<IPromptService> _mockedPromptService;

        [SetUp]
        public void Initialize()
        {
            _mockedPromptService = new Mock<IPromptService>();
            _mockedDirectory = new Mock<IDirectory>();
            _mockedFileCleaner = new Mock<IFileCleaner>();

            _mockedPromptService.Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(DefaultPath);
            
            Func<FileSelectionViewModel> fileSelectionViewModelfactory =
                () => new FileSelectionViewModel(_mockedDirectory.Object,
                                                 _mockedFileCleaner.Object);

            var eventAggregator = new EventAggregator();
            _createMainViewModel = () => new MainViewModel(_mockedPromptService.Object, eventAggregator,
                                                           fileSelectionViewModelfactory,
                                                           () => new OkCancelPanelViewModel(
                                                                     eventAggregator));
        }

        [Test]
        public void FileSelectionIsCleared()
        {
            var musicFiles = new List<string> { "AMusicFile.mp3", "BMusicfile.mp3" };
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath, SearchPattern, SearchOption.AllDirectories))
                            .Returns(musicFiles);

            MainViewModel mainViewModel = _createMainViewModel();
            mainViewModel.SelectFilesCommand.Execute(null);
            mainViewModel.OkCancelPanel.CancelCommand.Execute(null);

            Assert.IsNull(mainViewModel.FileSelection);
            Assert.IsNull(mainViewModel.OkCancelPanel);
        }
    }
}