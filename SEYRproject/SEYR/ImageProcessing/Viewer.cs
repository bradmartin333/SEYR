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
            features.ForEach(x => x.ScoreHistory.Clear());
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
            if (DataShown)
            {
                if (features != null && features != Features)
                {
                    Features = features;
                    LoadPlotFeatures();
                }
                if (string.IsNullOrEmpty(ComboFeatureSelector.Text))
                    GenerateAndDisplayChartPanelImage("No Feature Selected");
                else if (features != null) // This is used as a flag that we have new data
                {
                    Feature[] matches = Features.Where(x => x.Name == ComboFeatureSelector.Text).ToArray();
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
            if (DataShown) // Display a default image
            {
                string message = string.Empty;
                if (string.IsNullOrEmpty(SelectedFeatureName))
                    message = "No Feature Selected";
                else if (ChartFeatureData.Series[0].Points.Count == 0)
                    message = "No Data Available";
                GenerateAndDisplayChartPanelImage(message);
            }
        }

        private void LoadPlotFeatures()
        {
            ComboFeatureSelector.Items.Clear();
            if (Features != null) ComboFeatureSelector.Items.AddRange(Features.Select(x => x.Name).ToArray());
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
            if (scores.Length == 0) // Check for other display states
            {
                if (nullCount == 0) GenerateAndDisplayChartPanelImage("No Data Available");
                else GenerateAndDisplayChartPanelImage("No Passing Features");
            }
            else
            {
                double scoreRange = scores.Max() - scores.Min();
                scoreRange *= 0.2;
                ChartFeatureData.ChartAreas[0].AxisX.Minimum = scores.Min() - scoreRange;
                ChartFeatureData.ChartAreas[0].AxisX.Maximum = scores.Max() + scoreRange;
                GenerateAndDisplayChartPanelImage();
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

        private void GenerateAndDisplayChartPanelImage(string message = null)
        {
            Bitmap bmp = new Bitmap(300, 50);
            if (!string.IsNullOrEmpty(message))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    g.DrawString(message, new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), 
                        new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
            }
            PanelChart.BackgroundImage = bmp;
        }
    }
}
