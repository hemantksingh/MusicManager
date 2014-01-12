using System;
using Moq;
using MusicManager;
using MusicManager.Infrastructure;
using MusicManager.Shared;
using MusicManager.UI.Wpf;
using NUnit.Framework;

namespace Test.MusicManager.Scenarios.SelectingFiles
{
    class When_selecting_files_cancelled: BaseTestFixture<MainViewModel>
    {
        protected override void SetupDependencies()
        {
            OnDependency<IPromptService>().Setup(x => x.ShowFolderBrowserDialogue())
                .Returns(string.Empty);

            Func<FileSelectionViewModel> fileSelectionViewModelFactory = () =>
                new FileSelectionViewModel(Mock.Of<IDirectory>(), Mock.Of<IFileCleaner>());
            Func<OkCancelPanelViewModel> okCancelViewModelFactory =
                () => new OkCancelPanelViewModel(Mock.Of<IEventAggregator>());

            DoNotMock.Add(typeof(Func<FileSelectionViewModel>), fileSelectionViewModelFactory);
            DoNotMock.Add(typeof(Func<OkCancelPanelViewModel>), okCancelViewModelFactory);
        }
        
        protected override void When()
        {
            SubjectUnderTest.SelectFilesCommand.Execute(null);
        }

        [Then]
        public void Then_no_file_names_are_displayed()
        {
            Assert.IsNull(SubjectUnderTest.FileSelection);
        }

        [Then]
        public void Then_Ok_Cancel_panel_is_not_displayed()
        {
            Assert.IsNull(SubjectUnderTest.OkCancelPanel);
        }
    }
}
