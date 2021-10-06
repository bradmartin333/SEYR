using System.Collections.Generic;
using System.Text;

namespace SEYR
{
    public static class DataHandler
    {
        public static List<string> Output = new List<string>();

        public static StringBuilder StringBuilder = new StringBuilder();

        public static void Append(string s)
        {
            StringBuilder.Append(s);
        }

        public static void NextImage()
        {
            Output[Pipeline.ImageIdx - 1] += StringBuilder.ToString();
            StringBuilder.Clear();
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
