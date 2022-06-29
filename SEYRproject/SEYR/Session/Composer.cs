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

        private Feature _ActiveFeature = null;
        private Feature ActiveFeature {
            get => _ActiveFeature;
            set
            {
                if (value == null) LoadNullFeature();
                _ActiveFeature = value;
            } 
        }
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
            if (!Channel.IsNewProject) PbxTile.Image = null;
            FormClosing += Composer_FormClosing;
        }

        private void Composer_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Composer_Valid)
            {
                if (Properties.Settings.Default.Composer_Maximized)
                {
                    Location = Properties.Settings.Default.Composer_Location;
                    WindowState = FormWindowState.Maximized;
                    Size = Properties.Settings.Default.Composer_Size;
                }
                else if (Properties.Settings.Default.Composer_Minimized)
                {
                    Location = Properties.Settings.Default.Composer_Location;
                    WindowState = FormWindowState.Minimized;
                    Size = Properties.Settings.Default.Composer_Size;
                }
                else
                {
                    Location = Properties.Settings.Default.Composer_Location;
                    Size = Properties.Settings.Default.Composer_Size;
                }
            }
            Properties.Settings.Default.Composer_Valid = true;
        }

        private void Composer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.Composer_Location = RestoreBounds.Location;
                Properties.Settings.Default.Composer_Size = RestoreBounds.Size;
                Properties.Settings.Default.Composer_Maximized = true;
                Properties.Settings.Default.Composer_Minimized = false;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.Composer_Location = Location;
                Properties.Settings.Default.Composer_Size = Size;
                Properties.Settings.Default.Composer_Maximized = false;
                Properties.Settings.Default.Composer_Minimized = false;
            }
            else
            {
                Properties.Settings.Default.Composer_Location = RestoreBounds.Location;
                Properties.Settings.Default.Composer_Size = RestoreBounds.Size;
                Properties.Settings.Default.Composer_Maximized = false;
                Properties.Settings.Default.Composer_Minimized = true;
            }
            Properties.Settings.Default.Save();
        }

        private void InitializeHandlers()
        {
            ComboFeatureNullDetection.Items.AddRange(Feature.GetDisplayNames());
            PbxGrid.MouseUp += PbxGrid_MouseUp;
            PbxTile.MouseUp += PbxTile_MouseUp;
            OLV.ButtonClick += OLV_ButtonClick;
            OLV.DoubleClick += OLV_DoubleClick;
            SaveImagePanel.MouseUp += SaveImagePanel_MouseUp;
            FlipScorePanel.MouseUp += FlipScorePanel_MouseUp;
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

        private void UpdateGrid()
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
                PbxGrid.Image = Channel.IsNewProject ? null : BitmapFunctions.DrawGrid(bmp, TileRow, TileColumn);
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

        private void BtnInfoThreshold_Click(object sender, EventArgs e)
        {
            if (ActiveFeature == null) return;
            if (ShowThreshold)
            {
                Channel.DebugStream.Write($"Threshold button clicked for {ActiveFeature.Name} to turn off preview");
                ForceThreshold = -1;
                ShowThreshold = false;
                ApplyFeature();
            }
            else
            {
                Channel.DebugStream.Write($"Threshold button clicked for {ActiveFeature.Name} and has value {ActiveFeature.Threshold}");
                ForceThreshold = ActiveFeature.Threshold;
                ShowThreshold = true;
                ApplyFeature();
            }
        }

        private void OLV_DoubleClick(object sender, EventArgs e)
        {
            TabControl.SelectedIndex = 1;
        }

        private void ConfirmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
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
            if (Channel.IsNewProject || ClickGrid)
            {
                Point point = ZoomMousePos(e.Location, PbxGrid.Size, PbxGrid.BackgroundImage.Size);
                NumOriginX.Value = point.X;
                NumOriginY.Value = point.Y;
                ClickGrid = false;
                if (Channel.IsNewProject)
                {
                    PbxTile.Image = null;
                    Channel.IsNewProject = false;
                }
                else
                    ToolsToolStripMenuItem.Text = "Tools";
                UpdateImages();
            }
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
                TabControl.SelectedIndex = 0;
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
            UpdateImages();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            Channel.DebugStream.Write($"User Apply");
            TabControl.SelectedIndex = 0;
            if (ActiveFeature == null) return;
            ShowThreshold = false;
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
            ThresholdTrackBar.Value = (int)(ActiveFeature.Threshold * 100f);
            NumThreshold.Value = (decimal)(ActiveFeature.Threshold * 100f);
            Channel.DebugStream.Write("Loaded Threshold   ", false);
            ComboFeatureNullDetection.SelectedIndex = (int)ActiveFeature.NullDetection;
            Channel.DebugStream.Write("Loaded Null Detection   ", false);
            NumNullFilterPercentage.Value = (decimal)ActiveFeature.NullFilterPercentage * 100M;
            Channel.DebugStream.Write("Loaded Null Filter %   ", false);
            SaveImagePanel.BackgroundImage = ActiveFeature.SaveImage ? Properties.Resources.toggleOn : Properties.Resources.toggleOff;
            Channel.DebugStream.Write("Loaded Save Image   ", false);
            FlipScorePanel.BackgroundImage = ActiveFeature.FlipScore ? Properties.Resources.toggleOn : Properties.Resources.toggleOff;
            Channel.DebugStream.Write("Loaded Flip Score   ", false);
            Channel.DebugStream.Write($"{ActiveFeature.Name} Loaded");
            LoadingFeature = false;
            UpdateImages();
        }

        private void LoadNullFeature()
        {
            LoadingFeature = true;
            NumFeatureX.Value = NumFeatureX.Minimum;
            NumFeatureY.Value = NumFeatureY.Minimum;
            NumFeatureWidth.Value = NumFeatureWidth.Minimum;
            NumFeatureHeight.Value = NumFeatureHeight.Minimum;
            TxtFeatureName.Text = "No Feature Selected";
            ThresholdTrackBar.Value = ThresholdTrackBar.Minimum;
            NumThreshold.Value = NumThreshold.Minimum;
            NumNullFilterPercentage.Value = 0.1M;
            ComboFeatureNullDetection.SelectedIndex = 0;
            FlipScorePanel.BackgroundImage = Properties.Resources.toggleOff;
            LabelCurrentFeatureScore.Text = "N/A";
            Channel.DebugStream.Write("Null feature loaded");
            LoadingFeature = false;
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

        private void NumFeatureX_ValueChanged(object sender, EventArgs e)
        {
            UpdateRectangle();
        }

        private void NumFeatureWidth_ValueChanged(object sender, EventArgs e)
        {
            UpdateRectangle();
        }

        private void NumFeatureY_ValueChanged(object sender, EventArgs e)
        {
            UpdateRectangle();
        }

        private void NumFeatureHeight_ValueChanged(object sender, EventArgs e)
        {
            UpdateRectangle();
        }

        private void UpdateRectangle()
        {
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.Rectangle = new Rectangle((int)NumFeatureX.Value, (int)NumFeatureY.Value, (int)NumFeatureWidth.Value, (int)NumFeatureHeight.Value);
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
                ApplyFeature();
            }    
        }

        private void ThresholdTrackBar_Scroll(object sender, EventArgs e)
        {
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.Threshold = ThresholdTrackBar.Value / 100f;
            NumThreshold.Value = (decimal)(ActiveFeature.Threshold * 100f);
            ForceThreshold = -1f;
            ShowThreshold = true;
            ApplyFeature();
        }

        private void NumThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.Threshold = (float)NumThreshold.Value / 100f;
            ThresholdTrackBar.Value = (int)(ActiveFeature.Threshold * 100f);
            ForceThreshold = -1f;
            ShowThreshold = true;
            ApplyFeature();
        }

        private void ComboFeatureNullDetection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.NullDetection = (Feature.NullDetectionTypes)ComboFeatureNullDetection.SelectedIndex;
            ApplyFeature();
        }

        private void NumNullFilterPercentage_ValueChanged(object sender, EventArgs e)
        {
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.NullFilterPercentage = (float)(NumNullFilterPercentage.Value / 100);
            ApplyFeature();
        }

        private void SaveImagePanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.SaveImage = !ActiveFeature.SaveImage;
            SaveImagePanel.BackgroundImage = ActiveFeature.SaveImage ? Properties.Resources.toggleOn : Properties.Resources.toggleOff;
            ApplyFeature();
        }

        private void FlipScorePanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (ActiveFeature == null || LoadingFeature) return;
            ActiveFeature.FlipScore = !ActiveFeature.FlipScore;
            FlipScorePanel.BackgroundImage = ActiveFeature.FlipScore ? Properties.Resources.toggleOn : Properties.Resources.toggleOff;
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

        private void NumSelectedColumn_ValueChanged(object sender, EventArgs e)
        {
            TileColumn = (int)NumSelectedColumn.Value;
        }

        private void NumSelectedRow_ValueChanged(object sender, EventArgs e)
        {
            TileRow = (int)NumSelectedRow.Value;
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
            while (true)
            {
                using (PatternWizard w = new PatternWizard((Bitmap)InputImage.Clone()))
                {
                    if (w.ShowDialog() != DialogResult.Retry) break;
                }
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
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ResetWindowLayoutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Composer_Valid = false;
            Properties.Settings.Default.Viewer_Valid = false;
        }

        #endregion
    }
}
