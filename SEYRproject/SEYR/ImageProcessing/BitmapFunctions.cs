using Accord.Imaging.Filters;
using SEYR.Session;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace SEYR.ImageProcessing
{
    internal static class BitmapFunctions
    {
        internal static Composite Composite = new Composite();
        private static double _GridSize = 15;
        private static double _AmountDataDesired = 0.5; // Highest 10% of available data from training grid

        private struct Point2D
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        private struct Point3D
        {
            public int X { get; set; }
            public int Y { get; set; }
            public float Z { get; set; }
        }

        public static void LoadImage(Bitmap bmp)
        {
            ApplyFilters(ref bmp, true);
            Bitmap test = SliceImage(bmp);
            Composite.BackgroundImage = test;
            bmp.Dispose();
            GC.Collect();
        }

        private static Bitmap SliceImage(Bitmap bmp)
        {
            Rectangle rectangle = Channel.Project.GetGeometry();
            Bitmap frame = new Bitmap(bmp.Width, bmp.Height);
            string data = string.Empty;
            using (Graphics g = Graphics.FromImage(frame))
            {
                for (int i = 0; i < Channel.Project.Columns; i++)
                {
                    for (int j = 0; j < Channel.Project.Rows; j++)
                    {
                        int thisX = rectangle.X + (int)(i * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchX);
                        int thisY = rectangle.Y - (int)(j * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchY);
                        Rectangle cropRect = new Rectangle(thisX, thisY, rectangle.Width, rectangle.Height);
                        Bitmap crop = new Bitmap(rectangle.Width, rectangle.Height);
                        for (int k = 0; k < rectangle.Width; k++)
                            for (int l = 0; l < rectangle.Height; l++)
                                if (cropRect.X + k > 0 && cropRect.Y + l > 0 && cropRect.X + k <= bmp.Width && cropRect.Y + l <= bmp.Height)
                                    crop.SetPixel(k, l, bmp.GetPixel(cropRect.X + k, cropRect.Y + l));
                        Point2D[] focusTiles = GetTiles(crop);
                        double score = ScoreImage(crop, focusTiles);
                        data += $"{i}\t{j}\t{score}\n";
                        HighlightTiles(ref crop, focusTiles);
                        g.DrawImage(crop, thisX, thisY);
                    }
                }
            }
            Channel.DataStream.Write(data);
            return frame;
        }

        public static void ApplyFilters(ref Bitmap bmp, bool color = false)
        {
            // Resize incoming image
            Bitmap resize = new Bitmap((int)(Channel.Project.Scaling * bmp.Width), (int)(Channel.Project.Scaling * bmp.Height));
            using (Graphics g = Graphics.FromImage(resize))
                g.DrawImage(bmp, 0, 0, resize.Width, resize.Height);

            // Clone with necessary pixel format for image filtering
            Bitmap working = resize.Clone(new Rectangle(new Point(0, 0), resize.Size), PixelFormat.Format32bppArgb);
            working = RotateImage(working, Channel.Project.Angle);
            if (!color)
            {
                Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
                working = filter.Apply(working);
                Threshold threshold = new Threshold(Channel.Project.Threshold);
                threshold.ApplyInPlace(working);
            }

            Channel.Project.ImageHeight = working.Height;
            Channel.Project.ImageWidth = working.Width;
            bmp = working;
        }

        internal static void DrawGrid(ref Bitmap bmp)
        {
            ApplyFilters(ref bmp, true);
            Rectangle rectangle = Channel.Project.GetGeometry();
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < Channel.Project.Columns; i++)
                {
                    for (int j = 0; j < Channel.Project.Rows; j++)
                    {
                        int thisX = (int)(i * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchX);
                        int thisY = (int)(j * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchY);
                        g.DrawRectangle(new Pen(Brushes.HotPink, (float)(Math.Min(bmp.Height, bmp.Width) * 0.005)),
                            rectangle.X + thisX, rectangle.Y - thisY, rectangle.Width, rectangle.Height);
                    }
                }
            }
        }

        /// <summary>
        /// Return average of scores
        /// for desired tiles in image
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="tiles"></param>
        /// <returns></returns>
        private static double ScoreImage(Bitmap bmp, Point2D[] tiles)
        {
            Rectangle bmpRect = new Rectangle(Point.Empty, bmp.Size);
            BitmapData bmpData = bmp.LockBits(bmpRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int size = bmpData.Stride * bmpData.Height;
            byte[] data = new byte[size];
            Marshal.Copy(bmpData.Scan0, data, 0, size);

            Size scanSize = new Size(bmp.Width / (int)_GridSize, bmp.Height / (int)_GridSize);
            List<double> scores = new List<double>();

            foreach (Point2D tile in tiles)
            {
                Rectangle rect = new Rectangle((int)tile.X, (int)tile.Y, scanSize.Width, scanSize.Height);
                if (bmpRect.Contains(rect)) scores.Add(ScoreTile(data, rect, bmpData.Stride));
            }

            bmp.UnlockBits(bmpData);
            return scores.Average();
        }

        /// <summary>
        /// Core of AutoFocus
        /// </summary>
        /// <param name="data">
        /// BitmapData for current image
        /// </param>
        /// <param name="tile">
        /// Rectangle to crop BitmapData
        /// </param>
        /// <param name="stride"></param>
        /// <returns>
        /// Calculated score for tile
        /// </returns>
        private static double ScoreTile(byte[] data, Rectangle tile, int stride)
        {
            List<double> PDiff = new List<double>(); // Turn the data array into a convolutional data array

            for (int i = tile.Left; i < tile.Right; i += (int)(1 / _AmountDataDesired))
                for (int j = tile.Top; j < tile.Bottom; j += (int)(1 / _AmountDataDesired))
                {
                    int idx = i * 3 + j * stride; // Find starting index of pixel in data
                    if (idx + 2 < data.Length) // Determine if it is wihtin the padding
                    {
                        byte p1 = data[idx + 2]; // Get current pixel's Red channel value
                        double localPDiff = 0; // Percent difference of p1 and neighboring p2's
                        int counts = 0; // Number of p2's we found
                        for (int k = i - (int)(0.5 / _AmountDataDesired); k < i + (int)(0.5 / _AmountDataDesired); k++)
                            for (int l = j - (int)(0.5 / _AmountDataDesired); l < j + (int)(0.5 / _AmountDataDesired); l++)
                            {
                                int neighborIdx = k * 3 + l * stride;  // Find starting index of pixel in data
                                if (neighborIdx + 2 > 0 && neighborIdx + 2 < data.Length)  // Determine if it is wihtin the array
                                {
                                    byte p2 = data[neighborIdx + 2]; // Get current pixel's Red channel value
                                    localPDiff += Math.Abs(p1 - p2) / (double)((p1 + p2) / 2.0); // Add the calculated percent difference
                                    counts++;
                                }
                            }
                        PDiff.Add(localPDiff / counts); // Add the average to the list
                    }
                }

            return PDiff.Average();
        }

        /// <summary>
        /// Get ROI within image
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns>
        /// Tiles within a grid that have entropies in the highest 2 histogram bins
        /// </returns>
        private static Point2D[] GetTiles(Bitmap bmp)
        {
            Rectangle bmpRect = new Rectangle(Point.Empty, bmp.Size);
            BitmapData bmpData = bmp.LockBits(bmpRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int size = bmpData.Stride * bmpData.Height;
            byte[] data = new byte[size];
            Marshal.Copy(bmpData.Scan0, data, 0, size);

            Size scanSize = new Size(bmp.Width / (int)_GridSize, bmp.Height / (int)_GridSize);
            List<Point3D> tiles = new List<Point3D>();

            for (int i = 0; i < bmp.Width; i += scanSize.Width)
                for (int j = 0; j < bmp.Height; j += scanSize.Height)
                {
                    Rectangle rect = new Rectangle(i, j, scanSize.Width, scanSize.Height);
                    if (bmpRect.Contains(rect)) tiles.Add(new Point3D() { X = i, Y = j, Z = GetCropEntropyARGB(data, rect, bmpData.Stride) });
                }

            bmp.UnlockBits(bmpData);
            tiles.Sort((x, y) => y.Z.CompareTo(x.Z)); // Sort list by greatest entropy to smallest entropy
            Point2D[] selectedTiles = tiles.Take((int)(tiles.Count() * _AmountDataDesired)).Select(x => new Point2D() { X = x.X, Y = x.Y }).ToArray();
            return selectedTiles;
        }

        /// <summary>
        /// Crop BMP Pixel Data
        /// and calculate entropy
        /// for that tile
        /// </summary>
        /// <param name="data">
        /// BitmapData for current image
        /// </param>
        /// <param name="tile">
        /// Rectangle to crop BitmapData
        /// </param>
        /// <param name="stride"></param>
        /// <returns></returns>
        private static float GetCropEntropyARGB(byte[] data, Rectangle tile, int stride)
        {
            List<int> counts = new List<int>(); // Each int is a ARGB value of a pixel
            for (int i = tile.Left; i < tile.Right; i += (int)(1 / _AmountDataDesired))
                for (int j = tile.Top; j < tile.Bottom; j += (int)(1 / _AmountDataDesired))
                {
                    int idx = i * 3 + j * stride; // Find starting index of pixel in data
                    if (idx + 2 < data.Length) // Determine if it is wihtin the padding
                        counts.Add(data[idx + 2] << 16 | data[idx + 1] << 8 | data[idx]);
                    // Create ARGB int32 from RGB values which are ordered in the data array as BGR
                }
            float entropy = 0; // Calculate Shannon entropy
            foreach (var g in counts.GroupBy(i => i)) // Turn list into an array of counts
            {
                double val = g.Count() / (double)(tile.Width * tile.Height);
                entropy -= (float)(val * Math.Log(val, 2));
            }
            return entropy;
        }

        /// <summary>
        /// Colorize the used tiles from the grid training image
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="tiles"></param>
        private static void HighlightTiles(ref Bitmap bmp, Point2D[] tiles)
        {
            Size scanSize = new Size(bmp.Width / (int)_GridSize, bmp.Height / (int)_GridSize);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                foreach (Point2D tile in tiles)
                {
                    Rectangle rect = new Rectangle((int)tile.X, (int)tile.Y, scanSize.Width, scanSize.Height);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Green)), rect);
                }
            }
        }

        //private static double Scan(Bitmap colorImg, Bitmap filteredImg)
        //{
        //    List<double> pixelVals = new List<double>();
        //    int whitePixels = 0;
        //    int blackPixels = 0;
        //    for (int i = 0; i < filteredImg.Width; i++)
        //    {
        //        for (int j = 0; j < filteredImg.Height; j++)
        //        {
        //            Color color = colorImg.GetPixel(i, j);
        //            pixelVals.Add(color.R);

        //            Color binary = filteredImg.GetPixel(i, j);
        //            if (binary.R == 255) whitePixels++;
        //            if (binary.R == 0) blackPixels++;
        //        }
        //    }

        //    if (whitePixels < 10 || blackPixels < 10)
        //        return 0.0;
        //    else
        //        return Math.Round(Statistics.Entropy(pixelVals.ToArray()), 1) + (whitePixels / 2);
        //    // Add a fraction of the number of white pixels to simulate a rotation filter
        //}

        /// <summary>
        /// Method to rotate an image either clockwise or counter-clockwise
        /// </summary>
        /// <param name="img">the image to be rotated</param>
        /// <param name="rotationAngle">the angle (in degrees).
        /// NOTE: 
        /// Positive values will rotate clockwise
        /// Negative values will rotate counter-clockwise
        /// </param>
        /// <returns></returns>
        private static Bitmap RotateImage(Bitmap img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics g = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            g.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            g.RotateTransform(rotationAngle);
            g.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            g.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            g.Dispose();

            //return the image
            return bmp;
        }
    }
}
