using System;
using Moq;
using MusicManager;
using MusicManager.Infrastructure;
using MusicManager.Shared;
using MusicManager.UI.Wpf;
using NUnit.Framework;

namespace Test.MusicManager.Scenarios.CleaningUpFiles
{
    class When_Cancel_clicked_after_selecting_files: BaseTestFixture<MainViewModel>
    {
        private const string DefaultPath = "C:/";

        protected override void SetupDependencies()
        {
            OnDependency<IPromptService>().Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(DefaultPath);

            Func<FileSelectionViewModel> fileSelectionViewModelFactory = () =>
                new FileSelectionViewModel(Mock.Of<IDirectory>(), Mock.Of<IFileCleaner>());
            IEventAggregator eventAggregator = new EventAggregator();
            Func<OkCancelPanelViewModel> okCancelViewModelFactory =
                () => new OkCancelPanelViewModel(eventAggregator);

            DoNotMock.Add(typeof(IEventAggregator), eventAggregator);
            DoNotMock.Add(typeof(Func<FileSelectionViewModel>), fileSelectionViewModelFactory);
            DoNotMock.Add(typeof(Func<OkCancelPanelViewModel>), okCancelViewModelFactory);
        }
        protected override void When()
        {
            SubjectUnderTest.SelectFilesCommand.Execute(null);
            SubjectUnderTest.OkCancelPanel.CancelCommand.Execute(null);
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
