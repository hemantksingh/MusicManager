using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Moq;
using MusicManager;
using MusicManager.Infrastructure;
using MusicManager.Shared;
using MusicManager.UI.Wpf;

namespace Test.MusicManager.Scenarios.CleaningUpFiles
{
    class When_Ok_clicked_after_selecting_readonly_files : BaseTestFixture<MainViewModel>
    {
        private List<string> _musicFiles;
        private Mock<IFileCleaner> _mockedFileCleaner;
        private Mock<IDirectory> _mockedDirectory;
        private IContainer _container;
        private Mock<IPromptService> _mockedPromptService;
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
            _musicFiles = new List<string> { "AMusicFile.mp3", "BMusicfile.mp3" };            
            _mockedFileCleaner = new Mock<IFileCleaner>();
            _mockedFileCleaner.Setup(x => x.CleanFileProperties(_musicFiles))
                .Throws<UnauthorizedAccessException>();

            _mockedDirectory = new Mock<IDirectory>();
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath,
                                                  SearchPattern,
                                                  SearchOption.AllDirectories))
                                                  .Returns(_musicFiles);
            _mockedPromptService = new Mock<IPromptService>();
            _mockedPromptService.Setup(x => x.ShowFolderBrowserDialogue()).Returns(DefaultPath);

            var dependencies = new Dictionary<Type, object>
                {
                    {typeof (IFileCleaner), _mockedFileCleaner.Object},
                    {typeof (IDirectory), _mockedDirectory.Object},
                    {typeof (IPromptService), _mockedPromptService.Object},
                };

            var containerFactory = new ContainerFactory(dependencies);
            _container = ApplicationBootStrapper.BootUpTheApp(containerFactory);
        }

        protected override void When()
        {
            var mainViewModel = _container.Resolve<MainViewModel>();
            mainViewModel.SelectFilesCommand.Execute(null);
            mainViewModel.OkCancelPanel.OkCommand.Execute(null);
        }

        [Then]
        public void Then_error_is_prompted()
        {
            _mockedPromptService.Verify(x => x.ShowError(
                It.Is<string>(message => message.Contains(
                    "Failed to clean up files."))));
        }
    }
}
