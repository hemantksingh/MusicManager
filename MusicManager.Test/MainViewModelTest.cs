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
        [SetUp]
        public void Initialize()
        {
            _mocker = new AutoMoqer();
        }

        private AutoMoqer _mocker;

        [Test]
        public void OnSelectFilesCommandFolderBroswerDialougeisDisplayed()
        {
            var mockedViewModel = _mocker.Resolve<MainViewModel>();

            mockedViewModel.SelectFilesCommand.Execute(null);

            _mocker.GetMock<IPromptService>().Verify(
                x => x.ShowFolderBrowserDialogue(), Times.Once());
        }

        [Test]
        public void OnSelectFilesCommandIfFolderSelectedFileNamesDisplayed()
        {
            var mockedPromptService = new Mock<IPromptService>();
            var mainViewModel = new MainViewModel(mockedPromptService.Object,
                                                  selectedPath => new FileSelectionViewModel(
                                                                      Mock.Of<IDirectory>(),
                                                                      Mock.Of<IFileCleaner>(),
                                                                      selectedPath));
            mockedPromptService.Setup(
                x => x.ShowFolderBrowserDialogue()).Returns("C:/");

            mainViewModel.SelectFilesCommand.Execute(null);
            
            Assert.IsNotNull(mainViewModel.FileSelection);
        }

        [Test]
        public void OnSelectFilesCommandIfNoFolderSelectedNoFileNamesDisplayed()
        {
            var mockedPromptService = new Mock<IPromptService>();
            var mainViewModel = new MainViewModel(mockedPromptService.Object,
                                                  selectedPath => new FileSelectionViewModel(
                                                                      Mock.Of<IDirectory>(),
                                                                      Mock.Of<IFileCleaner>(),
                                                                      selectedPath));
            mockedPromptService.Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(() => null);
            mainViewModel.SelectFilesCommand.Execute(null);

            Assert.IsNull(mainViewModel.FileSelection);
        }
    }
}