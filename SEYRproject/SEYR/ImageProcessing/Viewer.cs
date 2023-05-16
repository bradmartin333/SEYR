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

        private void Viewer_Load(object sender, System.EventArgs e)
        {
            if (Properties.Settings.Default.Viewer_Valid)
            {
                if (Properties.Settings.Default.Viewer_Maximized)
                {
                    Location = Properties.Settings.Default.Viewer_Location;
                    WindowState = FormWindowState.Maximized;
                    Size = Properties.Settings.Default.Viewer_Size;
                }
                else if (Properties.Settings.Default.Viewer_Minimized)
                {
                    Location = Properties.Settings.Default.Viewer_Location;
                    WindowState = FormWindowState.Minimized;
                    Size = Properties.Settings.Default.Viewer_Size;
                }
                else
                {
                    Location = Properties.Settings.Default.Viewer_Location;
                    Size = Properties.Settings.Default.Viewer_Size;
                }
            }
            Properties.Settings.Default.Viewer_Valid = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.Viewer_Location = RestoreBounds.Location;
                Properties.Settings.Default.Viewer_Size = RestoreBounds.Size;
                Properties.Settings.Default.Viewer_Maximized = true;
                Properties.Settings.Default.Viewer_Minimized = false;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.Viewer_Location = Location;
                Properties.Settings.Default.Viewer_Size = Size;
                Properties.Settings.Default.Viewer_Maximized = false;
                Properties.Settings.Default.Viewer_Minimized = false;
            }
            else
            {
                Properties.Settings.Default.Viewer_Location = RestoreBounds.Location;
                Properties.Settings.Default.Viewer_Size = RestoreBounds.Size;
                Properties.Settings.Default.Viewer_Maximized = false;
                Properties.Settings.Default.Viewer_Minimized = true;
            }
            Properties.Settings.Default.Save();

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void CheckForImageFeature()
        {
            if (!Session.Channel.Project.HasImageFeature())
            {
                PBX.BackgroundImage = Properties.Resources.NoImage;
                InfoLabel.Text = "No features are saving images. Continue if desired.";
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
