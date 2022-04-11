using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Drawing;
using System;

namespace SEYR.Session
{
    public class Channel
    {
        public List<Task> Tasks { get; set; } = new List<Task>();
        private readonly string ProjectPath = null;
        private static DataStream DataStream { get; set; } = null;
        private static DataStream DebugStream { get; set; } = null;
        private Project Project { get; set; } = null;
        private string LastData { get; set; } = string.Empty;

        /// <summary>
        /// Create a new SEYR Channe
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="streamPath"></param>
        /// <param name="pixelsPerMM"></param>
        /// <param name="debugPath">
        /// If null, logs to temp directory
        /// </param>
        public Channel(string projectPath, string streamPath, double pixelsPerMM, string debugPath = null)
        {
            DataStream = new DataStream(streamPath);
            DebugStream = new DataStream(string.IsNullOrEmpty(debugPath) ? $"{Path.GetTempPath()}SEYRdebug.txt" : debugPath, true);
            ProjectPath = projectPath;
            Project = new Project() { PixelsPerMM = pixelsPerMM };
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

        #region Opening and Closing

        public string GetProjectInfo()
        {
            return $"{Project.PixelsPerMM}";
        }

        public void SaveProject()
        {
            using (StreamWriter stream = new StreamWriter(ProjectPath))
            {
                XmlSerializer x = new XmlSerializer(typeof(Project));
                x.Serialize(stream, Project);
            }
            DebugStream.WriteDTLine("Project Saved");
        }

        public void LoadProject()
        {
            DebugStream.Write("Loading Project");
            using (StreamReader stream = new StreamReader(ProjectPath))
            {
                DebugStream.WriteLine($"\t{ProjectPath}");
                XmlSerializer x = new XmlSerializer(typeof(Project));
                Project = (Project)x.Deserialize(stream);
            }
            DebugStream.WriteDTLine("Project Loaded");
        }

        public void Close()
        {
            DataStream.Close();
            DebugStream.Close();
        }

        #endregion

        #region Image Processing

        public void NewImage(Bitmap bmp)
        {
            Tasks.Add(Task.Factory.StartNew(() => LastData = ImageProcessing.BitmapFunctions.LoadImage(bmp)));
        }

        public string GetLastData()
        {
            return LastData;
        }

        #endregion
    }
}
