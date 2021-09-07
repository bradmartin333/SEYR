using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SEYR
{
    public partial class ImageLoader : Form
    {
        private string DirectoryPath { get; set; }
        private string[] Images { get; set; }
        private int ImageIndex { get; set; }
        private Composer Composer { get; set; }

        public ImageLoader(Composer composer)
        {
            InitializeComponent();
            Composer = composer;
            Show();
        }

        private void btnOpenDir_Click(object sender, EventArgs e)
        {
            string pathBuffer = OpenDirectory("Select a directory containing target images");
            if (pathBuffer == null)
                return;
            else
                DirectoryPath = pathBuffer;

            Images = GetImagesFrom(DirectoryPath);
            if (Images.Length == 0)
            {
                progressBar.Value = 0;
                MessageBox.Show("Invalid Directory Contents");
                return;
            }

            progressBar.Maximum = Images.Length;
            ImageIndex = -1;
        }

        private void btnNextImage_Click(object sender, EventArgs e)
        {
            LoadNewImage();
        }

        private void btnRunAll_Click(object sender, EventArgs e)
        {
            while (LoadNewImage()) { Application.DoEvents(); }
        }

        private bool LoadNewImage()
        {
            ImageIndex++;
            if (ImageIndex > Images.Length - 1)
            {
                ImageIndex = 0;
                progressBar.Value = ImageIndex;
                return false;
            }
            else
            {
                Composer.NewImage(new Bitmap(Images[ImageIndex]), ImageIndex);
                progressBar.Value = ImageIndex;
                return true;
            }
        }

        private string OpenFile(string title, string filter)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = title;
                openFileDialog.Filter = filter;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    return openFileDialog.FileName;
            }
            return null;
        }

        private string SaveFile(string title, string filter)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Title = title;
                saveFileDialog.Filter = filter;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    return saveFileDialog.FileName;
            }
            return null;
        }

        private string OpenDirectory(string description)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = description;
                folderBrowserDialog.ShowNewFolderButton = false;
                folderBrowserDialog.SelectedPath = DirectoryPath;
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
