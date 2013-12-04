using System.Collections.Generic;
using System.IO;

namespace MusicManager.Infrastructure
{
    public interface IDirectory
    {
        List<string> GetFiles(string path, string searchPattern, SearchOption searchOption);
        FileInfo[] GetFiles(string path, string searchPattern);
    }
}