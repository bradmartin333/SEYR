using SEYR.ImageProcessing;
using SEYR.ProjectComponents;
using SEYR.Session;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SEYR.Wizard
{
    internal partial class FiltersWizard : Form, IFilters
    {
        #region IFilters Members

        public double Scaling
        {
            get => Channel.Project.Scaling;
            set
            {
                Channel.Project.Scaling = value;
                Channel.Project.ScaledPixelsPerMicron = Channel.Project.PixelsPerMicron * Channel.Project.Scaling;
                UpdateImage();
            }
        }

        public int Threshold
        {
            get => Channel.Project.Threshold;
            set
            {
                Channel.Project.Threshold = value;
                UpdateImage();
            }
        }

        public float Angle
        {
            get => Channel.Project.Angle;
            set
            {
                Channel.Project.Angle = value;
                UpdateImage();
            }
        }

        #endregion

        private readonly Bitmap InputImage;
        private bool ShowGrid = false;

        public FiltersWizard(Bitmap bmp)
        {
            InitializeComponent();
            InputImage = bmp;
            NumScaling.Value = (decimal)Scaling;
            NumThreshold.Value = Threshold;
            NumAngle.Value = (decimal)Angle;
            PictureBox.BackgroundImage = BitmapFunctions.ApplyFilters((Bitmap)InputImage.Clone());
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void UpdateImage()
        {
            PictureBox.BackgroundImage = BitmapFunctions.ApplyFilters((Bitmap)InputImage.Clone());
        }

        private void NumScaling_ValueChanged(object sender, EventArgs e)
        {
            Scaling = (double)NumScaling.Value;
        }

        private void NumThreshold_ValueChanged(object sender, EventArgs e)
        {
            Threshold = (int)NumThreshold.Value;
        }

        private void NumAngle_ValueChanged(object sender, EventArgs e)
        {
            Angle = (float)NumAngle.Value;
        }

        private void ButtonToggleGrid_Click(object sender, EventArgs e)
        {
            ShowGrid = !ShowGrid;
            PictureBox.Image = ShowGrid ? GenerateGrid() : null;
        }

        private Bitmap GenerateGrid()
        {
            Bitmap bmp = new Bitmap(InputImage.Width, InputImage.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < InputImage.Width; i += (int)(InputImage.Width * 0.1))
                    g.DrawLine(new Pen(Brushes.HotPink, (float)(InputImage.Height * 0.01)), new Point(i, 0), new Point(i, InputImage.Height));
                for (int i = 0; i < InputImage.Height; i += (int)(InputImage.Height * 0.1))
                    g.DrawLine(new Pen(Brushes.HotPink, (float)(InputImage.Width * 0.01)), new Point(0, i), new Point(InputImage.Width, i));
            }
            return bmp;
        }
    }
}
