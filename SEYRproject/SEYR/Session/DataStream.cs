using System.IO;
using System.Text;

namespace SEYR.Session
{
    internal class DataStream
    {
        private readonly FileStream Stream;

        public DataStream(string path)
        {
            if (File.Exists(path)) File.Delete(path);
            Stream = File.Create(path);
            WriteLine("Stream Created");
        }

        public void Write(string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            WriteToFile(info);
        }

        public void WriteLine(string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value + '\n');
            WriteToFile(info);
        }

        public void CloseStream()
        {
            Stream.Close();
        }

        private void WriteToFile(byte[] info)
        {
            Stream.Write(info, 0, info.Length);
            Stream.Flush();
        }
    }
}
