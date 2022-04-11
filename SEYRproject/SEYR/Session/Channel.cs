using System.IO;
using System.Xml.Serialization;

namespace SEYR.Session
{
    public class Channel
    {
        private static DataStream DataStream = null;
        private static DataStream DebugStream = null;
        private Project Project = null;
        private readonly string ProjectPath = null;

        /// <summary>
        /// Create a new SEYR Channel
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
    }
}
