using System.Drawing;
using System.Windows.Forms;

namespace SEYR.ImageProcessing
{
    public partial class Training : Form
    {
        public Training()
        {
            InitializeComponent();
        }

        public void NewImage(Bitmap bmp)
        {
            pictureBox1.BackgroundImage = bmp;
            Application.DoEvents();
        }
    }
}
