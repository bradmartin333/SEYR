using SEYR.Session;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEYR.ImageProcessing
{
    public partial class Composite : Form
    {
        private int Rows;
        private int Columns;
        private int[,] Data = null;
        private int[,] LastData = null;
        private Timer Timer = new Timer()
        {
            Interval = 1000,
            Enabled = true,
        };

        public Composite()
        {
            InitializeComponent();
            Timer.Start();
            Timer.Tick += Timer_Tick;
            Show();
        }

        private void SetupComposite(Size size)
        {
            Rows = size.Height + 1;
            Columns = size.Width + 1;
            Data = new int[Columns, Rows];
            LastData = new int[Columns, Rows];
            Pbx.BackgroundImage = new Bitmap(Columns, Rows);
        }

        public void AddHotspots(Point[] tile, Size size)
        {
            if (Application.OpenForms.OfType<Wizard>().Any()) return;
            if (Data == null) SetupComposite(size);
            foreach (Point point in tile)
                Data[point.X, point.Y]++;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Data != null) UpdatePlot();
        }

        public void UpdatePlot()
        {
            int min = Data.Cast<int>().Min();
            int max = Data.Cast<int>().Max();
            using (Graphics g = Graphics.FromImage(Pbx.BackgroundImage))
            {
                for (int i = 0; i < Columns; i++)
                {
                    for (int j = 0; j < Rows; j++)
                    {
                        if (Data[i, j] > LastData[i, j])
                        {
                            Color c = ColorFromHSV(Map(Data[i, j], min, max));
                            Rectangle r = new Rectangle(i, j, 1, 1);
                            g.FillRectangle(new SolidBrush(c), r);
                            LastData[i, j] = Data[i, j];
                        }
                    }
                }
            }
            Pbx.Refresh();
        }

        private Color ColorFromHSV(double hue, double value = 1, double saturation = 1)
        {
            if (hue == 360) return Color.FromArgb(255, Color.White);
            if (hue == 0) return Color.FromArgb(255, Color.Black);

            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        private double Map(double value, double fromLow, double fromHigh, double toLow = 0, double toHigh = 360)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
