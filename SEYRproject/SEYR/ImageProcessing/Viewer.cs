using System.Linq;
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
            CheckForImageFeature();
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

        private void CheckForImageFeature()
        {
            if (Session.Channel.Project.Features.Count > 0 && !Session.Channel.Project.Features.Where(x => x.SaveImage).Any())
            {
                PBX.BackgroundImage = Properties.Resources.NoImage;
                InfoLabel.Text = "No features are saving images.";
            }
        }

        public void UpdateImage(Bitmap bmp, Bitmap overlay = null, bool force = false)
        {
            PBX.BackgroundImage = bmp;
            PBX.Image = overlay;
            if (force) Application.DoEvents();
        }
    }
}
