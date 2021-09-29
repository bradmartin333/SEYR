using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SEYRDesktop
{
    public partial class FormMain : Form
    {
        Image IMG = null;
        FrameDimension DIM = null;
        int FRAMECOUNT = 0;

        public FormMain()
        {
            InitializeComponent();
            SEYR.Pipeline.Initialize();
        }

        private void btnOpenComposer_Click(object sender, System.EventArgs e)
        {
            SEYR.Pipeline.Composer.Show();
            SEYR.Pipeline.Composer.BringToFront();
        }

        private void btnOpenGIF_Click(object sender, System.EventArgs e)
        {
            string path = OpenFile("Open a GIF", "GIF file (*.gif)|*.gif");
            if (path == null)
                return;

            IMG = Image.FromFile(path);
            DIM = new FrameDimension(IMG.FrameDimensionsList[0]);
            FRAMECOUNT = IMG.GetFrameCount(DIM);

            numFrame.Maximum = FRAMECOUNT;
            numFrame.Value = 1;
        }

        private void numFrame_ValueChanged(object sender, System.EventArgs e)
        {
            IMG.SelectActiveFrame(DIM, (int)numFrame.Value - 1);
            Image image = (Image)IMG.Clone();
            Bitmap bitmap = new Bitmap(image);
            pictureBox.BackgroundImage = image;
            SEYR.Pipeline.LoadNewImage(bitmap);
        }

        private string OpenFile(string title, string filter)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = title;
                openFileDialog.Filter = filter;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    return openFileDialog.FileName;
            }
            return null;
        }      
    }
}
