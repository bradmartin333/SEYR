﻿using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using SEYR.ImageProcessing;
using System;

namespace SEYR.Session
{
    public class Channel
    {
        internal static Project Project { get; set; } = null;
        internal static DataStream DataStream { get; set; } = null;
        internal static DataStream DebugStream { get; set; } = null;
        internal static Composite Composite { get; set; } = new Composite();
        private readonly string ProjectPath = null;

        /// <summary>
        /// Create a new SEYR Channe
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="streamPath"></param>
        /// <param name="pixelsPerMicron"></param>
        /// <param name="debugPath">
        /// If null, logs to temp directory
        /// </param>
        public Channel(string projectPath, string streamPath, double pixelsPerMicron, string debugPath = null)
        {
            DataStream = new DataStream(streamPath);
            DebugStream = new DataStream(string.IsNullOrEmpty(debugPath) ? $"{Path.GetTempPath()}SEYRdebug.txt" : debugPath, true);
            ProjectPath = projectPath;
            Project = new Project() { PixelsPerMicron = pixelsPerMicron };
            SaveProject();
        }

        /// <summary>
        /// Open an existing SEYR Channel
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="streamPath"></param>
        /// <param name="debugPath">
        /// If null, logs to temp directory
        /// </param>
        public Channel(string projectPath, string streamPath, string debugPath = null)
        {
            DataStream = new DataStream(streamPath);
            DebugStream = new DataStream(string.IsNullOrEmpty(debugPath) ? $"{Path.GetTempPath()}SEYRdebug.txt" : debugPath, true);
            ProjectPath = projectPath;
            LoadProject();
        }

        private void ClearLogs()
        {
            if (!string.IsNullOrEmpty(DataStream.Path)) DataStream = new DataStream(DataStream.Path);
            if (!string.IsNullOrEmpty(DebugStream.Path)) DebugStream = new DataStream(DebugStream.Path, true);
            DeleteComposite();
        }

        #region Opening and Closing

        private void SaveProject()
        {
            using (StreamWriter stream = new StreamWriter(ProjectPath))
            {
                XmlSerializer x = new XmlSerializer(typeof(Project));
                x.Serialize(stream, Project);
            }
            DebugStream.Write("Project Saved", true, true);
        }

        private void LoadProject()
        {
            DebugStream.Write($"Loading Project\t{ProjectPath}", true, true);
            using (StreamReader stream = new StreamReader(ProjectPath))
            {
                XmlSerializer x = new XmlSerializer(typeof(Project));
                Project = (Project)x.Deserialize(stream);
            }
            DebugStream.Write("Project Loaded", true, true);
        }

        #endregion

        #region Image Processing

        public void NewImage(Bitmap bmp)
        {
            Task.Factory.StartNew(() => BitmapFunctions.LoadImage(bmp));
        }

        public void RunWizard(Bitmap bmp)
        {
            using (Wizard w = new Wizard((Bitmap)bmp.Clone()))
            {
                var result = w.ShowDialog();
                if (result == DialogResult.OK)
                    SaveProject();
                else if (result == DialogResult.Ignore)
                    ClearLogs();
                else if (result == DialogResult.Abort)
                    LoadProject();
                else
                    return;
            }
        }

        public void ShowComposite()
        {
            if (Composite != null) Composite.Show();
        }

        private void DeleteComposite()
        {
            if (Composite != null) Composite.Close();
            Composite = new Composite();
        }

        #endregion
    }
}
