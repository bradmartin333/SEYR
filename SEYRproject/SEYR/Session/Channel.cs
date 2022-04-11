using System.IO;

namespace SEYR.Session
{
    public class Channel
    {
        private static DataStream DataStream;
        private Project Project;
        private readonly string ProjectPath;

        /// <summary>
        /// Create a new SEYR Channel
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="streamPath"></param>
        /// <param name="pixelsPerMM"></param>
        public Channel(string projectPath, string streamPath, double pixelsPerMM)
        {
            DataStream = new DataStream(streamPath);
            ProjectPath = projectPath;
            Project = new Project(pixelsPerMM);
            SaveProject();
        }

        /// <summary>
        /// Open an existing SEYR Channel
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="streamPath"></param>
        public Channel(string projectPath, string streamPath)
        {
            DataStream = new DataStream(streamPath);
            ProjectPath = projectPath;
            LoadProject();
        }

        public string GetProjectInfo()
        {
            return $"{Project.PixelsPerMM}";
        }

        public void SaveProject()
        {
            using (Stream stream = File.Open(ProjectPath, false ? FileMode.Append : FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Project.GetType());
                x.Serialize(stream, Project);
            }
            DataStream.WriteLine("Project Saved");
        }

        public void LoadProject()
        {
            using (Stream stream = File.Open(ProjectPath, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Project.GetType());
                Project = (Project)x.Deserialize(stream);
            }
            DataStream.WriteLine("Project Loaded");
        }

        public void Close()
        {
            DataStream.CloseStream();
        }
    }
}
