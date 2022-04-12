using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using SEYR.ImageProcessing;

namespace SEYR.Session
{
    public class Channel
    {
        internal static Project Project { get; set; } = null;
        internal static Training Training { get; set; } = new Training();
        internal static DataStream DataStream { get; set; } = null;
        internal static DataStream DebugStream { get; set; } = null;
        private List<Task> Tasks { get; set; } = new List<Task>();
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

        #region Opening and Closing

        private void SaveProject()
        {
            using (StreamWriter stream = new StreamWriter(ProjectPath))
            {
                XmlSerializer x = new XmlSerializer(typeof(Project));
                x.Serialize(stream, Project);
            }
            DebugStream.WriteDTLine("Project Saved");
        }

        private void LoadProject()
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

        #endregion

        #region Image Processing

        public void NewImage(Bitmap bmp)
        {
            Tasks.Add(Task.Factory.StartNew(() => BitmapFunctions.LoadImage(bmp)));
            Training.Show();
        }

        public void RunWizard(Bitmap bmp)
        {
            using (Wizard w = new Wizard((Bitmap)bmp.Clone()))
            {
                var result = w.ShowDialog();
                if (result == DialogResult.OK)
                    SaveProject();
                else
                    return;
            }
        }

        #endregion
    }
}
