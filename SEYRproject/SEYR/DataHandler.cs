namespace SEYR
{
    public static class DataHandler
    {
        public static string OutputString { get; set; }

        public enum State
        {
            Pass,
            Fail,
            Null,
            Misaligned
        }
    }
}
