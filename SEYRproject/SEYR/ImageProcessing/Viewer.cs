using System;
using System.Drawing;
using System.Windows.Forms;

namespace SEYR.ImageProcessing
{
    public partial class Viewer : Form
    {
        public Viewer()
        {
            InitializeComponent();
            Location = Point.Empty;
            Show();
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
            int q = Convert.ToInt32(value * (1 - (f * saturation)));
            int t = Convert.ToInt32(value * (1 - ((1 - f) * saturation)));

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
