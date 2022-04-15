using SEYR.Session;
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

            LocationChanged += Viewer_LocationChanged;
            ResizeEnd += Viewer_ResizeEnd;

            Location = Channel.Project.ViewerLocation;
            if (Channel.Project.ViewerSize != Size.Empty)
                Size = Channel.Project.ViewerSize;
            Show();
        }

        private void Viewer_LocationChanged(object sender, EventArgs e)
        {
            Channel.Project.ViewerLocation = Location;
        }

        private void Viewer_ResizeEnd(object sender, EventArgs e)
        {
            Channel.Project.ViewerSize = Size;
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
