using System;
using System.Drawing;
using System.Windows.Forms;

namespace SEYR
{
    public partial class PixelPitch : Form
    {
        public PixelPitch()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            lblSEYRWidth.Text = Picasso.ThisSize.Width.ToString();
            UpdateRatio();
            base.OnActivated(e);
        }

        private void txtFrameWidth_TextChanged(object sender, EventArgs e)
        {
            UpdateRatio();
        }

        private void UpdateRatio()
        {
            bool valid = double.TryParse(txtFrameWidth.Text, out double wid);
            if (valid)
            {
                lblRatio.Text = Math.Round(wid / Picasso.ThisSize.Width, 3).ToString();
                txtFrameWidth.BackColor = SystemColors.Window;
            }
            else
            {
                lblRatio.Text = "N\\A";
                txtFrameWidth.BackColor = Color.PaleGoldenrod;
            }
        }
    }
}
