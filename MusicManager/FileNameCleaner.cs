using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace MusicManager
{
    class FileNameCleaner
    {
        public void Convert(string[] files)
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
                            string propertyNewValue = propertyValue.Remove("-[]".ToCharArray()).Replace(stringToReplace, string.Empty);
                            property.SetValue(tagFile.Tag, propertyNewValue, null);
                        }
                    }
                }

                //if (!string.IsNullOrEmpty(tagFile.Tag.Title))
                //{
                //    string title = tagFile.Tag.Title.Clean(stripDash: true);
                //    tagFile.Tag.Title = title.Replace(stringToReplace, string.Empty);
                //}



                //if (!string.IsNullOrEmpty(tagFile.Tag.Album))
                //{
                //    string album = tagFile.Tag.Album.Clean(stripDash: true);
                //    tagFile.Tag.Album = album.Replace(stringToReplace, string.Empty);
                //}

                //if (!string.IsNullOrEmpty(tagFile.Tag.Comment))
                //{
                //    string comment = tagFile.Tag.Comment.Clean(stripDash: true);
                //    tagFile.Tag.Comment = comment.Replace(stringToReplace, string.Empty);
                //}

                //if (!string.IsNullOrEmpty(tagFile.Tag.Copyright))
                //{
                //    string copyright = tagFile.Tag.Copyright.Clean(stripDash: true);
                //    tagFile.Tag.Copyright = copyright.Replace(stringToReplace, string.Empty);
                //}

                //if (!string.IsNullOrEmpty(tagFile.Tag.TitleSort))
                //{
                //    string titleSort = tagFile.Tag.TitleSort.Clean(stripDash: true);
                //    tagFile.Tag.TitleSort = titleSort.Replace(stringToReplace, string.Empty);
                //}

                tagFile.Save();
            }
        }

        internal void Convert(FileInfo[] infos)
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
