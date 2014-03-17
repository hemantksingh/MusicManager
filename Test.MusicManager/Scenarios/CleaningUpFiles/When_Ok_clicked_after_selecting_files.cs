using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Moq;
using MusicManager;
using MusicManager.Infrastructure;
using MusicManager.Shared;
using NUnit.Framework;

namespace Test.MusicManager.Scenarios.CleaningUpFiles
{
    class When_Ok_clicked_after_selecting_files : BaseTestFixture
    {
        private List<string> _musicFiles;
        private Mock<IFileCleaner> _mockedFileCleaner;
        private FileInfo[] _files;
        private Mock<IDirectory> _mockedDirectory;
        private IContainer _container;
        private MainViewModel _mainViewModel;
        private const string DefaultPath = "C:/";
        private const string SearchPattern = "*.mp3";

        protected override void Given()
        {
            _mockedFileCleaner = new Mock<IFileCleaner>();            
            _mockedDirectory = new Mock<IDirectory>();            
            _musicFiles = new List<string> { "AMusicFile.mp3", "BMusicfile.mp3" };
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath,
                                                  SearchPattern,
                                                  SearchOption.AllDirectories))
                                                  .Returns(_musicFiles);
            _files = new FileInfo[] { };
            _mockedDirectory.Setup(x => x.GetFiles(DefaultPath, SearchPattern))
                            .Returns(_files);

            var mockedPromptService = new Mock<IPromptService>();
            mockedPromptService.Setup(
                x => x.ShowFolderBrowserDialogue()).Returns(DefaultPath);

            var dependencies = new Dictionary<Type, object>
                {
                    {typeof (IFileCleaner), _mockedFileCleaner.Object},
                    {typeof (IDirectory), _mockedDirectory.Object}, 
                    {typeof (IPromptService), mockedPromptService.Object},
                };

            var containerFactory = new ContainerFactory(dependencies);
            _container = _container = ApplicationBootStrapper.BootUpTheApp(containerFactory);
        }

        protected override void When()
        {
            _mainViewModel = _container.Resolve<MainViewModel>();
            _mainViewModel.SelectFilesCommand.Execute(null);
            _mainViewModel.OkCancelPanel.OkCommand.Execute(null);
        }

        [Then]
        public void Selected_file_properties_are_cleaned_up()
        {
            _mockedFileCleaner.Verify(x => x.CleanFileProperties(_musicFiles));
        }

        [Then]
        public void Selected_file_names_are_cleaned_up()
        {
            _mockedFileCleaner.Verify(x => x.CleanFileNames(_files));
        }

        [Then]
        public void File_selection_is_cleared()
        {
            Assert.IsNull(_mainViewModel.FileSelection);
        }

        [Then]
        public void Ok_Cancel_panel_is_cleared()
        {
            Assert.IsNull(_mainViewModel.OkCancelPanel);
        }
    }
}
