﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MusicManager
{
    class DisplayService : IDisplayService
    {
        public string DisplayFolderBrowserDialogue()
        {
            var dialog = new FolderBrowserDialog();

            //dlg.FileName = "Music Files";
            //dlg.DefaultExt = ".mp3";
            //dlg.Filter = "Music files (.mp3)| *.mp3";

            dialog.Description = "Select directory to choose from";
            dialog.ShowNewFolderButton = true;
            var result = dialog.ShowDialog();

            string path = string.Empty;

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.SelectedPath;
            }
            return path;
        }
    }
}