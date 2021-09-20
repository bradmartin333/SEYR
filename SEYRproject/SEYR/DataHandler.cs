using System.Collections.Generic;

namespace SEYR
{
    public static class DataHandler
    {
        public static List<string> Output = new List<string>();

        public enum State
        {
            Pass,
            Fail,
            Null,
            Misaligned
        }
    }
}
