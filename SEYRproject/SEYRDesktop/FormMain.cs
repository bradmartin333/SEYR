using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SEYRDesktop
{
    public partial class FormMain : Form
    {
        private SEYR.Session.Channel Channel;
        private string[] IMGS = null;
        private bool STOP;

        public FormMain()
        {
            InitializeComponent();
        }

        private void numFrame_ValueChanged(object sender, EventArgs e)
        {
            NextImage();
        }

        private void NextImage()
        {
            Bitmap bmp = new Bitmap(IMGS[(int)NumFrame.Value]);
            Channel.NewImage(bmp);
        }

        private void BtnLaunchWizard_Click(object sender, EventArgs e)
        {
            Channel.RunWizard(new Bitmap(IMGS[(int)NumFrame.Value]));
        }

        private void btnRunAll_Click(object sender, EventArgs e)
        {
            BtnRunAll.Enabled = false;
            BtnStop.Enabled = true;
            while (!STOP && NumFrame.Value < NumFrame.Maximum)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
                NextImage();
                NumFrame.Value++;
            }
            BtnRunAll.Enabled = true;
            BtnStop.Enabled = false;
            if (!STOP) NumFrame.Value = 0;
            STOP = false;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            STOP = true;
            BtnRunAll.Enabled = true;
            BtnStop.Enabled = false;
        }

        private void btnOpenDir_Click(object sender, EventArgs e)
        {
            string path = OpenFolder();
            if (path == null) return;
            
            IMGS = GetSortedPicturesFrom(path).ToArray();
            
            string[] files = Directory.GetFiles(path, "*.seyr");
            if (files.Length > 0)
                Channel = new SEYR.Session.Channel(files[0], $@"{path}\data.txt", $@"{path}\debug.txt");
            else
                Channel = new SEYR.Session.Channel($@"{path}\project.seyr", $@"{path}\data.txt", (double)NumPxPerMicron.Value, $@"{path}\debug.txt");
            
            BtnOpenDir.Enabled = false;
            BtnLaunchWizard.Enabled = true;
            NumPxPerMicron.Enabled = false;
            NumFrame.Enabled = true;
            BtnRunAll.Enabled = true;
            BtnRepeat.Enabled = true;
            BtnShowComposite.Enabled = true;
            BtnOpenDir.BackColor = Color.LawnGreen;
            
            NumFrame.Maximum = IMGS.Length - 1;
            NumFrame.Value = 0;
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
            return filesFound.AlphanumericSort();
        }

        private void BtnRepeat_Click(object sender, EventArgs e)
        {
            NextImage();
        }

        private void BtnShowComposite_Click(object sender, EventArgs e)
        {
            Channel.ShowComposite();
        }
    }
}
