using AutoMoq;
using Moq;
using NUnit.Framework;
using System.IO;

namespace MusicManager.Test
{
    [TestFixture]
    public class MainViewModelTest
    {
        private AutoMoqer _mocker;
        
        [SetUp]
        public void Initialize()
        {
            this._mocker = new AutoMoqer();
        }

        [Test]
        public void OnCleanUpCommandFolderBroswerDialougeisDisplayed()
        {
            var mockedViewModel = this._mocker.Resolve<MainViewModel>();

            mockedViewModel.CleanCommand.Execute(null);

            this._mocker.GetMock<IDisplayService>().Verify(
                x => x.DisplayFolderBrowserDialogue(), Times.Once());
        }

        [Test]
        public void OnCleanUpCommandIfFolderSelectedAllMp3FilePropertiesAreCleaned()
        {
            var mockedViewModel = this._mocker.Resolve<MainViewModel>();

            this._mocker.GetMock<IDisplayService>().Setup(
                x => x.DisplayFolderBrowserDialogue()).Returns("C:/");

            mockedViewModel.CleanCommand.Execute(null);

            var mockedCleaner = this._mocker.GetMock<IFileCleaner>();
            mockedCleaner.Verify(
                x => x.CleanFileProperties(It.IsAny<string[]>()), Times.Once());
        }

        [Test]
        public void OnCleanUpCommandIfFolderSelectedAllMp3FileNamesAreCleaned()
        {
            var mockedViewModel = this._mocker.Resolve<MainViewModel>();

            this._mocker.GetMock<IDisplayService>().Setup(
                x => x.DisplayFolderBrowserDialogue()).Returns("C:/");

            mockedViewModel.CleanCommand.Execute(null);
            
            var mockedCleaner = this._mocker.GetMock<IFileCleaner>();
            mockedCleaner.Verify(
                x => x.CleanFileProperties(It.IsAny<string[]>()), Times.Once());

            mockedCleaner.Verify(
                x => x.CleanFileNames(It.IsAny<FileInfo[]>()), Times.Once());
        }

        [Test]
        public void OnCleanUpCommandIfNoFolderSelectedNoFilePropertiesAreCleaned()
        {
            var mockedViewModel = this._mocker.Resolve<MainViewModel>();
            this._mocker.GetMock<IDisplayService>().Setup(
                x => x.DisplayFolderBrowserDialogue()).Returns(() => null);

            mockedViewModel.CleanCommand.Execute(null);

            var mockedCleaner = this._mocker.GetMock<IFileCleaner>();
            mockedCleaner.Verify(
                x => x.CleanFileProperties(It.IsAny<string[]>()), Times.Never());
        }

        [Test]
        public void OnCleanUpCommandIfNoFolderSelectedNoFileNamesAreCleaned()
        {
            var mockedViewModel = this._mocker.Resolve<MainViewModel>();
            this._mocker.GetMock<IDisplayService>().Setup(
                x => x.DisplayFolderBrowserDialogue()).Returns(() => null);

            mockedViewModel.CleanCommand.Execute(null);

            var mockedCleaner = this._mocker.GetMock<IFileCleaner>();

            mockedCleaner.Verify(
                x => x.CleanFileNames(It.IsAny<FileInfo[]>()), Times.Never());
        }
    }
}
