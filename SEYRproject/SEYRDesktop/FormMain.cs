using System;
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
        bool STOP = false;

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnOpenComposer_Click(object sender, System.EventArgs e)
        {
            SEYR.Pipeline.Appear();
        }

        private void btnOpenGIF_Click(object sender, System.EventArgs e)
        {
            string path = OpenFile("Open a GIF", "GIF file (*.gif)|*.gif");
            if (path == null)
                return;

            btnOpenGIF.BackColor = Color.LawnGreen;

            IMG = Image.FromFile(path);
            DIM = new FrameDimension(IMG.FrameDimensionsList[0]);
            FRAMECOUNT = IMG.GetFrameCount(DIM);

            numFrame.Maximum = FRAMECOUNT;
            numFrame.Value = 1;
            NextImage();
        }

        private void numFrame_ValueChanged(object sender, System.EventArgs e)
        {
            NextImage();
        }

        private void NextImage()
        {
            IMG.SelectActiveFrame(DIM, (int)numFrame.Value - 1);
            Image image = (Image)IMG.Clone();
            Bitmap bitmap = new Bitmap(image);
            SEYR.Pipeline.ImageIdx = (int)numFrame.Value;
            SEYR.Pipeline.R = (int)(((double)numFrame.Value + 1) / Math.Sqrt(FRAMECOUNT + 1));
            SEYR.Pipeline.C = (int)(((double)numFrame.Value + 1) % Math.Sqrt(FRAMECOUNT + 1));
            SEYR.Pipeline.LoadNewImage(bitmap);
            while (SEYR.Pipeline.Working)
                Application.DoEvents();
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

        private void btnRunAll_Click(object sender, System.EventArgs e)
        {
            for (int i = (int)numFrame.Value; i < FRAMECOUNT; i++)
            {
                if (STOP)
                {
                    STOP = false;
                    return;
                }
                numFrame.Value++;
                Application.DoEvents();
            }
        }

        private void numPatternFollowInterval_ValueChanged(object sender, System.EventArgs e)
        {
            SEYR.Pipeline.PatternFollowInterval = (int)numPatternFollowInterval.Value;
        }

        private void numPatternFollowDelay_ValueChanged(object sender, System.EventArgs e)
        {
            SEYR.Pipeline.PatternFollowDelay = (int)numPatternFollowDelay.Value;
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            SEYR.Pipeline.ClearOutput(reloadImage: true);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            STOP = true;
        }
    }
}
