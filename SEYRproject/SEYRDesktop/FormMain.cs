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
        private readonly List<string> Data = new List<string>();
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

            string data = $"{NumFrame.Value}\t0\t0\t0\t0\t0\t0\t0\t0\t";
            string matchString = imagePath.Split('_').Last().Replace(".png", "").Replace("R", "").Replace("C", "").Replace("S", "").Replace(" ", "").Replace(",", "\t");
            string[] dataMatches = Data.Where(x => 
                x.Contains(matchString)).ToArray();
            if (dataMatches.Any())data = dataMatches[0];
            
            string info = await Channel.NewImage(bmp, forcePattern, data);
            //System.Diagnostics.Debug.WriteLine($"{NumFrame.Value}\t{info}");
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
            if (!STOP)
            {
                Channel.MakeArchive(true);
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
                Channel.MakeArchive(true);
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

        private async void BtnOpenDir_Click(object sender, EventArgs e)
        {
            string path = OpenFolder();
            if (path == null) return;
            
            IMGS = GetSortedPicturesFrom(path).ToArray();
            
            string[] files = Directory.GetFiles(path, "*.seyr");
            if (files.Length > 0)
                Channel = new SEYR.Session.Channel(path);
            else
                Channel = new SEYR.Session.Channel(path, (float)NumPxPerMicron.Value);

            files = Directory.GetFiles(path, "Inlinepositions.txt");
            if (files.Length > 0)
            {
                string[] lines = File.ReadAllLines(files[0]);
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] cols = lines[i].Split('\t');
                    if (cols.Length > 0) Data.Add($"{i - 1}\t{cols[2]}\t{cols[3]}\t{cols[4]}\t{cols[5]}\t{cols[6]}\t{cols[7]}\t{cols[8]}\t{cols[9]}\t");
                }
            }

            NumPxPerMicron.Enabled = false;
            BtnOpenDir.Enabled = false;
            BtnOpenComposer.Enabled = true;
            NumFrame.Enabled = true;
            BtnRunAll.Enabled = true;
            BtnRestartAndRun.Enabled = true;
            BtnRepeat.Enabled = true;
            BtnShowViewer.Enabled = true;
            BtnForcePattern.Enabled = true;
            BtnCustomFilter.Enabled = true;
            BtnOpenDir.BackColor = Color.LawnGreen;
            
            NumFrame.Maximum = IMGS.Length - 1;
            NumFrame.Value = 0;
            ProgressBar.Maximum = IMGS.Length - 1;

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

        private async void BtnCustomFilter_Click(object sender, EventArgs e)
        {
            BUSY = true;
            Bitmap bmp = new Bitmap(IMGS[(int)NumFrame.Value]);
            string info = await Channel.NewImage(bmp, customFilter: true);
            System.Diagnostics.Debug.WriteLine($"{NumFrame.Value}\t{info}");
            BUSY = false;
            GC.Collect();
            Form form = new Form()
            {
                BackgroundImage = SEYR.Session.Channel.CustomImage,
                BackgroundImageLayout = ImageLayout.Zoom,
                Text = $"{info} Blobs Found",
            };
            form.Show();
        }
    }
}
