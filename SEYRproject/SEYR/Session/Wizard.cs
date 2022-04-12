using SEYR.ImageProcessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEYR.Session
{
    public partial class Wizard : Form, IProject
    {
        #region IProject Members

        public double Scaling
        {
            get => Channel.Project.Scaling;
            set
            {
                Channel.Project.Scaling = value;
                Channel.Project.ScaledPixelsPerMicron = Channel.Project.PixelsPerMicron * Channel.Project.Scaling;
                UpdateFilters();
                UpdateGrid();
            }
        }

        public int Threshold
        {
            get => Channel.Project.Threshold;
            set
            {
                Channel.Project.Threshold = value;
                UpdateFilters();
                UpdateGrid();
            }
        }

        public float Angle
        {
            get => Channel.Project.Angle;
            set
            {
                Channel.Project.Angle = value;
                UpdateFilters();
                UpdateGrid();
            }
        }
        public int OriginX
        {
            get => Channel.Project.OriginX;
            set
            {
                Channel.Project.OriginX = value;
                UpdateGrid();
            }
        }

        public int OriginY
        {
            get => Channel.Project.OriginY;
            set
            {
                Channel.Project.OriginY = value;
                UpdateGrid();
            }
        }

        public int PitchX
        {
            get => Channel.Project.PitchX;
            set
            {
                Channel.Project.PitchX = value;
                UpdateGrid();
            }
        }

        public int PitchY
        {
            get => Channel.Project.PitchY;
            set
            {
                Channel.Project.PitchY = value;
                UpdateGrid();
            }
        }


        public int SizeX
        {
            get => Channel.Project.SizeX;
            set
            {
                Channel.Project.SizeX = value;
                UpdateGrid();
            }
        }

        public int SizeY
        {
            get => Channel.Project.SizeY;
            set
            {
                Channel.Project.SizeY = value;
                UpdateGrid();
            }
        }

        public int Rows
        {
            get => Channel.Project.Rows;
            set
            {
                Channel.Project.Rows = value;
                UpdateGrid();
            }
        }

        public int Columns
        {
            get => Channel.Project.Columns;
            set
            {
                Channel.Project.Columns = value;
                UpdateGrid();
            }
        }

        public int Density
        {
            get => Channel.Project.Density;
            set
            {
                Channel.Project.Density = value;
            }
        }

        #endregion

        private readonly Bitmap InputImage;
        private bool FormReady = false;

        public Wizard(Bitmap bitmap)
        {
            InitializeComponent();
            InputImage = bitmap;
            PbxGrid.BackgroundImage = bitmap;
            NumScaling.Value = (decimal)Scaling;
            NumThreshold.Value = Threshold;
            NumAngle.Value = (decimal)Angle;
            NumOriginX.Value = OriginX;
            NumOriginY.Value = OriginY;
            NumPitchX.Value = PitchX;
            NumPitchY.Value = PitchY;
            NumSizeX.Value = SizeX;
            NumSizeY.Value = SizeY;
            NumColumns.Value = Columns;
            NumRows.Value = Rows;
            NumDensity.Value = Density;
            FormReady = true;
            UpdateFilters();
            UpdateGrid();
        }

        private void UpdateFilters()
        {
            if (!FormReady) return;
            FormReady = false;
            Bitmap bmp = (Bitmap)InputImage.Clone();
            BitmapFunctions.ApplyFilters(ref bmp);
            PbxFilters.BackgroundImage = bmp;
            FormReady = true;
        }

        private void UpdateGrid()
        {
            if (!FormReady) return;
            FormReady = false;
            Bitmap bmp = (Bitmap)InputImage.Clone();
            BitmapFunctions.DrawGrid(ref bmp);
            PbxGrid.BackgroundImage = bmp;
            FormReady = true;
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

        #region Num Handlers

        private void NumScaling_ValueChanged(object sender, EventArgs e)
        {
            Scaling = (float)NumScaling.Value;
        }

        private void NumThreshold_ValueChanged(object sender, EventArgs e)
        {
            Threshold = (int)NumThreshold.Value;
        }

        private void NumAngle_ValueChanged(object sender, EventArgs e)
        {
            Angle = (float)NumAngle.Value;
        }

        private void NumOriginX_ValueChanged(object sender, EventArgs e)
        {
            OriginX = (int)NumOriginX.Value;
        }

        private void NumOriginY_ValueChanged(object sender, EventArgs e)
        {
            OriginY = (int)NumOriginY.Value;
        }

        private void NumPitchX_ValueChanged(object sender, EventArgs e)
        {
            PitchX = (int)NumPitchX.Value;
        }

        private void NumPitchY_ValueChanged(object sender, EventArgs e)
        {
            PitchY = (int)NumPitchY.Value;
        }

        private void NumSizeX_ValueChanged(object sender, EventArgs e)
        {
            SizeX = (int)NumSizeX.Value;
        }

        private void NumSizeY_ValueChanged(object sender, EventArgs e)
        {
            SizeY = (int)NumSizeY.Value;
        }

        private void NumColumns_ValueChanged(object sender, EventArgs e)
        {
            Columns = (int)NumColumns.Value;
        }

        private void NumRows_ValueChanged(object sender, EventArgs e)
        {
            Rows = (int)NumRows.Value;
        }

        private void NumDensity_ValueChanged(object sender, EventArgs e)
        {
            Density = (int)NumDensity.Value;
        }

        #endregion
    }
}
