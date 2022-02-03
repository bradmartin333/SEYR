using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            SEYR.Pipeline.Appear();
        }

        private void btnOpenComposer_Click(object sender, System.EventArgs e)
        {
            SEYR.Pipeline.Appear();
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
            SEYR.Pipeline.ImageIdx = (int)numFrame.Value;
            SEYR.Pipeline.X = (int)(((double)numFrame.Value) % Math.Sqrt(FRAMECOUNT)) + 1;
            SEYR.Pipeline.Y = (int)(((double)numFrame.Value) / Math.Sqrt(FRAMECOUNT)) + (((int)SEYR.Pipeline.X > 1) ? 1 : 0);
            if (btnOpenGIF.Enabled)
            {
                IMG.SelectActiveFrame(DIM, (int)numFrame.Value - 1);
                Image image = (Image)IMG.Clone();
                Bitmap bitmap = new Bitmap(image);
                SEYR.Pipeline.LoadNewImage(bitmap);
                while (SEYR.Pipeline.Working)
                    Application.DoEvents();
            }
            else if (btnOpenDir.Enabled)
            {
                SEYR.Pipeline.LoadNewImage(new Bitmap(IMGS[(int)(numFrame.Value - 1)]));
                while (SEYR.Pipeline.Working)
                    Application.DoEvents();
            }

            string lastData = SEYR.Pipeline.GetData();
            if (!string.IsNullOrEmpty(lastData))
            {
                string[] dataLines = lastData.Split('\n');
                int pass = dataLines.Where(x => !string.IsNullOrEmpty(x) && x.Split('\t')[4] == "Pass").Count();
                System.Diagnostics.Debug.WriteLine($"Pass: {pass}\tYield: {Math.Round(pass / (double)dataLines.Length * 100, 1)}%");
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
            for (int i = (int)numFrame.Value; i < FRAMECOUNT; i++)
            {
                if (STOP)
                {
                    STOP = false;
                    return;
                }
                numFrame.Value++;
                Application.DoEvents();
            }
        }

        private void numPatternFollowInterval_ValueChanged(object sender, System.EventArgs e)
        {
            SEYR.Pipeline.PatternFollowInterval = (int)numPatternFollowInterval.Value;
        }

        private void numPatternFollowDelay_ValueChanged(object sender, System.EventArgs e)
        {
            SEYR.Pipeline.PatternFollowDelay = (int)numPatternFollowDelay.Value;
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            SEYR.Pipeline.ClearOutput(reloadImage: true);
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
                filesFound.AddRange(Directory.GetFiles(searchFolder, string.Format("*.{0}", filter), SearchOption.TopDirectoryOnly));
            return filesFound.AlphanumericSort();
        }
    }
}
