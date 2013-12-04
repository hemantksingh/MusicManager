using System.Collections.Generic;

namespace MusicManager.Shared
{
    public interface IFileCleaner
    {
        void CleanFileNames(System.IO.FileInfo[] infos);
        void CleanFileProperties(List<string> files);
    }
}
