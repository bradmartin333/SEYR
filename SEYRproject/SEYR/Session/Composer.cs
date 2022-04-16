using Accord.Imaging;
using SEYR.ImageProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        public List<Feature> Features { get => Channel.Project.Features; set => Channel.Project.Features = value; }

        public string PatternIntervalString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int PatternIntervalValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int PatternDeltaMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float PatternScore { get => Channel.Project.PatternScore; set => Channel.Project.PatternScore = value; }

        public List<Point> PatternLocations { get => Channel.Project.PatternLocations; set => Channel.Project.PatternLocations = value; }

        #endregion

        private int _TileRow = 1;
        private int TileRow
        {
            get => _TileRow;
            set
            {
                _TileRow = value;
                UpdateImages();
            }
        }
        private int _TileColumn = 1;
        private int TileColumn
        {
            get => _TileColumn;
            set
            {
                _TileColumn = value;
                UpdateImages();
            }
        }

        private Feature ActiveFeature = null;
        private readonly Bitmap InputImage;
        private bool FormReady = false;
        private bool ClickGrid = false;
        private bool LoadingFeature = false;

        public Composer(Bitmap bitmap)
        {
            InitializeComponent();
            InputImage = bitmap;
            InitializeHandlers();
            InitializeUI();
            FormReady = true;
            SetupFeatureUI(true);
            UpdateImages();
        }

        private void InitializeHandlers()
        {
            ComboFeatureNullDetection.Items.AddRange(Feature.GetDisplayNames());
            PbxGrid.MouseUp += PbxGrid_MouseUp;
            PbxTile.MouseUp += PbxTile_MouseUp;
            PbxTile.MouseDown += PbxTile_MouseDown;
        }

        private void InitializeUI()
        {
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
        }

        private void UpdateImages()
        {
            while (!FormReady) Application.DoEvents();
            Channel.DebugStream.Write($"Updating Grid");
            UpdateGrid();
            while (!FormReady) Application.DoEvents();
            Channel.DebugStream.Write($"Updating Tile");
            UpdateTile();
            while (!FormReady) Application.DoEvents();
        }

        private async void UpdateGrid()
        {
            if (!FormReady) return;
            FormReady = false;
            try
            {
                Bitmap bmp = (Bitmap)InputImage.Clone();
                PbxGrid.BackgroundImage = await BitmapFunctions.DrawGrid(bmp, TileRow, TileColumn);
            }
            catch (Exception ex)
            {
                Channel.DebugStream.Write($"Exception in UpdateGrid: {ex}");
            }
            FormReady = true;
        }

        private async void UpdateTile()
        {
            if (!FormReady) return;
            FormReady = false;
            try
            {
                Bitmap bmp = (Bitmap)InputImage.Clone();
                Bitmap tile = await BitmapFunctions.GenerateSingleTile(bmp, TileRow, TileColumn, ActiveFeature);
                PbxTile.BackgroundImage = tile;
                if (ActiveFeature != null) LabelCurrentFeatureScore.Text = ActiveFeature.GetLastScore().ToString();
            }
            catch (Exception ex)
            {
                Channel.DebugStream.Write($"Exception in UpdateTile: {ex}");
            }
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

        private async void PbxTile_MouseDown(object sender, MouseEventArgs e)
        {
            if (ActiveFeature == null || ClickGrid) return;
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetThreshold(ActiveFeature.Threshold);
            Bitmap bmp = (Bitmap)InputImage.Clone();
            Bitmap tile = await BitmapFunctions.GenerateSingleTile(bmp, TileRow, TileColumn, ActiveFeature, false);
            using (Graphics g = Graphics.FromImage(tile))
                g.DrawImage(tile, new Rectangle(Point.Empty, tile.Size), 0, 0, tile.Width, tile.Height, GraphicsUnit.Pixel, imageAttr);
            PbxTile.Image = tile;
        }

        private void PbxTile_MouseUp(object sender, MouseEventArgs e)
        {
            if (!ClickGrid)
            {
                PbxTile.Image = null;
                return;
            }
            LoadingFeature = true;
            Point point = ZoomMousePos(e.Location, PbxTile.Size, PbxTile.BackgroundImage.Size);
            if (ActiveFeature != null)
            {
                NumFeatureX.Value = point.X;
                NumFeatureY.Value = point.Y;
            }
            ClickGrid = false;
            ToolsToolStripMenuItem.Text = "Tools";
            LoadingFeature = false;
            UpdateRectangle();
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
            FormReady = false;
            ComboFeatures.Items.Clear();
            ComboFeatures.Items.AddRange(Features.Select(x => x.Name).ToArray());
            ComboFeatures.SelectedIndex = setNull ? -1 : Features.Count - 1;
            FormReady = true;
            if (setNull)
            {
                ActiveFeature = null;
                ComboFeatures.Text = "";
                tabControl.SelectedIndex = 0;
            }
            FormReady = true;
            UpdateTile();
            Channel.DebugStream.Write($"Load Feature UI");
        }

        private void AddFeature(Feature feature)
        {
            Features.Add(feature);
            SetupFeatureUI(false);
        }

        private void ApplyFeature()
        {
            if (LoadingFeature || !FormReady) return;
            Channel.DebugStream.Write($"Apply {ActiveFeature.Name}");
            UpdateTile();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            Channel.DebugStream.Write($"User Apply");
            SetupFeatureUI(true);
        }

        private void ComboFeatures_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadingFeature = true;
            ActiveFeature = Features[ComboFeatures.SelectedIndex];
            Channel.DebugStream.Write($"Editing {ActiveFeature.Name}");
            NumFeatureX.Value = ActiveFeature.Rectangle.X;
            NumFeatureY.Value = ActiveFeature.Rectangle.Y;
            NumFeatureWidth.Value = ActiveFeature.Rectangle.Width;
            NumFeatureHeight.Value = ActiveFeature.Rectangle.Height;
            TxtFeatureName.Text = ActiveFeature.Name;
            NumFeatureThreshold.Value = (decimal)ActiveFeature.Threshold;
            ComboFeatureNullDetection.SelectedIndex = (int)ActiveFeature.NullDetection;
            LoadingFeature = false;
            ApplyFeature();
        }

        private void BtnAddFeature_Click(object sender, EventArgs e)
        {
            Feature feature = new Feature();
            AddFeature(feature);
            Channel.DebugStream.Write($"{feature.Name} Added");
        }

        private void BtnDeleteFeature_Click(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            Channel.DebugStream.Write($"{ActiveFeature.Name} Deleted");
            Features.RemoveAt(ComboFeatures.SelectedIndex);
            SetupFeatureUI(true);
            UpdateTile();
        }

        private void BtnCopyFeature_Click(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            Feature feature = ActiveFeature.Clone();
            Channel.DebugStream.Write($"{ActiveFeature.Name} Copied To {feature.Name}");
            AddFeature(feature);
        }

        private void FeatureRectangle_ValueChanged(object sender, EventArgs e)
        {
            UpdateRectangle();
        }

        private void UpdateRectangle()
        {
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.Rectangle = new Rectangle((int)NumFeatureX.Value, (int)NumFeatureY.Value, (int)NumFeatureWidth.Value, (int)NumFeatureHeight.Value);
            Channel.DebugStream.Write($"{ActiveFeature.Name} Rectangle Changed");
            ApplyFeature();
        }

        private void TxtFeatureName_TextChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null || LoadingFeature) return;
            if (Features.Where(x => x.Name != ActiveFeature.Name).Select(x => x.Name).Contains(TxtFeatureName.Text))
                TxtFeatureName.BackColor = Color.LightCoral;
            else
            {
                TxtFeatureName.BackColor = Color.White;
                Channel.DebugStream.Write($"{ActiveFeature.Name} Renamed To {TxtFeatureName.Text}");
                ActiveFeature.Name = TxtFeatureName.Text;
                int lastIndex = ComboFeatures.SelectedIndex;
                ComboFeatures.Items.Clear();
                ComboFeatures.Items.AddRange(Features.Select(x => x.Name).ToArray());
                ComboFeatures.SelectedIndex = lastIndex;
            }    
        }

        private void NumFeatureThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.Threshold = (float)NumFeatureThreshold.Value;
            Channel.DebugStream.Write($"{ActiveFeature.Name} Threshold Changed");
            ApplyFeature();
        }

        private void ComboFeatureNullDetection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.NullDetection = (Feature.NullDetectionTypes)ComboFeatureNullDetection.SelectedIndex;
            Channel.DebugStream.Write($"{ActiveFeature.Name} Null Detection Changed");
            ApplyFeature();
        }

        private void BtnResetScoreHistory_Click(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            ActiveFeature.ResetScore();
            ApplyFeature();
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

        private void NumTileColumn_ValueChanged(object sender, EventArgs e)
        {
            TileColumn = (int)NumTileColumn.Value;
        }

        private void NumTileRow_ValueChanged(object sender, EventArgs e)
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
                Channel.DebugStream.Write($"Deskew Success: Angle = {angle}");
            }
            catch (Exception ex)
            {
                Channel.DebugStream.Write($"Deskew Failed: {ex}");
            }
            ToolsToolStripMenuItem.Text = "Tools";
        }

        private void ClickGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolsToolStripMenuItem.Text = "Ready for click...";
            ClickGrid = true;
        }

        private void TrainLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolsToolStripMenuItem.Text = "Working...";
            Application.DoEvents();
            if (Channel.Pattern != null)
            {
                PatternLocations = new List<Point>();
                Bitmap sourceImage = (Bitmap)InputImage.Clone();
                BitmapFunctions.ResizeAndRotate(ref sourceImage);
                var tm = new ExhaustiveTemplateMatching(PatternScore);
                TemplateMatch[] matchings = tm.ProcessImage(sourceImage, Channel.Pattern);
                foreach (TemplateMatch m in matchings)
                {
                    Channel.DebugStream.Write($"Pattern match: {m.Similarity} {m.Rectangle}");
                    PatternLocations.Add(m.Rectangle.Center());
                    using (Graphics g = Graphics.FromImage(sourceImage))
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.HotPink)), m.Rectangle);
                    }
                }
                PbxGrid.Image = sourceImage;
            }
            ToolsToolStripMenuItem.Text = "Tools";
        }

        private void HideLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PbxGrid.Image = null;
        }

        #endregion
    }
}
