using SEYR.Session;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SEYR.ImageProcessing
{
    public partial class Viewer : Form
    {
        private static bool DataShown = false;
        private static List<Feature> Features = new List<Feature>();
        private static string SelectedFeatureName = "";

        public Viewer(List<Feature> features)
        {
            InitializeComponent();
            Features = features;
            LoadPlotFeatures();
            LoadPlot();
            Rectangle screen = Screen.FromControl(this).Bounds;
            Location = new Point(screen.X - Width, 0);
            if (screen.X - Width > 0)
                Location = new Point(screen.X - Width, 0);
            else
                Location = new Point(0, 0);
            CheckForImageFeature();
            Show();
            BringToFront();
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Viewer_Valid)
            {
                if (Properties.Settings.Default.Viewer_Maximized)
                {
                    Location = Properties.Settings.Default.Viewer_Location;
                    WindowState = FormWindowState.Maximized;
                    Size = Properties.Settings.Default.Viewer_Size;
                }
                else if (Properties.Settings.Default.Viewer_Minimized)
                {
                    Location = Properties.Settings.Default.Viewer_Location;
                    WindowState = FormWindowState.Minimized;
                    Size = Properties.Settings.Default.Viewer_Size;
                }
                else
                {
                    Location = Properties.Settings.Default.Viewer_Location;
                    Size = Properties.Settings.Default.Viewer_Size;
                }
            }
            Properties.Settings.Default.Viewer_Valid = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.Viewer_Location = RestoreBounds.Location;
                Properties.Settings.Default.Viewer_Size = RestoreBounds.Size;
                Properties.Settings.Default.Viewer_Maximized = true;
                Properties.Settings.Default.Viewer_Minimized = false;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.Viewer_Location = Location;
                Properties.Settings.Default.Viewer_Size = Size;
                Properties.Settings.Default.Viewer_Maximized = false;
                Properties.Settings.Default.Viewer_Minimized = false;
            }
            else
            {
                Properties.Settings.Default.Viewer_Location = RestoreBounds.Location;
                Properties.Settings.Default.Viewer_Size = RestoreBounds.Size;
                Properties.Settings.Default.Viewer_Maximized = false;
                Properties.Settings.Default.Viewer_Minimized = true;
            }
            Properties.Settings.Default.Save();

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void CheckForImageFeature()
        {
            if (!Channel.Project.HasImageFeature())
            {
                PBX.BackgroundImage = Properties.Resources.NoImage;
                InfoLabel.Text = "No features are saving images. Continue if desired.";
            }
        }

        public void UpdateImage(Bitmap bmp, Bitmap overlay = null, bool force = false, List<Feature> features = null)
        {
            PBX.BackgroundImage = bmp;
            PBX.Image = overlay;
            if (features != null && DataShown)
            {
                if (features != Features)
                {
                    Features = features;
                    LoadPlotFeatures();
                }
                if (!string.IsNullOrEmpty(ComboFeatureSelector.Text))
                {
                    Feature[] matches = features.Where(x => x.Name == ComboFeatureSelector.Text).ToArray();
                    if (matches.Any()) PlotData(matches.First());
                }
            }
            if (force) Application.DoEvents();
        }

        private void LoadPlot()
        {
            BtnShowData.BackgroundImage = DataShown ? Properties.Resources.caretDown : Properties.Resources.caretUp;
            TLP.RowStyles[2].Height = DataShown ? 225 : 0;
            ComboFeatureSelector.Text = SelectedFeatureName;
        }

        private void LoadPlotFeatures()
        {
            ComboFeatureSelector.Items.Clear();
            ComboFeatureSelector.Items.AddRange(Features.Select(x => x.Name).ToArray());
            ChartFeatureData.Series[0].Points.Clear();
        }

        private void PlotData(Feature feature)
        {
            int nullCount = feature.ScoreHistory.Where(x => x < 0).Count();
            LblFailingNullCount.Text = nullCount.ToString();
            ChartFeatureData.Titles[0].Text = $"Count in Last {feature.ScoreHistory.Count} Points";
            double[] scores = feature.ScoreHistory.Where(x => x >= 0).Select(x => Math.Round(x)).Distinct().ToArray();
            int[] counts = new int[scores.Length];
            for (int i = 0; i < scores.Length; i++)
                counts[i] = feature.ScoreHistory.Count(x => Math.Round(x) == scores[i]);
            ChartFeatureData.Series[0].Points.Clear();
            for (int i = 0; i < scores.Length; i++)
            {
                DataPoint dataPoint = new DataPoint(scores[i], counts[i]);
                ChartFeatureData.Series[0].Points.Add(dataPoint);
            }
        }

        private void BtnShowData_Click(object sender, EventArgs e)
        {
            DataShown = !DataShown;
            LoadPlot();
        }

        private void ComboFeatureSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboFeatureSelector.Text != SelectedFeatureName)
            {
                SelectedFeatureName = ComboFeatureSelector.Text;
                PlotData(Features.Where(x => x.Name == SelectedFeatureName).First());
            }
        }
    }
}
