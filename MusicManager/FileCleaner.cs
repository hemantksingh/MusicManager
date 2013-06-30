using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace MusicManager
{
    class FileCleaner : MusicManager.IFileCleaner
    {
        public void CleanFileProperties(string[] files)
        {
            foreach (string filePath in files)
            {
                string stringToReplace = "www.Songs.PK";

                TagLib.File tagFile = TagLib.File.Create(filePath);
                string artist = tagFile.Tag.FirstAlbumArtist;

                Type tag = tagFile.Tag.GetType();

                foreach (PropertyInfo property in tag.GetProperties())
                {
                    if (property.PropertyType == typeof(string) && property.CanWrite)
                    {
                        string propertyValue = (string)property.GetValue(tagFile.Tag, null);
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
            string stringToReplace = "Songs.PK";

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
