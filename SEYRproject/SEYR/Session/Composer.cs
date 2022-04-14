using SEYR.ImageProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SEYR.Session
{
    public partial class Composer : Form, IProject
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

        public List<Feature> Features {
            get => Channel.Project.Features;
            set
            {
                Channel.Project.Features = value;
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
                UpdateGrid();
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
                UpdateGrid();
                UpdateTile();
            }
        }

        private Feature ActiveFeature;

        public Composer(Bitmap bitmap)
        {
            InitializeComponent();
            InputImage = bitmap;
            PbxGrid.BackgroundImage = bitmap;
            NumScaling.Value = (decimal)Scaling;
            NumAngle.Value = (decimal)Angle;
            NumOriginX.Value = OriginX;
            NumOriginY.Value = OriginY;
            NumPitchX.Value = PitchX;
            NumPitchY.Value = PitchY;
            NumSizeX.Value = SizeX;
            NumSizeY.Value = SizeY;
            NumColumns.Value = Columns;
            NumRows.Value = Rows;
            FormReady = true;
            SetupFeatureUI();
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
            PbxFilters.BackgroundImage = bmp;
            FormReady = true;
        }

        private void UpdateGrid()
        {
            if (!FormReady) return;
            FormReady = false;
            Bitmap bmp = (Bitmap)InputImage.Clone();
            BitmapFunctions.DrawGrid(ref bmp, TileRow, TileColumn);
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

        #region Feature Management

        private void SetupFeatureUI()
        {
            ComboFeatures.Items.AddRange(Features.Select(x => x.Name).ToArray());
            ComboFeatureNullDetection.Items.AddRange(Feature.GetDisplayNames());
        }

        private void ComboFeatures_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveFeature = Features[ComboFeatures.SelectedIndex];
            NumFeatureX.Value = ActiveFeature.Rectangle.X;
            NumFeatureY.Value = ActiveFeature.Rectangle.Y;
            NumFeatureWidth.Value = ActiveFeature.Rectangle.Width;
            NumFeatureHeight.Value = ActiveFeature.Rectangle.Height;
            NumFeaturePass.Value = (decimal)ActiveFeature.PassScore;
            NumFeaturePassTolerance.Value = (decimal)ActiveFeature.PassTolerance;
            NumFeatureFail.Value = (decimal)ActiveFeature.FailScore;
            NumFeatureFailTolerance.Value = (decimal)ActiveFeature.FailTolerance;
            TxtFeatureName.Text = ActiveFeature.Name;
            NumFeatureThreshold.Value = (decimal)ActiveFeature.Threshold;
            ComboFeatureNullDetection.SelectedIndex = (int)ActiveFeature.NullDetection;
        }

        #endregion

        #region Num Handlers

        private void NumScaling_ValueChanged(object sender, EventArgs e)
        {
            Scaling = (float)NumScaling.Value;
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

        private void NumTileColumn_ValueChanged_1(object sender, EventArgs e)
        {
            TileColumn = (int)NumTileColumn.Value;
        }

        private void NumTileRow_ValueChanged_1(object sender, EventArgs e)
        {
            TileRow = (int)NumTileRow.Value;
        }

        #endregion
    }
}
