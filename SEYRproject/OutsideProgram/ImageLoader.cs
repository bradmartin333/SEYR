using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SEYR;

namespace OutsideProgram
{
    public partial class ImageLoader : Form
    {
        private string DirectoryPath { get; set; } = @"S:\SEYR\SEYRproject\GenerateGridImages\bin\Debug\OutputImages";
        private string[] Images { get; set; }
        private int ImageIndex { get; set; } = -1;
        private Composer Composer { get; set; }

        public ImageLoader(Composer composer)
        {
            InitializeComponent();

            Images = GetImagesFrom(DirectoryPath);
            progressBar.Maximum = Images.Length;

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
            DataHandler.OutputString = "";
        }

        private async void btnNextImage_Click(object sender, EventArgs e)
        {
            await LoadNewImage();
        }

        private async void btnRunAll_Click(object sender, EventArgs e)
        {
            while (true)
            {
                bool result = await LoadNewImage();
                if (!result) break;
            }

            File.WriteAllText(DirectoryPath + "\\" + "report.txt", DataHandler.OutputString);
        }

        private async Task<bool> LoadNewImage()
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
                await Composer.LoadNewImage(new Bitmap(Images[ImageIndex]));
                progressBar.Value = ImageIndex;
                return true;
            }
        }

        private string OpenDirectory(string description)
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
