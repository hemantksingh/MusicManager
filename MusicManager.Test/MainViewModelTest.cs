using System.IO;
using AutoMoq;
using Moq;
using MusicManager.Shared;
using NUnit.Framework;

namespace MusicManager.Test
{
    [TestFixture]
    public class MainViewModelTest
    {
        [SetUp]
        public void Initialize()
        {
            _mocker = new AutoMoqer();
        }

        private AutoMoqer _mocker;

        [Test]
        public void OnCleanUpCommandFolderBroswerDialougeisDisplayed()
        {
            var mockedViewModel = _mocker.Resolve<MainViewModel>();

            mockedViewModel.CleanCommand.Execute(null);

            _mocker.GetMock<IPromptService>().Verify(
                x => x.ShowFolderBrowserDialogue(), Times.Once());
        }

        [Test]
        public void OnCleanUpCommandIfFolderSelectedAllMp3FileNamesAreCleaned()
        {
            var mockedViewModel = _mocker.Resolve<MainViewModel>();

            _mocker.GetMock<IPromptService>().Setup(
                x => x.ShowFolderBrowserDialogue()).Returns("C:/");

            mockedViewModel.CleanCommand.Execute(null);

            Mock<IFileCleaner> mockedCleaner = _mocker.GetMock<IFileCleaner>();
            mockedCleaner.Verify(
                x => x.CleanFileProperties(It.IsAny<string[]>()), Times.Once());

            mockedCleaner.Verify(
                x => x.CleanFileNames(It.IsAny<FileInfo[]>()), Times.Once());
        }

        [Test]
        public void OnCleanUpCommandIfFolderSelectedAllMp3FilePropertiesAreCleaned()
        {
            var mockedViewModel = _mocker.Resolve<MainViewModel>();

            _mocker.GetMock<IPromptService>().Setup(
                x => x.ShowFolderBrowserDialogue()).Returns("C:/");

            mockedViewModel.CleanCommand.Execute(null);

            Mock<IFileCleaner> mockedCleaner = _mocker.GetMock<IFileCleaner>();
            mockedCleaner.Verify(
                x => x.CleanFileProperties(It.IsAny<string[]>()), Times.Once());
        }

        [Test]
        public void OnCleanUpCommandIfNoFolderSelectedNoFileNamesAreCleaned()
        {
            var mockedViewModel = _mocker.Resolve<MainViewModel>();
            _mocker.GetMock<IPromptService>().Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(() => null);

            mockedViewModel.CleanCommand.Execute(null);

            Mock<IFileCleaner> mockedCleaner = _mocker.GetMock<IFileCleaner>();

            mockedCleaner.Verify(
                x => x.CleanFileNames(It.IsAny<FileInfo[]>()), Times.Never());
        }

        [Test]
        public void OnCleanUpCommandIfNoFolderSelectedNoFilePropertiesAreCleaned()
        {
            var mockedViewModel = _mocker.Resolve<MainViewModel>();
            _mocker.GetMock<IPromptService>().Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(() => null);

            mockedViewModel.CleanCommand.Execute(null);

            Mock<IFileCleaner> mockedCleaner = _mocker.GetMock<IFileCleaner>();
            mockedCleaner.Verify(
                x => x.CleanFileProperties(It.IsAny<string[]>()), Times.Never());
        }
    }
}