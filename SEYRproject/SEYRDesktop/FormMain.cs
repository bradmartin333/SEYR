using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SEYRDesktop
{
    public partial class FormMain : Form
    {
        Image IMG = null;
        FrameDimension DIM = null;
        string[] IMGS = null;
        int FRAMECOUNT = 0;
        bool STOP = false;

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnOpenComposer_Click(object sender, System.EventArgs e)
        {
        }

        private void btnOpenGIF_Click(object sender, System.EventArgs e)
        {
            string path = OpenFile("Open a GIF", "GIF file (*.gif)|*.gif");
            if (path == null)
                return;

            btnOpenDir.Enabled = false;

            btnOpenGIF.BackColor = Color.LawnGreen;

            IMG = Image.FromFile(path);
            DIM = new FrameDimension(IMG.FrameDimensionsList[0]);
            FRAMECOUNT = IMG.GetFrameCount(DIM);

            if (FRAMECOUNT > 100) MessageBox.Show("This is a large GIF, consider using a Dir of images instead.");

            numFrame.Maximum = FRAMECOUNT;
            numFrame.Value = 1;
            NextImage();
        }

        private void btnOpenDir_Click(object sender, EventArgs e)
        {
            string path = OpenFolder();
            if (path == null)
                return;

            btnOpenGIF.Enabled = false;

            btnOpenDir.BackColor = Color.LawnGreen;

            IMGS = GetSortedPicturesFrom(path).ToArray();
            FRAMECOUNT = IMGS.Count();

            numFrame.Maximum = FRAMECOUNT;
            numFrame.Value = 1;
            NextImage();
        }

        private void numFrame_ValueChanged(object sender, System.EventArgs e)
        {
            NextImage();
        }

        private void NextImage()
        {

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

        private string OpenFolder()
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                openFileDialog.Title = "Open a directory containing photos";
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    return folderBrowserDialog.SelectedPath;
            }
            return null;
        }

        private void btnRunAll_Click(object sender, System.EventArgs e)
        {

        }

        private void numPatternFollowInterval_ValueChanged(object sender, System.EventArgs e)
        {

        }

        private void btnClearData_Click(object sender, EventArgs e)
        {

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            STOP = true;
        }

        public static IEnumerable<string> GetSortedPicturesFrom(string searchFolder)
        {
            string[] filters = new string[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
            List<string> filesFound = new List<string>();
            foreach (var filter in filters)
                filesFound.AddRange(Directory.GetFiles(searchFolder, string.Format("*.{0}", filter), SearchOption.AllDirectories));
            return filesFound.AlphanumericSort();
        }
    }
}
