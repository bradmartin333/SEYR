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
                UpdateImages();
            }
        }

        public float Angle
        {
            get => Channel.Project.Angle;
            set
            {
                Channel.Project.Angle = value;
                UpdateImages();
            }
        }

        public int OriginX
        {
            get => Channel.Project.OriginX;
            set
            {
                Channel.Project.OriginX = value;
                UpdateImages();
            }
        }

        public int OriginY
        {
            get => Channel.Project.OriginY;
            set
            {
                Channel.Project.OriginY = value;
                UpdateImages();
            }
        }

        public int PitchX
        {
            get => Channel.Project.PitchX;
            set
            {
                Channel.Project.PitchX = value;
                UpdateImages();
            }
        }

        public int PitchY
        {
            get => Channel.Project.PitchY;
            set
            {
                Channel.Project.PitchY = value;
                UpdateImages();
            }
        }

        public int SizeX
        {
            get => Channel.Project.SizeX;
            set
            {
                Channel.Project.SizeX = value;
                UpdateImages();
            }
        }

        public int SizeY
        {
            get => Channel.Project.SizeY;
            set
            {
                Channel.Project.SizeY = value;
                UpdateImages();
            }
        }

        public int Rows
        {
            get => Channel.Project.Rows;
            set
            {
                Channel.Project.Rows = value;
                UpdateImages();
            }
        }

        public int Columns
        {
            get => Channel.Project.Columns;
            set
            {
                Channel.Project.Columns = value;
                UpdateImages();
            }
        }

        public List<Feature> Features { 
            get => Channel.Project.Features;
            set
            {
                Channel.Project.Features = value;
                UpdateImages();
            }
        }

        public string PatternIntervalString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int PatternIntervalValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int PatternDeltaMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float PatternScore { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<Point> PatternLocations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
        private bool ClickGrid = false;
        private float ForceThreshold = -1f;
        private bool ShowThreshold = false;
        private bool LoadingFeature = true;

        public Composer(Bitmap bitmap)
        {
            InitializeComponent();
            InputImage = bitmap;
            InitializeHandlers();
            InitializeUI();
            SetupFeatureUI(true);
            UpdateImages();
        }

        private void InitializeHandlers()
        {
            ComboFeatureNullDetection.Items.AddRange(Feature.GetDisplayNames());
            PbxGrid.MouseUp += PbxGrid_MouseUp;
            PbxTile.MouseUp += PbxTile_MouseUp;
            OLV.ButtonClick += OLV_ButtonClick;
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
            if (LoadingFeature) return;
            UpdateGrid();
            UpdateTile();
        }

        private async void UpdateGrid()
        {
            try
            {
                Bitmap bmp = (Bitmap)InputImage.Clone();
                BitmapFunctions.ResizeAndRotate(ref bmp);
                if (ShowThreshold)
                {
                    ImageAttributes imageAttr = new ImageAttributes();
                    imageAttr.SetThreshold(ForceThreshold > -1 ? ForceThreshold : ActiveFeature.Threshold);
                    using (Graphics g = Graphics.FromImage(bmp))
                        g.DrawImage(bmp, new Rectangle(Point.Empty, bmp.Size), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttr);
                }
                PbxGrid.BackgroundImage = bmp;
                PbxGrid.Image = await BitmapFunctions.DrawGrid(bmp, TileRow, TileColumn);
            }
            catch (Exception ex)
            {
                Channel.DebugStream.Write($"Exception in UpdateGrid: {ex}");
            }
        }

        private async void UpdateTile()
        {
            try
            {
                Bitmap bmp = (Bitmap)InputImage.Clone();
                Bitmap tile = await BitmapFunctions.GenerateSingleTile(bmp, TileRow, TileColumn, ActiveFeature);
                PbxTile.BackgroundImage = tile;
                if (ActiveFeature != null) LabelCurrentFeatureScore.Text = ActiveFeature.LastScore.ToString();
            }
            catch (Exception ex)
            {
                Channel.DebugStream.Write($"Exception in UpdateTile: {ex}");
            }
        }

        private void OLV_ButtonClick(object sender, BrightIdeasSoftware.CellClickEventArgs e)
        {
            Feature feature = (Feature)e.Model;
            if (ActiveFeature == null) ActiveFeature = feature;
            OLV.SelectedObject = feature;
            Channel.DebugStream.Write($"Threshold button clicked for {feature.Name} and has value {feature.Threshold}");
            ForceThreshold = feature.Threshold;
            ShowThreshold = true;
            ApplyFeature();
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
            if (!ClickGrid)
            {
                //PbxTile.Image = null;
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
            float imageAspect = imgSize.Width / (float)imgSize.Height;
            float controlAspect = pbxSize.Width / (float)pbxSize.Height;
            PointF pos = new PointF(click.X, click.Y);
            if (imageAspect > controlAspect)
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
            OLV.ClearObjects();
            OLV.AddObjects(Features);
            if (setNull)
            {
                ActiveFeature = null;
                tabControl.SelectedIndex = 0;
            }
            else
                OLV.SelectedObject = ActiveFeature;
            LoadingFeature = false;
            UpdateImages();
            Channel.DebugStream.Write($"Load Feature UI");
        }


        private void ApplyFeature()
        {
            if (LoadingFeature) return;
            Channel.DebugStream.Write($"Apply {ActiveFeature.Name}");
            UpdateImages();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            ShowThreshold = false;
            Channel.DebugStream.Write($"User Apply");
            SetupFeatureUI(false);
        }

        private void OLV_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadingFeature = true;
            Channel.DebugStream.Write("Selected Feature Changed   ", false);
            if (OLV.SelectedObject == null) return;
            Channel.DebugStream.Write("Selected Feature Valid   ", false);
            ActiveFeature = (Feature)OLV.SelectedObject;
            Channel.DebugStream.Write($"Editing {ActiveFeature.Name}   ", false);
            NumFeatureX.Value = ActiveFeature.Rectangle.X;
            NumFeatureY.Value = ActiveFeature.Rectangle.Y;
            NumFeatureWidth.Value = ActiveFeature.Rectangle.Width;
            NumFeatureHeight.Value = ActiveFeature.Rectangle.Height;
            Channel.DebugStream.Write("Loaded Rectangle   ", false);
            TxtFeatureName.Text = ActiveFeature.Name;
            Channel.DebugStream.Write("Loaded Name   ", false);
            ThresholdScrollBar.Value = (int)(ActiveFeature.Threshold * 100f);
            LabelThreshold.Text = $"Threshold: {ActiveFeature.Threshold}";
            Channel.DebugStream.Write("Loaded Threshold   ", false);
            ComboFeatureNullDetection.SelectedIndex = (int)ActiveFeature.NullDetection;
            Channel.DebugStream.Write("Loaded Null Detection   ", false);
            LoadingFeature = false;
            Channel.DebugStream.Write($"{ActiveFeature.Name} Loaded");
            UpdateImages();
        }

        private void BtnDeleteFeature_Click(object sender, EventArgs e)
        {
            if (ActiveFeature == null || OLV.SelectedObject == null) return;
            Channel.DebugStream.Write($"{ActiveFeature.Name} Deleted");
            Features.Remove((Feature)OLV.SelectedObject);
            SetupFeatureUI(true);
        }

        private void BtnCopyFeature_Click(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            Feature feature = ActiveFeature.Clone();
            Channel.DebugStream.Write($"{ActiveFeature.Name} Copied To {feature.Name}");
            AddFeature(feature);
        }

        private void BtnAddFeature_Click(object sender, EventArgs e)
        {
            Feature feature = new Feature();
            AddFeature(feature);
            Channel.DebugStream.Write($"{feature.Name} Added");
        }

        private void AddFeature(Feature feature)
        {
            Features.Add(feature);
            ActiveFeature = feature;
            SetupFeatureUI(false);
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
                TxtFeatureName.BackColor = Color.LightCoral; // Name already taken
            else
            {
                TxtFeatureName.BackColor = Color.White;
                ActiveFeature.Name = TxtFeatureName.Text;
                Channel.DebugStream.Write($"{ActiveFeature.Name} Renamed To {TxtFeatureName.Text}");
                ApplyFeature();
            }    
        }

        private void ThresholdScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.Threshold = ThresholdScrollBar.Value / 100f;
            LabelThreshold.Text = $"Thresold: {ActiveFeature.Threshold}";
            Channel.DebugStream.Write($"{ActiveFeature.Name} Threshold Changed");
            ForceThreshold = -1f;
            ShowThreshold = true;
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
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.ClearScore();
            Channel.DebugStream.Write($"{ActiveFeature.Name} Score History Cleared");
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
                Channel.DebugStream.Write($"Deskew Success: Angle = {Math.Round(angle, 3)}");
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

        private void OpenPatternWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PatternWizard w = new PatternWizard((Bitmap)InputImage.Clone()))
            {
                _ = w.ShowDialog();
            }
        }

        private void ClearLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Channel.ClearLogs();
        }

        private void ClearAllFeatureScoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Channel.ClearAllFeatureScores();
        }

        private void OpenDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Channel.DirPath);
        }

        private void ReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Channel.DataStream.Path);
        }

        private void DebugLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Channel.DebugStream.Path);
        }

        private void MakeSEYRUPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
            Close();
        }

        #endregion
    }
}
