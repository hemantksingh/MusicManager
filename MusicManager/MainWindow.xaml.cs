using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

namespace MusicManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string path = GetSelectedpath();

            if (!string.IsNullOrEmpty(path))
            {
                FileNameCleaner converter = new FileNameCleaner();
                converter.Convert(Directory.GetFiles(path, "*.mp3"));

                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] infos = d.GetFiles("*.mp3");
                converter.Convert(infos);
            }
        }

        private static string GetSelectedpath()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            //dlg.FileName = "Music Files";
            //dlg.DefaultExt = ".mp3";
            //dlg.Filter = "Music files (.mp3)| *.mp3";

            dlg.Description = "Select directory to choose from";
            dlg.ShowNewFolderButton = true;
            var result = dlg.ShowDialog();

            string path = string.Empty;

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                path = dlg.SelectedPath;
            }
            return path;
        }
    }
}
