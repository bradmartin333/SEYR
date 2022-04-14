using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;
using SEYR.ImageProcessing;
using System.Threading.Tasks;

namespace SEYR.Session
{
    public class Channel
    {
        internal static Project Project { get; set; } = null;
        internal static DataStream DataStream { get; set; } = null;
        internal static DataStream DebugStream { get; set; } = null;
        internal static Viewer Viewer { get; set; } = new Viewer();
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

        public void ClearLogs()
        {
            if (!string.IsNullOrEmpty(DataStream.Path)) DataStream = new DataStream(DataStream.Path);
            if (!string.IsNullOrEmpty(DebugStream.Path)) DebugStream = new DataStream(DebugStream.Path, true);
            foreach (Feature feature in Project.Features)
                feature.ResetScore();
            DiscardViewer();
        }

        #region Opening and Closing

        private void SaveProject()
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
                Project = (Project)x.Deserialize(stream);
            }
            DebugStream.Write("Project Loaded", addDT: true);
        }

        #endregion

        #region Image Processing

        public async Task<string> NewImage(Bitmap bmp)
        {
            await Task.Run(() => BitmapFunctions.LoadImage(bmp));
            return "hi";
        }

        public void OpenComposer(Bitmap bmp)
        {
            using (Composer w = new Composer((Bitmap)bmp.Clone()))
            {
                var result = w.ShowDialog();
                if (result == DialogResult.OK)
                    SaveProject();
                else
                    LoadProject();
            }
        }

        public void ShowViewer()
        {
            if (Viewer != null) Viewer.Show();
        }

        private void DiscardViewer()
        {
            if (Viewer != null) Viewer.Close();
            Viewer = new Viewer();
        }

        #endregion
    }
}
