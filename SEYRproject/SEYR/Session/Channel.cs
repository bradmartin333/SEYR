﻿using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;
using SEYR.ImageProcessing;
using System.Threading.Tasks;
using System;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Linq;

namespace SEYR.Session
{
    public class Channel
    {
        /// <summary>
        /// Backup for situations where NewImage cannot be awaited
        /// </summary>
        public bool Working { get; set; } = false;
        /// <summary>
        /// Storage for a filter processed with a custom filter
        /// </summary>
        public static Bitmap CustomImage { get; set; } = new Bitmap(1, 1);
        internal static Project Project { get; set; } = null;
        internal static DataStream DataStream { get; set; } = null;
        internal static DataStream DebugStream { get; set; } = null;
        internal static Viewer Viewer { get; set; }
        internal static Bitmap Pattern { get; set; } = null;
        internal static string PatternPath { get; set; } = null;
        internal static string DirPath { get; set; } = null;
        internal static bool IsNewProject { get; set; } = false;
        private static string ProjectPath { get; set; } = null;

        /// <summary>
        /// Create a new SEYR Channel
        /// </summary>
        /// <param name="projectDir"></param>
        /// <param name="pixelsPerMicron"></param>
        /// <param name="dataHeader"></param>
        public Channel(string projectDir, float pixelsPerMicron, string dataHeader = "ImageNumber\tX\tY\tRR\tRC\tR\tC\tSR\tSC\t")
        {
            IsNewProject = true;
            DirPath = projectDir;
            DataStream = new DataStream(DirPath + @"\SEYRreport.txt", dataHeader);
            DebugStream = new DataStream(DirPath + @"\SEYRdebug.txt", isDebug: true);
            ProjectPath = DirPath + @"\project.seyr";
            PatternPath = DirPath + @"\SEYRpattern.png";
            Project = new Project() { PixelsPerMicron = pixelsPerMicron };
            SaveProject();
            Viewer = new Viewer();
        }

        /// <summary>
        /// Open an existing SEYR Channel
        /// </summary>
        /// <param name="projectDir"></param>
        /// <param name="dataHeader">
        /// </param>
        public Channel(string projectDir, string dataHeader = "ImageNumber\tX\tY\tRR\tRC\tR\tC\tSR\tSC\t")
        {
            DirPath = projectDir;
            DataStream = new DataStream(DirPath + @"\SEYRreport.txt", dataHeader);
            DebugStream = new DataStream(DirPath + @"\SEYRdebug.txt", isDebug: true);
            ProjectPath = DirPath + @"\project.seyr";
            PatternPath = DirPath + @"\SEYRpattern.png";
            LoadProject();
            Viewer = new Viewer();
        }

        /// <summary>
        /// Clear the Debug and Report logs
        /// </summary>
        public static void ClearLogs()
        {
            if (!string.IsNullOrEmpty(DataStream.Path)) DataStream = new DataStream(DataStream.Path, DataStream.BaseHeader);
            if (!string.IsNullOrEmpty(DebugStream.Path)) DebugStream = new DataStream(DebugStream.Path, isDebug: true);
            DiscardViewer();
        }

        /// <summary>
        /// Clear all score history for all features in project
        /// </summary>
        public static void ClearAllFeatureScores()
        {
            DebugStream.Write($"User Reset Score History");
            foreach (Feature feature in Project.Features)
                feature.ClearScore();
            SaveProject();
            DiscardViewer();
        }

        #region Opening and Closing

        /// <summary>
        /// Allow user to chose directory for SEYR operation
        /// </summary>
        /// <returns>
        /// A SEYR channel or null depending on completion of the folder browser dialog
        /// </returns>
        public static Channel OpenSEYR()
        {
            Channel channel = null;
            FolderBrowserDialog fbd = new FolderBrowserDialog { Description = "Open a directory for SEYR operations" };
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(fbd.SelectedPath, "*.seyr");
                if (files.Length > 0)
                    channel= new Channel(fbd.SelectedPath);
                else
                    channel = new Channel(fbd.SelectedPath, 1f);
            }
            return channel;
        }

        private static void SaveProject()
        {
            using (StreamWriter stream = new StreamWriter(ProjectPath))
            {
                XmlSerializer x = new XmlSerializer(typeof(Project));
                x.Serialize(stream, Project);
            }
            DebugStream.Write("Project Saved", addDT: true);
            DiscardViewer();
        }

        private void LoadProject()
        {
            DebugStream.Write($"Loading Project\t{ProjectPath}", addDT: true);
            using (StreamReader stream = new StreamReader(ProjectPath))
            {
                XmlSerializer x = new XmlSerializer(typeof(Project));
                try
                {
                    Project = (Project)x.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Corrupt SEYR Project. Loading default project.", "SEYR");
                    DebugStream.Write($"Corrupt SEYR Project: {ex}", addDT: true);
                    Project = new Project();
                    DebugStream.Write("Default project loaded");
                    return;
                }
            }
            DebugStream.Write("Project Loaded", addDT: true);
            LoadPattern();
        }

        private void LoadPattern()
        {
            if (File.Exists(PatternPath))
            {
                Bitmap bmp = new Bitmap(PatternPath);
                Pattern = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format24bppRgb);
                using (Graphics g = Graphics.FromImage(Pattern))
                    g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                DebugStream.Write("Pattern Loaded");
            } 
        }

        /// <summary>
        /// Make an archive (SEYRUP file) of all active files
        /// </summary>
        public void MakeArchive(bool complete = false)
        {
            Viewer.UpdateImage(Properties.Resources.SEYRworking);
            SaveProject();
            string[] filesFound = Directory.GetFiles(DirPath, string.Format("*.{0}", "seyrup"), SearchOption.AllDirectories);
            int idx = filesFound.Length;
            string zipPath = $@"{DirPath}\{idx}.seyrup";
            while (true)
            {
                if (File.Exists(zipPath))
                {
                    idx++;
                    zipPath = $@"{DirPath}\{filesFound.Length}.seyrup";
                }
                else
                    break;
            }
            
            DebugStream.Write($"Adding seyrup to {zipPath}");
            Viewer.InfoLabel.Text = "SEYRUP file created";
            // Create and open a new ZIP file
            using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(DataStream.Path, Path.GetFileName(DataStream.Path));
                zip.CreateEntryFromFile(DebugStream.Path, Path.GetFileName(DebugStream.Path));
                zip.CreateEntryFromFile(ProjectPath, Path.GetFileName(ProjectPath));
                if (Pattern != null) zip.CreateEntryFromFile(DirPath + @"\SEYRpattern.png", "SEYRpattern.png");
            }
            if (complete) Viewer.UpdateImage(Properties.Resources.SEYRdone);
        }

        /// <summary>
        /// Make SEYRUP, clear logs, and clear all feature scores
        /// </summary>
        public void ResetAll()
        {
            MakeArchive();
            ClearLogs();
            ClearAllFeatureScores();
        }

        #endregion

        #region Image Processing

        /// <summary>
        /// The core of SEYR
        /// </summary>
        /// <param name="bmp">
        /// Active image
        /// </param>
        /// <param name="forcePattern"></param>
        /// <param name="imageInfo">
        /// Info that matches dataHeader schema used for channel creation
        /// </param>
        /// <param name="customFilter"></param>
        /// <returns>
        /// Percentage of passing features in last image
        /// </returns>
        public async Task<string> NewImage(Bitmap bmp, bool forcePattern = false, string imageInfo = "", bool customFilter = false)
        {
            Working = true;
            double result = await BitmapFunctions.LoadImage(bmp, forcePattern, imageInfo, customFilter);
            Working = false;
            return customFilter ? result.ToString() : result.ToString("P");
        }

        public void OpenComposer(Bitmap bmp)
        {
            using (Composer w = new Composer((Bitmap)bmp.Clone()))
            {
                var result = w.ShowDialog();
                if (result == DialogResult.OK)
                    SaveProject();
                else if (result == DialogResult.Retry)
                    MakeArchive();
                else
                    LoadProject();
            }
        }

        private static void DiscardViewer()
        {
            foreach (Viewer v in Application.OpenForms.OfType<Viewer>())
                v.Close();
            Viewer = new Viewer();
        }

        #endregion
    }
}
