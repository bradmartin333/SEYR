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

        private async void NumFrame_ValueChanged(object sender, EventArgs e)
        {
            if (!BtnRunAll.Enabled || BUSY) return;
            await NextImage();
        }

        private async Task NextImage(bool forcePattern = false)
        {
            BUSY = true;
            string imagePath = IMGS[(int)NumFrame.Value];
            Bitmap bmp = new Bitmap(imagePath);

            FileInfo fileInfo = new FileInfo(imagePath);
            string name = fileInfo.Name.Replace(fileInfo.Extension, "");
            string[] cols = name.Split('_');
            double.TryParse(cols[0], out double x);
            double.TryParse(cols[1], out double y);
            string data = $"{NumFrame.Value}\t{x}\t{y}\t";
            
            _ = await Channel.NewImage(bmp, forcePattern, data, CbxStampInspection.Checked);

            ProgressBar.Value = (int)NumFrame.Value;
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
            Channel.ResetAll();
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
            if (!STOP)
            {
                if (!CbxStampInspection.Checked) Channel.MakeArchive();
                Channel.SignalComplete();
                ProgressBar.Value = 0;
            }
            STOP = false;
        }

        private async void BtnRunAll_Click(object sender, EventArgs e)
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
            if (!STOP)
            {
                if (!CbxStampInspection.Checked) Channel.MakeArchive();
                Channel.SignalComplete();
                ProgressBar.Value = 0;
            }
            STOP = false;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            STOP = true;
            BtnRunAll.Enabled = true;
            BtnRestartAndRun.Enabled = true;
            BtnStop.Enabled = false;
        }

        private void BtnOpenDir_Click(object sender, EventArgs e)
        {
            string path = OpenFolder();
            if (path == null) return;

            IMGS = GetSortedPicturesFrom(path).ToArray();
            if (IMGS.Length == 0)
            {
                MessageBox.Show("No pictures found -- try again");
                return;
            }

            SEYR.Session.Channel channel = SEYR.Session.Channel.OpenSEYR("ImageNumber\tX\tY\t");
            if (channel != null)
            {
                Channel = channel;
                Channel.SetPixelsPerMicron((float)NumPxPerMicron.Value);
            }
            
            NumPxPerMicron.Enabled = false;
            BtnOpenDir.Enabled = false;
            BtnOpenComposer.Enabled = true;
            NumFrame.Enabled = true;
            BtnRunAll.Enabled = true;
            BtnRestartAndRun.Enabled = true;
            BtnRepeat.Enabled = true;
            BtnForcePattern.Enabled = true;
            BtnOpenDir.BackColor = Color.LightGreen;
            
            NumFrame.Maximum = IMGS.Length - 1;
            NumFrame.Value = 0;
            ProgressBar.Maximum = IMGS.Length - 1;
        }

        private string OpenFolder()
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Open a directory containing photos";
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

        private async void BtnForcePattern_Click(object sender, EventArgs e)
        {
            await NextImage(true);
        }

        private void CbxStampInspection_CheckedChanged(object sender, EventArgs e)
        {
            if (CbxStampInspection.Checked) Channel.InputParameters(new Bitmap(IMGS[(int)NumFrame.Value]));
        }
    }
}
