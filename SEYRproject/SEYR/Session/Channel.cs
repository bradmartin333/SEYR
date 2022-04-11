using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEYR.Session
{
    public class Channel
    {
        private static NLog.Logger Logger;
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
            CreateLogger(streamPath);
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
            CreateLogger(streamPath);
            ProjectPath = projectPath;
            LoadProject();
        }

        public void SaveProject()
        {
            using (Stream stream = File.Open(ProjectPath, false ? FileMode.Append : FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Project.GetType());
                x.Serialize(stream, Project);
            }
            Logger.Info("Project Saved");
        }

        public void LoadProject()
        {
            using (Stream stream = File.Open(ProjectPath, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Project.GetType());
                Project = (Project)x.Deserialize(stream);
            }
            Logger.Info("Project Loaded");
        }

        private static void CreateLogger(string path)
        {
            Logger = NLog.LogManager.GetLogger(path);
            Logger.Info("Logger Created");
        }
    }
}
