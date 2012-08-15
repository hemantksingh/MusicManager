using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace MusicManager
{
    class FileReader
    {

        public void Read(string filePath)
        {
            TagLib.File tagFile = TagLib.File.Create(filePath);
            string artist = tagFile.Tag.FirstAlbumArtist;
            string album = tagFile.Tag.Album;
            string title = tagFile.Tag.Title;
        }
    }
}
