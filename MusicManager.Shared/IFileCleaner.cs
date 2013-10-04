namespace MusicManager.Shared
{
    public interface IFileCleaner
    {
        void CleanFileNames(System.IO.FileInfo[] infos);
        void CleanFileProperties(string[] files);
    }
}
