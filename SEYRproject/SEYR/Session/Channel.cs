using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;
using SEYR.ImageProcessing;
using System.Threading.Tasks;
using System;
using System.Drawing.Imaging;
using System.IO.Compression;

namespace SEYR.Session
{
    public class Channel
    {
        /// <summary>
        /// Tab separated data to append to beggining of each data line:
        /// Img# X Y RR RC R C SR SC
        /// </summary>
        public static string OutputData { get; set; }
        internal static Project Project { get; set; } = null;
        internal static DataStream DataStream { get; set; } = null;
        internal static DataStream DebugStream { get; set; } = null;
        internal static Viewer Viewer { get; set; }
        internal static Bitmap Pattern { get; set; } = null;
        private readonly string ProjectPath = null;
        private readonly string DirPath = null;

        /// <summary>
        /// Create a new SEYR Channel
        /// </summary>
        /// <param name="projectDir"></param>
        /// <param name="pixelsPerMicron"></param>
        /// <param name="dataHeader">
        /// Header for OutputData Lines
        /// </param>
        public Channel(string projectDir, float pixelsPerMicron, string dataHeader = "ImageNumber\tX\tY\tRR\tRC\tR\tC\tSR\tSC\t")
        {
            DirPath = projectDir;
            DataStream = new DataStream(DirPath + @"\SEYRreport.txt", header: dataHeader);
            DebugStream = new DataStream(DirPath + @"\SEYRdebug.txt", true);
            ProjectPath = DirPath + @"\project.seyr";
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
            DataStream = new DataStream(DirPath + @"\SEYRreport.txt", header: dataHeader);
            DebugStream = new DataStream(DirPath + @"\SEYRdebug.txt", true);
            ProjectPath = DirPath + @"\project.seyr";
            LoadProject();
            Viewer = new Viewer();
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
            LoadPattern();
        }

        private void LoadPattern()
        {
            FileInfo info = new FileInfo(ProjectPath);
            string patternPath = $@"{info.DirectoryName}\SEYRpattern.png";
            if (File.Exists(patternPath))
            {
                Bitmap bmp = new Bitmap(patternPath);
                Pattern = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format24bppRgb);
                using (Graphics g = Graphics.FromImage(Pattern))
                    g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                DebugStream.Write("Pattern Loaded");
            } 
        }

        /// <summary>
        /// Make an archive of all active files
        /// </summary>
        public void MakeArchive()
        {
            string zipPath = $@"{DirPath}\{DateTime.Now.ToString("s").Replace(':', '_')}.seyrup";
            DebugStream.Write($"Adding seyrup to {zipPath}");
            // Create and open a new ZIP file
            using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(DataStream.Path, Path.GetFileName(DataStream.Path));
                zip.CreateEntryFromFile(DebugStream.Path, Path.GetFileName(DebugStream.Path));
                zip.CreateEntryFromFile(ProjectPath, Path.GetFileName(ProjectPath));
                if (Pattern != null) zip.CreateEntryFromFile(DirPath + @"\SEYRpattern.png", "SEYRpattern.png");
            }
        }

        #endregion

        #region Image Processing

        public async Task<string> NewImage(Bitmap bmp, bool forcePattern = false)
        {
            await Task.Run(() => BitmapFunctions.LoadImage(bmp, forcePattern));
            return CreateStatusString();
        }


        private string CreateStatusString()
        {
            string output = string.Empty;
            foreach (Feature feature in Project.Features)
                output += $"{feature.Name}\t{feature.QuickGlance()}\n";
            return output;
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
