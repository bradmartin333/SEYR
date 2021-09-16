using Accord.Imaging;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SEYR
{
    public partial class Alignment : Form
    {
        private Feature _Feature;
        private Point _Center;

        public Alignment()
        {
            InitializeComponent();

            numRow.Maximum = (decimal)FileHandler.Grid.NumberX;
            numCol.Maximum = (decimal)FileHandler.Grid.NumberY;

            ShowImage();
            Show();
        }

        private void ShowImage()
        {
            Tile tile = FileHandler.Grid.Tiles.Find(x => x.Index == new Point((int)numRow.Value, (int)numCol.Value));
            Feature displayFeature = tile.Features.Find(x => x.Equals(FileHandler.Grid.ActiveFeature));
            lblFeatureName.Text = displayFeature.Name;
            _Feature = FileHandler.Grid.Features.Find(x => x.Equals(FileHandler.Grid.ActiveFeature));

            Bitmap blob = Imaging.Crop(displayFeature, true);
            HorizontalIntensityStatistics his = new HorizontalIntensityStatistics(blob);
            VerticalIntensityStatistics vis = new VerticalIntensityStatistics(blob);
            _Center = new Point(his.Red.Median, vis.Red.Median);

            using (Graphics g = Graphics.FromImage(blob))
            {
                int p = (int)(Math.Min(blob.Width, blob.Height) * 0.1);
                g.FillEllipse(Brushes.LawnGreen, new Rectangle(_Center.X - p, _Center.Y - p, 2 * p, 2 * p));
            }

            pictureBox.BackgroundImage = blob;
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            _Feature.WeightedCenter = _Center;
            UpdateComposer();
            Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _Feature.WeightedCenter = Point.Empty;
            UpdateComposer();
            Close();
        }

        private void UpdateComposer()
        {
            if (Application.OpenForms.OfType<Composer>().Any())
                Application.OpenForms.OfType<Composer>().First().DisplayAlignmentStatus();
        }

        private void num_ValueChanged(object sender, EventArgs e)
        {
            ShowImage();
        }
    }
}
