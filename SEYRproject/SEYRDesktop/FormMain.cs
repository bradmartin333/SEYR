﻿using System;
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
            ProgressBar.Value = (int)NumFrame.Value;
            if (NumFrame.Value == 0)
            {
                Channel.ResetAll();
                return;
            }
            if (!BtnRunAll.Enabled || BUSY) return;
            await NextImage();
        }

        private async Task NextImage(bool forcePattern = false)
        {
            if (NumFrame.Value == 0) return;

            BUSY = true;
            string imagePath = IMGS[(int)NumFrame.Value - 1];
            Bitmap bmp = new Bitmap(imagePath);

            FileInfo fileInfo = new FileInfo(imagePath);
            string name = fileInfo.Name.Replace(fileInfo.Extension, "");
            _ = await Channel.NewImage(bmp, forcePattern, $"{NumFrame.Value}\t{string.Join("\t", name.Replace("A", "").Replace("B", "").Replace("C", "").Replace("X", "").Replace("Y", "").Split('_'))}\t");

            ProgressBar.Value = (int)NumFrame.Value;
            BUSY = false;
            GC.Collect();
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
            NumPxPerMicron.Value = (decimal)Channel.PxPerMicron;
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
                Channel.MakeArchive();
                Channel.SignalComplete();
            }
            STOP = false;
        }

        private async void BtnRunAll_Click(object sender, EventArgs e)
        {
            BtnRunAll.Enabled = false;
            BtnRestartAndRun.Enabled = false;
            BtnStop.Enabled = true;
            NumPxPerMicron.Value = (decimal)Channel.PxPerMicron;
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
                Channel.MakeArchive();
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

            SEYR.Session.Channel channel = SEYR.Session.Channel.OpenSEYR();
            if (channel != null)
            {
                Channel = channel;
                Channel.SetPixelsPerMicron((float)NumPxPerMicron.Value);
                Channel.SetDefaultChroma(Color.Red);
                Channel.SetFeatureScoreHistorySize(10000);
            }
            else
                return;
            
            NumPxPerMicron.Enabled = false;
            BtnOpenDir.Enabled = false;
            BtnOpenComposer.Enabled = true;
            NumFrame.Enabled = true;
            BtnRunAll.Enabled = true;
            BtnRestartAndRun.Enabled = true;
            BtnForcePattern.Enabled = true;
            BtnOpenDir.BackColor = Color.LightGreen;
            
            NumFrame.Maximum = IMGS.Length;
            NumFrame.Value = 0;
            ProgressBar.Maximum = IMGS.Length;
        }

        private void BtnOpenComposer_Click(object sender, EventArgs e)
        {
            if (NumFrame.Value == 0)
            {
                NumFrame.Value++;
                Application.DoEvents();
            }
            Channel.OpenComposer(new Bitmap(IMGS[(int)NumFrame.Value - 1]));
        }

        private string OpenFolder()
        {
            string lastFolder = Properties.Settings.Default.OpenDirRoot;
            lastFolder = Directory.Exists(lastFolder) ? lastFolder : null;
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (!string.IsNullOrEmpty(lastFolder))
                    folderBrowserDialog.SelectedPath = lastFolder;
                folderBrowserDialog.Description = "Open a directory containing photos";
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default.OpenDirRoot = folderBrowserDialog.SelectedPath;
                    Properties.Settings.Default.Save();
                    return folderBrowserDialog.SelectedPath;
                }  
            }
            return null;
        }

        private static IEnumerable<string> GetSortedPicturesFrom(string searchFolder)
        {
            string[] filters = new string[] { "jpg", "jpeg", "png", "bmp" };
            List<string> filesFound = new List<string>();
            foreach (var filter in filters)
                filesFound.AddRange(Directory.GetFiles(searchFolder, string.Format("*.{0}", filter), SearchOption.AllDirectories));
            string patFile = string.Empty;
            foreach (var file in filesFound)
                if (file.Contains("SEYRpattern")) patFile = file;
            if (!string.IsNullOrEmpty(patFile)) filesFound.Remove(patFile);
            return filesFound;
        }

        private async void BtnForcePattern_Click(object sender, EventArgs e)
        {
            await NextImage(true);
            Channel.ShowViewer();
        }
    }
}
