using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SEYR
{
    public static class DataHandler
    {
        public static List<string> Output = new List<string>();

        public static StringBuilder StringBuilder = new StringBuilder();

        public static string StreamPath = null;

        public static string LastData = null;

        public static void Append(string s)
        {
            StringBuilder.Append(s);
        }

        public static void NextImage()
        {
            if (StreamPath != null)
            {
                Output[0] += StringBuilder.ToString();
                File.AppendAllText(StreamPath, StringBuilder.ToString());
                LastData = Output[0];
                Output = new List<string>();
            }
            else
            {
                Output[Pipeline.ImageIdx - 1] += StringBuilder.ToString();
                LastData = Output[Pipeline.ImageIdx - 1];
                StringBuilder.Clear();
            }
        }

        public enum State
        {
            Pass,
            Fail,
            Null,
            Misaligned
        }
    }
}
