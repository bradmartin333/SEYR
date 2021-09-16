using System;
using System.Windows.Forms;

namespace SEYR
{
    static class Interface
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Open()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Composer(new System.Drawing.Bitmap(200, 200)));
        }
    }
}
