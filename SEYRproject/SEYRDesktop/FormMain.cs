using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEYRDesktop
{
    public partial class FormMain : Form
    {
        private SEYR.Session.Channel Channel;
        private string[] IMGS = null;
        private bool STOP;
        private bool BUSY;

        public FormMain()
        {
            InitializeComponent();
        }

        private async void numFrame_ValueChanged(object sender, EventArgs e)
        {
            if (!BtnRunAll.Enabled || BUSY) return;
            await NextImage();
        }

        private async Task NextImage(bool forcePattern = false)
        {
            BUSY = true;
            SEYR.Session.Channel.OutputData = $"{NumFrame.Value}\t0\t0\t0\t0\t0\t0\t0\t0\t";
            Bitmap bmp = new Bitmap(IMGS[(int)NumFrame.Value]);
            string info = await Channel.NewImage(bmp, forcePattern);
            System.Diagnostics.Debug.WriteLine($"{NumFrame.Value}\t{info}");
            BUSY = false;
            GC.Collect();
        }

        private void BtnOpenComposer_Click(object sender, EventArgs e)
        {
            Channel.OpenComposer(new Bitmap(IMGS[(int)NumFrame.Value]));
        }

        private async void BtnRestartAndRun_Click(object sender, EventArgs e)
        {
            BUSY = true;
            STOP = false;
            BtnRunAll.Enabled = false;
            BtnStop.Enabled = false;
            BtnStop.Enabled = true;
            NumFrame.Value = 0;
            Application.DoEvents();
            Channel.MakeArchive();
            SEYR.Session.Channel.ClearLogs();
            SEYR.Session.Channel.ClearAllFeatureScores();
            BUSY = false;
            while (!STOP && NumFrame.Value < NumFrame.Maximum)
            {
                Application.DoEvents();
                await NextImage();
                NumFrame.Value++;
            }
            BtnRunAll.Enabled = true;
            BtnRestartAndRun.Enabled = true;
            BtnStop.Enabled = false;
            if (!STOP) Channel.MakeArchive();
            STOP = false;
        }

        private async void btnRunAll_Click(object sender, EventArgs e)
        {
            BtnRunAll.Enabled = false;
            BtnRestartAndRun.Enabled = false;
            BtnStop.Enabled = true;
            while (!STOP && NumFrame.Value < NumFrame.Maximum)
            {
                Application.DoEvents();
                await NextImage();
                NumFrame.Value++;
            }
            BtnRunAll.Enabled = true;
            BtnRestartAndRun.Enabled = true;
            BtnStop.Enabled = false;
            if (!STOP) Channel.MakeArchive();
            STOP = false;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            STOP = true;
            BtnRunAll.Enabled = true;
            BtnRestartAndRun.Enabled = true;
            BtnStop.Enabled = false;
        }

        private async void btnOpenDir_Click(object sender, EventArgs e)
        {
            string path = OpenFolder();
            if (path == null) return;
            
            IMGS = GetSortedPicturesFrom(path).ToArray();
            
            string[] files = Directory.GetFiles(path, "*.seyr");
            if (files.Length > 0)
                Channel = new SEYR.Session.Channel(path);
            else
                Channel = new SEYR.Session.Channel(path, (float)NumPxPerMicron.Value);

            NumPxPerMicron.Enabled = false;
            BtnOpenDir.Enabled = false;
            BtnOpenComposer.Enabled = true;
            NumFrame.Enabled = true;
            BtnRunAll.Enabled = true;
            BtnRestartAndRun.Enabled = true;
            BtnRepeat.Enabled = true;
            BtnShowViewer.Enabled = true;
            BtnForcePattern.Enabled = true;
            BtnOpenDir.BackColor = Color.LawnGreen;
            
            NumFrame.Maximum = IMGS.Length - 1;
            NumFrame.Value = 0;

            await NextImage();
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

        private static IEnumerable<string> GetSortedPicturesFrom(string searchFolder)
        {
            string[] filters = new string[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
            List<string> filesFound = new List<string>();
            foreach (var filter in filters)
                filesFound.AddRange(Directory.GetFiles(searchFolder, string.Format("*.{0}", filter), SearchOption.AllDirectories));
            string patFile = string.Empty;
            foreach (var file in filesFound)
                if (file.Contains("SEYRpattern")) patFile = file;
            if (!string.IsNullOrEmpty(patFile)) filesFound.Remove(patFile);
            return filesFound.AlphanumericSort();
        }

        private async void BtnRepeat_Click(object sender, EventArgs e)
        {
            await NextImage();
        }

        private void BtnShowViewer_Click(object sender, EventArgs e)
        {
            Channel.ShowViewer();
        }

        private async void BtnForcePattern_Click(object sender, EventArgs e)
        {
            await NextImage(true);
        }
    }
}
