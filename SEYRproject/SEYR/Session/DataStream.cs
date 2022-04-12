using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SEYR.Session
{
    internal class DataStream
    {
        private List<Task> Tasks { get; set; } = new List<Task>();
        private static readonly object Locker = new object();
        private readonly string Path = null;

        public DataStream(string path, bool isDebug = false)
        {
            if (File.Exists(path)) File.Delete(path);
            Path = path;
            if (isDebug) 
                Write("Stream Opened", true, true);
            else
                Write("Header", true);
        }

        public void Write(string value, bool addNewLine = false, bool addDT = false)
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
