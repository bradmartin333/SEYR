using System.IO;
using System.Threading.Tasks;

namespace SEYR.Session
{
    internal class DataStream
    {
        public string Path { get; set; } = null;
        internal static string BaseHeader = null;
        internal static string Header = null;

        public DataStream(string path, string header = "", bool isDebug = false)
        {
            if (File.Exists(path)) File.Delete(path);
            Path = path;
            if (isDebug)
                Write("Stream Opened", addDT: true);
            else
            {
                BaseHeader = header;
                Header = $"{header}TileRow\tTileCol\tFeature\tScore\tImageData";
                Write(Header);
            } 
        }

        public void Write(string value, bool addNewLine = true, bool addDT = false)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(Path, append: true))
                    file.WriteAsync($"{(addDT ? $"{System.DateTime.Now}\t" : "")}{value}{(addNewLine ? "\n" : "")}");
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
