using System.IO;

namespace SEYR.Session
{
    internal class DataStream
    {
        private readonly string Path = null;

        public DataStream(string path, bool isDebug = false)
        {
            if (File.Exists(path)) File.Delete(path);
            Path = path;
            if (isDebug) WriteDTLine("Stream Opened");
        }

        public void Write(string value)
        {
            File.AppendAllText(Path, value);
        }

        public void WriteLine(string value)
        {
            File.AppendAllText(Path, $"{value}\n");
        }

        public void WriteDTLine(string value)
        {
            File.AppendAllText(Path, $"{System.DateTime.Now}\t{value}\n");
        }
    }
}
