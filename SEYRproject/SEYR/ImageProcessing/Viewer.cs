using System;
using System.Drawing;
using System.Windows.Forms;

namespace SEYR.ImageProcessing
{
    public partial class Viewer : Form
    {
        public Viewer()
        {
            InitializeComponent();
            Location = Point.Empty;
            Show();
        }

        public void UpdateImage(Bitmap bmp)
        {
            Pbx.BackgroundImage = bmp;
        }
    }
}
