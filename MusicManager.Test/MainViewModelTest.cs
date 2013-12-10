using System;
using System.Collections.Generic;
using System.IO;
using AutoMoq;
using Moq;
using MusicManager.Infrastructure;
using MusicManager.Shared;
using MusicManager.UI.Wpf;
using NUnit.Framework;

namespace MusicManager.Test
{
    [TestFixture]
    public class MainViewModelTest
    {
        private AutoMoqer _mocker;
        private Mock<IPromptService> _mockedPromptService;
        private Func<MainViewModel> _createMainViewModel;
        private Mock<IDirectory> _mockedDirectory;

        [SetUp]
        public void Initialize()
        {
            _mocker = new AutoMoqer();
            _mockedPromptService = new Mock<IPromptService>();
            _mockedDirectory = new Mock<IDirectory>();

            Func<string, FileSelectionViewModel> fileSelectionViewModelfactory =
                selectedPath => new FileSelectionViewModel(_mockedDirectory.Object,
                                                           Mock.Of<IFileCleaner>(),
                                                           selectedPath);

            _createMainViewModel = () => new MainViewModel(_mockedPromptService.Object,
                                                           fileSelectionViewModelfactory,
                                                           Mock.Of<OkCancelPanelViewModel>);
        }

        [Test]
        public void OnSelectFilesCommandFolderBroswerDialougeIsDisplayed()
        {
            var mockedViewModel = _mocker.Resolve<MainViewModel>();

            mockedViewModel.SelectFilesCommand.Execute(null);

            _mocker.GetMock<IPromptService>().Verify(
                x => x.ShowFolderBrowserDialogue(), Times.Once());
        }

        [Test]
        public void OnSelectFilesCommandIfFolderSelectedFileNamesDisplayed()
        {
            const string path = "C:/";
            _mockedPromptService.Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(path);
            var musicFiles = new List<string> {"AMusicFile.mp3", "BMusicfile.mp3"};
            _mockedDirectory.Setup(x => x.GetFiles(path, It.IsAny<string>(), SearchOption.AllDirectories))
                            .Returns(musicFiles);

            MainViewModel mainViewModel = _createMainViewModel();
            mainViewModel.SelectFilesCommand.Execute(null);

            Assert.IsNotNull(mainViewModel.FileSelection);
            Assert.AreEqual(musicFiles.Count, mainViewModel.FileSelection.Files.Count);
        }

        [Test]
        public void OnSelectFilesCommandIfFolderSelectedOkCancelPanelDisplayed()
        {
            _mockedPromptService.Setup(
                x => x.ShowFolderBrowserDialogue()).Returns("C:/");

            MainViewModel mainViewModel = _createMainViewModel();
            mainViewModel.SelectFilesCommand.Execute(null);

            Assert.IsNotNull(mainViewModel.OkCancelPanel);
        }

        [Test]
        public void OnSelectFilesCommandIfNoFolderSelectedNoFileNamesDisplayed()
        {
            _mockedPromptService.Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(() => null);

            MainViewModel mainViewModel = _createMainViewModel();
            mainViewModel.SelectFilesCommand.Execute(null);

            Assert.AreEqual(0, mainViewModel.FileSelection.Files.Count);
        }
    }
}