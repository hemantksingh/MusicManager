using System;
using System.IO;
using System.Reflection;

namespace MusicManager
{
    class FileCleaner : IFileCleaner
    {
        public void CleanFileProperties(string[] files)
        {
            foreach (string filePath in files)
            {
                const string stringToReplace = "www.Songs.PK";

                TagLib.File tagFile = TagLib.File.Create(filePath);

                Type tag = tagFile.Tag.GetType();

                foreach (PropertyInfo property in tag.GetProperties())
                {
                    if (property.PropertyType == typeof(string) && property.CanWrite)
                    {
                        var propertyValue = (string)property.GetValue(tagFile.Tag, null);
                        if(!string.IsNullOrEmpty(propertyValue))
                        {
                            string propertyNewValue = propertyValue.Remove("-[]".ToCharArray())
                                .Replace(stringToReplace, string.Empty);
                            property.SetValue(tagFile.Tag, propertyNewValue, null);
                        }
                    }
                }
                
                tagFile.Save();
            }
        }

        public void CleanFileNames(FileInfo[] infos)
        {
            const string stringToReplace = "Songs.PK";

            foreach (FileInfo fileInfo in infos)
            {
                string cleanedFileName = fileInfo.Name.Remove("[]".ToCharArray());

                if (cleanedFileName.Contains(stringToReplace) || cleanedFileName != fileInfo.Name)
                {
                    string destFileName = Path.Combine(fileInfo.Directory.ToString(),
                        cleanedFileName.Replace(stringToReplace, string.Empty));

                    File.Move(fileInfo.FullName, destFileName);
                }
            }
        }
    }
}
