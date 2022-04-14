using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SEYR.Session
{
    internal class DataStream
    {
        public string Path { get; set; } = null;
        private List<Task> Tasks { get; set; } = new List<Task>();
        private static readonly object Locker = new object();

        public DataStream(string path, bool isDebug = false, string header = "")
        {
            if (File.Exists(path)) File.Delete(path);
            Path = path;
            if (isDebug)
                Write("Stream Opened", addDT: true);
            else
                Write($"{header}TileRow\tTileCol\tFeature\tScore");
        }

        public void Write(string value, bool addNewLine = true, bool addDT = false)
        {
            Tasks.Add(Task.Factory.StartNew(() => 
            {
                lock (Locker)
                {
                    using (StreamWriter file = new StreamWriter(Path, append: true))
                        file.Write($"{(addDT ? $"{System.DateTime.Now}\t" : "")}{value}{(addNewLine ? "\n" : "")}");
                }
            }));
        }
    }
}
