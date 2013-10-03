namespace MusicManager
{
    public interface IFileCleaner
    {
        void CleanFileNames(System.IO.FileInfo[] infos);
        void CleanFileProperties(string[] files);
    }
}
