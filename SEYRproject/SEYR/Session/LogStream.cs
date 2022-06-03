using System.IO;
using System.Threading.Tasks;

namespace SEYR.Session
{
    internal class LogStream
    {
        public string Path { get; set; } = null;
        internal static string BaseHeader = null;
        internal static string Header = null;

        public LogStream(string path, string header = "", bool isDebug = false, bool isStamp = false)
        {
            if (File.Exists(path)) File.Delete(path);
            Path = path;
            if (!isDebug)
            {
                BaseHeader = header;
                Header = $"{header}TileRow\tTileCol\tFeature\tScore\tState\tImageData";
                Write(Header);
            } 
            else if (isStamp)
            {
                BaseHeader = header;
                Header = $"{header}NumPosts\tPxPostDebris\tPxMesaDebris";
                Write(Header);
            }
        }

        public void Write(string value, bool addNewLine = true, bool addDT = false, bool showInViewer = false)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(Path, append: true))
                    file.WriteAsync($"{(addDT ? $"{System.DateTime.Now}\t" : "")}{value}{(addNewLine ? "\n" : "")}");
                if (showInViewer) Channel.Viewer.InfoLabel.Text = $"{System.DateTime.Now}   {value.Replace("\t", "   ")}";
            }
            catch (System.Exception) { }
        }

        public async Task WriteAsync(string value, bool addNewLine = true, bool addDT = false, bool showInViewer = false)
        {
            using (StreamWriter file = new StreamWriter(Path, append: true))
                await file.WriteAsync($"{(addDT ? $"{System.DateTime.Now}\t" : "")}{value}{(addNewLine ? "\n" : "")}");
            if (showInViewer)
            {
                try { Channel.Viewer.Invoke((System.Windows.Forms.MethodInvoker)delegate { 
                    Channel.Viewer.InfoLabel.Text = $"{System.DateTime.Now}   {value.Replace("\t", "   ")}"; }); }
                catch (System.Exception) { }
            }
        }
    }
}
