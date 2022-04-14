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

        private Feature _ActiveFeature = null;
        public Feature ActiveFeature
        {
            get => _ActiveFeature;
            set
            {
                _ActiveFeature = value;
            }
        }

        private readonly Bitmap InputImage;
        private bool FormReady = false;
        private bool ClickGrid = false;

        public Composer(Bitmap bitmap)
        {
            InitializeComponent();
            ComboFeatureNullDetection.Items.AddRange(Feature.GetDisplayNames());
            InputImage = bitmap;
            PbxGrid.BackgroundImage = bitmap;
            PbxGrid.MouseUp += PbxGrid_MouseUp;
            PbxTile.MouseUp += PbxTile_MouseUp;
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
            SetupFeatureUI(true);
            UpdateGrid();
            UpdateTile();
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

        private async void UpdateTile()
        {
            if (!FormReady) return;
            FormReady = false;
            Bitmap bmp = (Bitmap)InputImage.Clone();
            (Bitmap tile, float entropy) = await BitmapFunctions.GenerateSingleTile(bmp, TileRow, TileColumn);
            PbxTile.BackgroundImage = tile;
            FormReady = true;
        }

        private void ConfirmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #region PBX Handlers

        private void PbxTile_MouseUp(object sender, MouseEventArgs e)
        {
            if (!ClickGrid) return;
            Point point = ZoomMousePos(e.Location, PbxTile.Size, PbxTile.BackgroundImage.Size);
            if (ActiveFeature != null)
            {
                NumFeatureX.Value = point.X;
                NumFeatureY.Value = point.Y;
            }
            ClickGrid = false;
            ToolsToolStripMenuItem.Text = "Tools";
        }

        private void PbxGrid_MouseUp(object sender, MouseEventArgs e)
        {
            if (!ClickGrid) return;
            Point point = ZoomMousePos(e.Location, PbxGrid.Size, PbxGrid.BackgroundImage.Size);
            NumOriginX.Value = point.X;
            NumOriginY.Value = point.Y;
            ClickGrid = false;
            ToolsToolStripMenuItem.Text = "Tools";
        }

        /// <summary>
        /// Method for adjusting mouse pos to pictureBox set to Zoom
        /// </summary>
        /// <param name="click">
        /// Mouse coordinates
        /// </param>
        /// <param name="pbxSize"></param>
        /// <param name="imgSize"></param>
        /// <returns>
        /// Pixel coordinates
        /// </returns>
        private Point ZoomMousePos(Point click, Size pbxSize, Size imgSize)
        {
            float ImageAspect = imgSize.Width / (float)imgSize.Height;
            float controlAspect = pbxSize.Width / (float)pbxSize.Height;
            PointF pos = new PointF(click.X, click.Y);
            if (ImageAspect > controlAspect)
            {
                float ratioWidth = imgSize.Width / (float)pbxSize.Width;
                pos.X *= ratioWidth;
                float scale = pbxSize.Width / (float)imgSize.Width;
                float displayHeight = scale * imgSize.Height;
                float diffHeight = pbxSize.Height - displayHeight;
                diffHeight /= 2;
                pos.Y -= diffHeight;
                pos.Y /= scale;
            }
            else
            {
                float ratioHeight = imgSize.Height / (float)pbxSize.Height;
                pos.Y *= ratioHeight;
                float scale = pbxSize.Height / (float)imgSize.Height;
                float displayWidth = scale * imgSize.Width;
                float diffWidth = pbxSize.Width - displayWidth;
                diffWidth /= 2;
                pos.X -= diffWidth;
                pos.X /= scale;
            }
            return new Point((int)(pos.X / Channel.Project.ScaledPixelsPerMicron), 
                (int)(pos.Y / Channel.Project.ScaledPixelsPerMicron));
        }

        #endregion

        #region Feature Management

        private void SetupFeatureUI(bool setNull)
        {
            ComboFeatures.Items.Clear();
            ComboFeatures.Items.AddRange(Features.Select(x => x.Name).ToArray());
            ComboFeatures.SelectedIndex = setNull ? -1 : Features.Count - 1;
        }

        private void ComboFeatures_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveFeature = Features[ComboFeatures.SelectedIndex];
            NumFeatureX.Value = ActiveFeature.Rectangle.X;
            NumFeatureY.Value = ActiveFeature.Rectangle.Y;
            NumFeatureWidth.Value = ActiveFeature.Rectangle.Width;
            NumFeatureHeight.Value = ActiveFeature.Rectangle.Height;
            TxtFeatureName.Text = ActiveFeature.Name;
            NumFeatureThreshold.Value = (decimal)ActiveFeature.Threshold;
            ComboFeatureNullDetection.SelectedIndex = (int)ActiveFeature.NullDetection;
        }

        private void AddFeature(Feature feature)
        {
            Features.Add(feature);
            SetupFeatureUI(false);
        }

        private void BtnAddFeature_Click(object sender, EventArgs e)
        {
            Feature feature = new Feature();
            AddFeature(feature);
        }

        private void BtnDeleteFeature_Click(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            Features.RemoveAt(ComboFeatures.SelectedIndex);
            SetupFeatureUI(true);
            ComboFeatures.Text = "";
        }

        private void BtnCopyFeature_Click(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            Feature feature = ActiveFeature.Clone();
            AddFeature(feature);
        }

        private void NumFeatureX_ValueChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            Rectangle R = ActiveFeature.Rectangle;
            ActiveFeature.Rectangle = new Rectangle((int)NumFeatureX.Value, R.Y, R.Width, R.Height);
            Features[ComboFeatures.SelectedIndex] = ActiveFeature;
        }

        private void NumFeatureY_ValueChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            Rectangle R = ActiveFeature.Rectangle;
            ActiveFeature.Rectangle = new Rectangle(R.X, (int)NumFeatureY.Value, R.Width, R.Height);
            Features[ComboFeatures.SelectedIndex] = ActiveFeature;
        }

        private void NumFeatureWidth_ValueChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            Rectangle R = ActiveFeature.Rectangle;
            ActiveFeature.Rectangle = new Rectangle(R.X, R.Y, (int)NumFeatureWidth.Value, R.Height);
            Features[ComboFeatures.SelectedIndex] = ActiveFeature;
        }

        private void NumFeatureHeight_ValueChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            Rectangle R = ActiveFeature.Rectangle;
            ActiveFeature.Rectangle = new Rectangle(R.X, R.Y, R.Width, (int)NumFeatureHeight.Value);
            Features[ComboFeatures.SelectedIndex] = ActiveFeature;
        }

        private void BtnApplyFeatureName_Click(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            ActiveFeature.Name = TxtFeatureName.Text;
            Features[ComboFeatures.SelectedIndex] = ActiveFeature;
            SetupFeatureUI(false);
        }

        private void NumFeatureThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            ActiveFeature.Threshold = (float)NumFeatureThreshold.Value;
            Features[ComboFeatures.SelectedIndex] = ActiveFeature;
        }

        private void ComboFeatureNullDetection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            ActiveFeature.NullDetection = (Feature.NullDetectionTypes)ComboFeatureNullDetection.SelectedIndex;
            Features[ComboFeatures.SelectedIndex] = ActiveFeature;
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

        #region Tools

        private void ApplyDeskewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolsToolStripMenuItem.Text = "Working...";
            Application.DoEvents();
            try
            {
                Deskew deskew = new Deskew((Bitmap)InputImage.Clone());
                double angle = deskew.GetSkewAngle();
                NumAngle.Value = (decimal)angle;
                Channel.DebugStream.Write($"Deskew Success: Angle = {angle}", true);
            }
            catch (Exception ex)
            {
                Channel.DebugStream.Write($"Deskew Failed: {ex}", true);
            }
            ToolsToolStripMenuItem.Text = "Tools";
        }

        private void ClickGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolsToolStripMenuItem.Text = "Ready for click...";
            ClickGrid = true;
        }

        #endregion
    }
}
