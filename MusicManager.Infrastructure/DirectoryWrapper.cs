using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicManager.Infrastructure
{
    class DirectoryWrapper : IDirectory
    {
        public List<string> GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(path, searchPattern, searchOption).ToList();
        }

        public FileInfo[] GetFiles(string path, string searchPattern)
        {
            var directoryInfo = new DirectoryInfo(path);
            return directoryInfo.GetFiles();
        }
    }
}
