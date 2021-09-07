namespace SEYR
{
    public static class DataHandler
    {
        public static string OutputString { get; internal set; }

        public enum State
        {
            Pass,
            Fail,
            Null,
            Misaligned
        }
    }
}
