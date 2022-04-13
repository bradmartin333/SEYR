using SEYR.ImageProcessing;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SEYR.Session
{
    public partial class Wizard : Form, IProject
    {
        #region IProject Members

        public float Scaling
        {
            get => Channel.Project.Scaling;
            set
            {
                Channel.Project.Scaling = value;
                Channel.Project.ScaledPixelsPerMicron = Channel.Project.PixelsPerMicron * Channel.Project.Scaling;
                UpdateFilters();
                UpdateGrid();
                UpdateTile();
            }
        }

        public float Threshold
        {
            get => Channel.Project.Threshold;
            set
            {
                Channel.Project.Threshold = value;
                UpdateFilters();
                UpdateGrid();
                UpdateTile();
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
                UpdateTile();
            }
        }
        public int OriginX
        {
            get => Channel.Project.OriginX;
            set
            {
                Channel.Project.OriginX = value;
                UpdateGrid();
                UpdateTile();
            }
        }

        public int OriginY
        {
            get => Channel.Project.OriginY;
            set
            {
                Channel.Project.OriginY = value;
                UpdateGrid();
                UpdateTile();
            }
        }

        public int PitchX
        {
            get => Channel.Project.PitchX;
            set
            {
                Channel.Project.PitchX = value;
                UpdateGrid();
                UpdateTile();
            }
        }

        public int PitchY
        {
            get => Channel.Project.PitchY;
            set
            {
                Channel.Project.PitchY = value;
                UpdateGrid();
                UpdateTile();
            }
        }

        public int SizeX
        {
            get => Channel.Project.SizeX;
            set
            {
                Channel.Project.SizeX = value;
                UpdateGrid();
                UpdateTile();
            }
        }

        public int SizeY
        {
            get => Channel.Project.SizeY;
            set
            {
                Channel.Project.SizeY = value;
                UpdateGrid();
                UpdateTile();
            }
        }

        public int Rows
        {
            get => Channel.Project.Rows;
            set
            {
                Channel.Project.Rows = value;
                UpdateGrid();
                UpdateTile();
            }
        }

        public int Columns
        {
            get => Channel.Project.Columns;
            set
            {
                Channel.Project.Columns = value;
                UpdateGrid();
                UpdateTile();
            }
        }

        public int Density
        {
            get => Channel.Project.Density;
            set
            {
                Channel.Project.Density = value;
                UpdateTile();
            }
        }

        public float Contrast
        {
            get => Channel.Project.Contrast;
            set
            {
                Channel.Project.Contrast = value;
                UpdateTile();
            }
        }

        public float Score
        {
            get => Channel.Project.Score;
            set
            {
                Channel.Project.Score = value;
                UpdateTile();
            }
        }

        public float Tolerance
        {
            get => Channel.Project.Tolerance;
            set
            {
                Channel.Project.Tolerance = value;
                UpdateTile();
            }
        }

        #endregion

        private readonly Bitmap InputImage;
        private bool FormReady = false;

        private int _TileRow = 1;
        private int TileRow
        {
            get => _TileRow;
            set
            {
                _TileRow = value;
                UpdateTile();
            }
        }
        private int _TileColumn = 1;
        private int TileColumn
        {
            get => _TileColumn;
            set
            {
                _TileColumn = value;
                UpdateTile();
            }
        }

        public Wizard(Bitmap bitmap)
        {
            InitializeComponent();
            InputImage = bitmap;
            PbxGrid.BackgroundImage = bitmap;
            NumScaling.Value = (decimal)Scaling;
            NumThreshold.Value = (decimal)Threshold;
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
            NumTolerance.Value = (decimal)Tolerance;
            NumContrast.Value = (decimal)Contrast;
            NumScore.Value = (decimal)Score;
            FormReady = true;
            UpdateFilters();
            UpdateGrid();
            UpdateTile();
        }

        private void UpdateFilters()
        {
            if (!FormReady) return;
            FormReady = false;
            Bitmap bmp = (Bitmap)InputImage.Clone();
            BitmapFunctions.ResizeAndRotate(ref bmp);
            BitmapFunctions.ApplyManualThreshold(ref bmp);
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

        private void UpdateTile()
        {
            if (!FormReady) return;
            FormReady = false;
            Bitmap bmp = (Bitmap)InputImage.Clone();
            (Bitmap tile, float entropy) = BitmapFunctions.GenerateSingleTile(bmp, TileRow, TileColumn);
            PbxTile.BackgroundImage = tile;
            LabelScore.Text = $"Score = {entropy}";
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

        private void BtnResetLogs_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Ignore;
            Close();
        }

        private void BtnReloadProject_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        #region Num Handlers

        private void NumScaling_ValueChanged(object sender, EventArgs e)
        {
            Scaling = (float)NumScaling.Value;
        }

        private void NumThreshold_ValueChanged(object sender, EventArgs e)
        {
            Threshold = (float)NumThreshold.Value;
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

        private void NumTolerance_ValueChanged(object sender, EventArgs e)
        {
            Tolerance = (float)NumTolerance.Value;
        }

        private void NumTileColumn_ValueChanged(object sender, EventArgs e)
        {
            TileColumn = (int)NumTileColumn.Value;
        }

        private void NumTileRow_ValueChanged(object sender, EventArgs e)
        {
            TileRow = (int)NumTileRow.Value;
        }

        private void NumScore_ValueChanged(object sender, EventArgs e)
        {
            Score = (float)NumScore.Value;
        }

        private void NumContrast_ValueChanged(object sender, EventArgs e)
        {
            Contrast = (float)NumContrast.Value;
        }

        #endregion
    }
}
