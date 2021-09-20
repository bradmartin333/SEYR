using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SEYR
{
    public static class FileHandler
    {
        public static Viewer Viewer = new Viewer();
        public static Grid Grid = new Grid();
        public static string FilePath { get; set; } = string.Empty;
        public static int ImageIdx { get; set; }
        public static string[] Images { get; set; }
        public static int PatternFollowInterval { get; set; } = 1;

        private static string _ImageDirectoryPath = string.Empty;
        public static string ImageDirectoryPath
        {
            get { return _ImageDirectoryPath; }
            set
            {
                _ImageDirectoryPath = value;
                string[] buffer = GetImagesFrom(_ImageDirectoryPath);
                if (buffer.Length == 0)
                {
                    MessageBox.Show("Invalid Directory Contents");
                    return;
                }
                DataHandler.OutputString = "";
                ImageIdx = 0;
                Images = buffer;
            }
        }

        public static string LoadFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open Simple Entropy Yield Routine";
                openFileDialog.Filter = "SEYR (*.seyr) | *.seyr";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    return openFileDialog.FileName;
            }
            return null;
        }

        public static string SaveFile()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Title = "Save Simple Entropy Yield Routine";
                saveFileDialog.Filter = "SEYR (*.seyr) | *.seyr";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    return saveFileDialog.FileName;
            }
            return null;
        }

        public static void WriteParametersToBinaryFile()
        {
            using (Stream stream = File.Open(FilePath, false ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, Grid);
            }
        }

        public static void ReadParametersFromBinaryFile()
        {
            using (Stream stream = File.Open(FilePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                Grid = (Grid)binaryFormatter.Deserialize(stream);
            }
        }

        public static string OpenDirectory(string description)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = description;
                folderBrowserDialog.ShowNewFolderButton = false;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    return folderBrowserDialog.SelectedPath;
            }
            return null;
        }

        private static string[] GetImagesFrom(string searchFolder)
        {
            List<string> filesFound = new List<string>();
            SearchOption searchOption = SearchOption.AllDirectories;
            string[] filters = new string[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, string.Format("*.{0}", filter), searchOption));
            }
            string[] fileArr = filesFound.ToArray();
            NumericComparer ns = new NumericComparer();
            Array.Sort(fileArr, ns);
            return fileArr;
        }
    }
}
