﻿using System.Drawing;
using System.Windows.Forms;

namespace SEYR
{
    public partial class Viewer : Form
    {
        private PictureBox[] PictureBoxes;

        public Viewer()
        {
            InitializeComponent();
            PictureBoxes = new PictureBox[] { pbxMain, pbxA, pbxB, pbxC, pbxD, pbxE };
            foreach (PictureBox pictureBox in PictureBoxes)
            {
                pictureBox.Image = new Bitmap(1, 1);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        public void InsertNewImage(PictureBox pictureBox)
        {
            Bitmap background = (Bitmap)pictureBox.BackgroundImage.Clone();
            Bitmap foreground = (Bitmap)pictureBox.Image.Clone();
            using (Graphics g = Graphics.FromImage(background))
            {
                g.DrawImage(foreground, 0, 0);
            }
            for (int i = 5; i > 0; i--)
            {
                PictureBoxes[i].Image = PictureBoxes[i - 1].Image;
            }
            PictureBoxes[0].Image = background;
        }

        private void btnClearViewer_Click(object sender, System.EventArgs e)
        {
            foreach (PictureBox pictureBox in PictureBoxes)
            {
                pictureBox.Image = new Bitmap(1, 1);
            }
        }
    }
}
