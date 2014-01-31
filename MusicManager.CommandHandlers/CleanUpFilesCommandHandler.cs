using System.IO;
using MusicManager.Commands;
using MusicManager.Infrastructure;
using MusicManager.Shared;

namespace MusicManager.CommandHandlers
{
    class CleanUpFilesCommandHandler : ICommandHandler<CleanUpFilesCommand>
    {
        private readonly IDirectory _directory;
        private readonly IFileCleaner _fileCleaner;

        private const string Mp3FileSearchPattern = "*.mp3";

        public CleanUpFilesCommandHandler(IDirectory directory, IFileCleaner fileCleaner)
        {
            _directory = directory;
            _fileCleaner = fileCleaner;
        }

        public void Execute(CleanUpFilesCommand command)
        {
            _fileCleaner.CleanFileProperties(command.SelectedFiles);

            FileInfo[] fileInfos = _directory.GetFiles(command.SelectedPath, Mp3FileSearchPattern);
            _fileCleaner.CleanFileNames(fileInfos);
        }
    }
}