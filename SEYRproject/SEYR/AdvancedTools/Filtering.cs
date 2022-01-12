using Accord.Imaging;
using Accord.Imaging.Filters;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace SEYR.AdvancedTools
{
    public static class Filtering
    {
        public static int DetectThreshold()
        {
            Bitmap bmp = (Bitmap)Imaging.DisplayedImage.Clone();
            double[] pVisibleDiff = new double[255];
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);

            for (int i = 0; i < 255; i += 1)
            {
                Bitmap working = bmp.Clone(new Rectangle(new Point(0, 0), bmp.Size), PixelFormat.Format32bppArgb);
                working = filter.Apply(working);
                Threshold threshold = new Threshold(i);
                threshold.ApplyInPlace(working);
                ImageStatistics stats = new ImageStatistics(working);
                pVisibleDiff[i] = Math.Abs((stats.PixelsCountWithoutBlack / (double)stats.PixelsCount) - 0.035);
            }

            return pVisibleDiff.ToList().IndexOf(pVisibleDiff.Min());
        }
    }
}
