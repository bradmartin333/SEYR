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
        /// Directory within project directory for saving images of interest
        /// </summary>
        public string ImagesDirectory { get; set; } = null;
        /// <summary>
        /// SEYR Project's current PxPerMicron value
        /// </summary>
        public float PxPerMicron { get => Project.PixelsPerMicron; }
        internal static Project Project { get; set; } = null;
        internal static LogStream DataStream { get; set; } = null;
        internal static LogStream DebugStream { get; set; } = null;
        internal static LogStream StampStream { get; set; } = null;
        internal static Viewer Viewer { get; set; } = null;
        internal static Bitmap Pattern { get; set; } = null;
        internal static string PatternPath { get; set; } = null;
        internal static string DirPath { get; set; } = null;
        internal static bool IsNewProject { get; set; } = false;
        internal static string ProjectPath { get; set; } = null;

        /// <summary>
        /// Create instance of a SEYR Channel
        /// </summary>
        /// <param name="projectDir"></param>
        /// <param name="dataHeader"></param>
        /// <param name="isNewProject"></param>
        public Channel(string projectDir, string dataHeader = "ImageNumber\tX\tY\tRR\tRC\tR\tC\tSR\tSC\t", bool isNewProject = false)
        {
            IsNewProject = isNewProject;
            DirPath = projectDir;
            DataStream = new LogStream(DirPath + @"\SEYRreport.txt", dataHeader);
            DebugStream = new LogStream(DirPath + @"\SEYRdebug.txt", isDebug: true);
            ProjectPath = DirPath + @"\project.seyr";
            PatternPath = DirPath + @"\SEYRpattern.png";
            Pattern = null;
            if (isNewProject)
            {
                Project = new Project();
                SaveProject();
            }
            else
                LoadProject();
            Viewer = new Viewer(Project.Features);
            InitializePhotosDir();
        }

        private void InitializePhotosDir()
        {
            string testPath = $@"{DirPath}\Images\";
            if (!Directory.Exists(testPath)) Directory.CreateDirectory(testPath);
            ImagesDirectory = testPath;
        }

        /// <summary>
        /// Override the default value of 1.000 and save the project
        /// </summary>
        /// <param name="value"></param>
        public void SetPixelsPerMicron(float value)
        {
            Project.PixelsPerMicron = value;
            SaveProject();
        }

        /// <summary>
        /// Override the default Red color for feature Chroma
        /// </summary>
        /// <param name="color"></param>
        public void SetDefaultChroma(Color color)
        {
            Feature.DefaultRedChroma = color.R / 255f;
            Feature.DefaultGreenChroma = color.G / 255f;
            Feature.DefaultBlueChroma = color.B / 255f;
        }

        /// <summary>
        /// Override the default Score History Size of 1000
        /// </summary>
        /// <param name="size"></param>
        public void SetFeatureScoreHistorySize(int size) => Feature.ScoreHistorySize = size;

        /// <summary>
        /// Clear the Debug and Report logs
        /// </summary>
        public static void ClearLogs()
        {
            if (!string.IsNullOrEmpty(DataStream.Path)) DataStream = new LogStream(DataStream.Path, LogStream.BaseHeader);
            if (!string.IsNullOrEmpty(DebugStream.Path)) DebugStream = new LogStream(DebugStream.Path, isDebug: true);
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
        }

        #region Opening and Closing

        /// <summary>
        /// Allow user to chose directory for SEYR operation
        /// </summary>
        /// <param name="dataHeader">
        /// Tab delimeted header for the imageInfo provided with each image
        /// </param>
        /// <returns>
        /// A SEYR channel or null depending on completion of the folder browser dialog
        /// </returns>
        public static Channel OpenSEYR(string dataHeader = "ImageNumber\tX\tY\tRR\tRC\tR\tC\tSR\tSC\t")
        {
            try
            {
                Channel channel = null;
                FolderBrowserDialog fbd = new FolderBrowserDialog
                {
                    Description = "Open a directory for SEYR operations",
                    ShowNewFolderButton = true,
                    SelectedPath = Properties.Settings.Default.Folder_Path,
                };
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath, "*.seyr");
                    if (files.Length > 0)
                        channel = new Channel(fbd.SelectedPath, dataHeader, false);
                    else
                        channel = new Channel(fbd.SelectedPath, dataHeader, true);
                    Properties.Settings.Default.Folder_Path = fbd.SelectedPath;
                    Properties.Settings.Default.Save();
                }
                return channel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening folder browser dialog -- try again\n\n{ex.Message}");
                return null;
            }
        }

        private static void SaveProject(bool preserveViewer = false)
        {
            using (StreamWriter stream = new StreamWriter(ProjectPath))
            {
                XmlSerializer x = new XmlSerializer(typeof(Project));
                x.Serialize(stream, Project);
            }
            DebugStream.Write("Project Saved", addDT: true);
            if (!preserveViewer) DiscardViewer();
        }

        public void SaveProjectTo(string path)
        {
            if (File.Exists(PatternPath)) File.Copy(PatternPath, path + @"\SEYRpattern.png");
            using (StreamWriter stream = new StreamWriter(path + @"\project.seyr"))
            {
                XmlSerializer x = new XmlSerializer(typeof(Project));
                x.Serialize(stream, Project);
            }
            Properties.Settings.Default.Folder_Path = path;
            Properties.Settings.Default.Save();
            DiscardViewer();
        }

        /// <summary>
        /// Used when importing project.seyr
        /// </summary>
        /// <param name="path"></param>
        public void SetAndLoadProject(string path)
        {
            IsNewProject = false;
            LoadProject(path);
            string testPattern = path.Replace("project.seyr", "SEYRpattern.png");
            if (File.Exists(testPattern)) File.Copy(testPattern, PatternPath, true);
            using (StreamWriter stream = new StreamWriter(ProjectPath))
            {
                XmlSerializer x = new XmlSerializer(typeof(Project));
                x.Serialize(stream, Project);
            }
            DiscardViewer();
        }

        private void LoadProject(string path = null)
        {
            string thisPath = string.IsNullOrEmpty(path) ? ProjectPath : path;
            DebugStream.Write($"Loading Project\t{thisPath}", addDT: true);
            using (StreamReader stream = new StreamReader(thisPath))
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

        public static void LoadPattern()
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
        /// <param name="signalComplete">
        /// Show big green eye when done archiving
        /// </param>
        public void MakeArchive(bool signalComplete = true)
        {
            DebugStream.Write($"Compressing files", addDT: true, showInViewer: true);
            Viewer.UpdateImage(Properties.Resources.SEYRworking, force: true);

            SaveProject(true);
            string[] filesFound = Directory.GetFiles(DirPath, string.Format("*.{0}", "seyrup"), SearchOption.AllDirectories);
            int idx = filesFound.Length;
            string projectName = DirPath.Split('\\').Last();
            string zipPath = $@"{DirPath}\{projectName}_{idx}.seyrup";
            while (true)
            {
                if (File.Exists(zipPath))
                {
                    idx++;
                    zipPath = $@"{DirPath}\{projectName}_{idx}.seyrup";
                }
                else
                    break;
            }
            
            DebugStream.Write($"Adding seyrup to {zipPath}", addDT: true);
            Viewer.InfoLabel.Text = "SEYRUP file created";
            // Create and open a new ZIP file
            using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(DataStream.Path, Path.GetFileName(DataStream.Path));
                zip.CreateEntryFromFile(DebugStream.Path, Path.GetFileName(DebugStream.Path));
                zip.CreateEntryFromFile(ProjectPath, Path.GetFileName(ProjectPath));
                if (Pattern != null) zip.CreateEntryFromFile(DirPath + @"\SEYRpattern.png", "SEYRpattern.png");
            }

            if (signalComplete) SignalComplete();
        }

        public void SignalComplete()
        {
            Viewer.UpdateImage(Properties.Resources.SEYRdone);
        }

        /// <summary>
        /// Make SEYRUP, clear logs, and clear all feature scores
        /// </summary>
        public void ResetAll()
        {
            MakeArchive();
            ClearLogs();
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
        /// <param name="stamp">
        /// Whether or not to perform stamp inspection routine
        /// </param>
        /// <returns>
        /// Percentage of null fail features in last image or a number of posts in last stamp image
        /// </returns>
        public async Task<double> NewImage(Bitmap bmp, bool forcePattern = false, string imageInfo = "", bool stamp = false)
        {
            Working = true;
            double result = await BitmapFunctions.LoadImage(bmp, forcePattern, imageInfo, stamp);
            Working = false;
            return result;
        }

        public void OpenComposer(Bitmap bmp)
        {
            while (true)
            {
                using (Composer w = new Composer((Bitmap)bmp.Clone(), this))
                {
                    var result = w.ShowDialog();
                    if (result == DialogResult.Yes)
                    {
                        SaveProject();
                        break;
                    }
                    else if (result == DialogResult.OK)
                    {
                        MakeArchive();
                        break;
                    }
                    else if (result == DialogResult.Retry)
                        LoadProject();
                    else
                    {
                        LoadProject();
                        break;
                    }
                }
            }
        }

        public void InputParameters(Bitmap bmp)
        {
            StampStream = new LogStream(DirPath + @"\StampInspection.txt", LogStream.BaseHeader, true, true);
            using (StampParameterEntry parameterEntry = new StampParameterEntry(bmp))
            {
                parameterEntry.ShowDialog();
            }
        }

        public void ShowViewer()
        {
            if (Viewer != null)
            {
                Viewer.Show();
                Viewer.BringToFront();
            }
        }

        public static void DiscardViewer()
        {
            foreach (Viewer v in Application.OpenForms.OfType<Viewer>())
                v.Close();
            Viewer = new Viewer(Project.Features);
        }

        #endregion
    }
}
