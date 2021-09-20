using System;
using System.Windows.Forms;

namespace SEYRDesktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            SEYR.Pipeline.Initialize();
            Application.Run(SEYR.Pipeline.Composer);
        }
    }
}
