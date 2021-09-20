using System.Drawing;
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

        public void InsertNewImage(PictureBox pictureBox)
        {
            Bitmap bitmap = (Bitmap)pictureBox.BackgroundImage;
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(pictureBox.Image, 0, 0);
            }
            for (int i = 5; i > 0; i--)
            {
                PictureBoxes[i].Image = PictureBoxes[i - 1].Image;
            }
            PictureBoxes[0].Image = bitmap;
        }
    }
}
