using System.Drawing;
using System.Windows.Forms;

namespace SEYR.ImageProcessing
{
    public partial class Viewer : Form
    {
        public Viewer()
        {
            InitializeComponent();
            Rectangle screen = Screen.FromControl(this).Bounds;
            Location = new Point(screen.X - Width, 0);
            if (screen.X - Width > 0)
                Location = new Point(screen.X - Width, 0);
            else
                Location = new Point(0, 0);
            Show();
            BringToFront();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        public void UpdateImage(Bitmap bmp)
        {
            Pbx.BackgroundImage = bmp;
        }
    }
}
