using AutoMoq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MusicManager.Test
{
    [TestFixture]
    public class MusicManagerViewModelTest
    {
        private AutoMoqer mocker;
        
        [SetUp]
        public void Initialize()
        {
            this.mocker = new AutoMoqer();
        }

        [Test]
        public void OnCleanUpCommandFolderBroswerDialougeisDisplayed()
        {
            var mockedViewModel = this.mocker.Resolve<MusicManagerViewModel>();

            mockedViewModel.CleanCommand.Execute(null);

            this.mocker.GetMock<IDisplayService>().Verify(
                x => x.DisplayFolderBrowserDialogue(), Times.Once());
        }

        [Test]
        public void OnCleanUpCommandIfFolderSelectedAllMp3FilePropertiesAreCleaned()
        {
            var mockedViewModel = this.mocker.Resolve<MusicManagerViewModel>();

            this.mocker.GetMock<IDisplayService>().Setup(
                x => x.DisplayFolderBrowserDialogue()).Returns("C:/");

            mockedViewModel.CleanCommand.Execute(null);

            var mockedCleaner = this.mocker.GetMock<IFileCleaner>();
            mockedCleaner.Verify(
                x => x.CleanFileProperties(It.IsAny<string[]>()), Times.Once());
        }

        [Test]
        public void OnCleanUpCommandIfFolderSelectedAllMp3FileNamesAreCleaned()
        {
            var mockedViewModel = this.mocker.Resolve<MusicManagerViewModel>();

            this.mocker.GetMock<IDisplayService>().Setup(
                x => x.DisplayFolderBrowserDialogue()).Returns("C:/");

            mockedViewModel.CleanCommand.Execute(null);
            
            var mockedCleaner = this.mocker.GetMock<IFileCleaner>();
            mockedCleaner.Verify(
                x => x.CleanFileProperties(It.IsAny<string[]>()), Times.Once());

            mockedCleaner.Verify(
                x => x.CleanFileNames(It.IsAny<FileInfo[]>()), Times.Once());
        }

        [Test]
        public void OnCleanUpCommandIfNoFolderSelectedNoFilePropertiesAreCleaned()
        {
            var mockedViewModel = this.mocker.Resolve<MusicManagerViewModel>();
            this.mocker.GetMock<IDisplayService>().Setup(
                x => x.DisplayFolderBrowserDialogue()).Returns(() => null);

            mockedViewModel.CleanCommand.Execute(null);

            var mockedCleaner = this.mocker.GetMock<IFileCleaner>();
            mockedCleaner.Verify(
                x => x.CleanFileProperties(It.IsAny<string[]>()), Times.Never());
        }

        [Test]
        public void OnCleanUpCommandIfNoFolderSelectedNoFileNamesAreCleaned()
        {
            var mockedViewModel = this.mocker.Resolve<MusicManagerViewModel>();
            this.mocker.GetMock<IDisplayService>().Setup(
                x => x.DisplayFolderBrowserDialogue()).Returns(() => null);

            mockedViewModel.CleanCommand.Execute(null);

            var mockedCleaner = this.mocker.GetMock<IFileCleaner>();

            mockedCleaner.Verify(
                x => x.CleanFileNames(It.IsAny<FileInfo[]>()), Times.Never());
        }
    }
}
