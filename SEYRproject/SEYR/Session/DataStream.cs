using System.IO;

namespace SEYR.Session
{
    internal class DataStream
    {
        private readonly StreamWriter Stream = null;

        public DataStream(string path, bool isDebug = false)
        {
            Stream = new StreamWriter(path, false);
            if (isDebug) WriteDTLine("Stream Opened");
        }

        public void Write(string value)
        {
            Stream.Write(value);
        }

        public void WriteLine(string value)
        {
            Stream.WriteLine(value);
        }

        public void WriteDTLine(string value)
        {
            Stream.WriteLine($"{System.DateTime.Now}\t{value}");
        }

        public void Close()
        {
            Stream.Close();
            Stream.Dispose();
        }
    }
}
