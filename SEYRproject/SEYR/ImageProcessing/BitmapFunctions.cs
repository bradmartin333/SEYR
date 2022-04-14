using SEYR.Session;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SEYR.ImageProcessing
{
    internal static class BitmapFunctions
    {
        private static readonly Point NullTile = new Point(-1, -1);

        private struct Point3D
        {
            public int X { get; set; }
            public int Y { get; set; }
            public float Z { get; set; }
        }

        /// <summary>
        /// Send a new image into SEYR
        /// </summary>
        /// <param name="bmp"></param>
        public static async void LoadImage(Bitmap bmp)
        {
            Channel.Project.ImageHeight = bmp.Height;
            Channel.Project.ImageWidth = bmp.Width;
            await ProcessImage(bmp, NullTile);
        }

        /// <summary>
        /// Analyze an image and output it's string representation
        /// </summary>
        /// <param name="bmp">
        /// Filtered image
        /// </param>
        /// <param name="desiredTile">
        /// Desired tile preview for Composer
        /// </param>
        /// <param name="desiredFeature"></param>
        /// <returns>
        /// Either a tile preview or the entire analyzed image
        /// </returns>
        private static async Task<Bitmap> ProcessImage(Bitmap bmp, Point desiredTile, Feature desiredFeature = null)
        {
            ResizeAndRotate(ref bmp);
            Rectangle rectangle = Channel.Project.GetGeometry();
            string outputData = string.Empty;

            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < Channel.Project.Columns; i++)
                {
                    for (int j = Channel.Project.Rows - 1; j >= 0; j--)
                    {
                        int thisX = rectangle.X + (int)(i * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchX);
                        int thisY = rectangle.Y + (int)(j * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchY);
                        Rectangle cropRect = new Rectangle(thisX, thisY, rectangle.Width, rectangle.Height);
                        Bitmap crop = new Bitmap(cropRect.Width, cropRect.Height);
                        using (Graphics g2 = Graphics.FromImage(crop))
                        {
                            g2.DrawImage(bmp, new Rectangle(Point.Empty, crop.Size), cropRect, GraphicsUnit.Pixel);
                            foreach (Feature feature in Channel.Project.Features)
                            {
                                float entropy = await Task.Run(() => AnalyzeData(crop, feature));
                                feature.AddScore(entropy);
                                g2.FillRectangle(new SolidBrush(ColorFromHSV(entropy, feature.GetMinScore(), feature.GetMaxScore(), 100)), feature.GetGeometry());
                                Color border = (desiredFeature != null && feature.Name == desiredFeature.Name) ? Color.HotPink : Color.Black;
                                g2.DrawRectangle(new Pen(border, (float)Channel.Project.ScaledPixelsPerMicron), feature.GetGeometry());
                                outputData += $"{desiredTile.X + 1}\t{Channel.Project.Rows - desiredTile.Y}\t{feature.Name}\t{entropy}\n";
                            }
                        }
                        if (i == desiredTile.X && j == desiredTile.Y)
                        {
                            Channel.DebugStream.Write($"Return tile {desiredTile.X + 1}, {Channel.Project.Rows - desiredTile.Y} and feature " +
                                $"{(desiredFeature != null ? desiredFeature.Name : "null")}");
                            return crop;
                        }
                        g.DrawImage(crop, thisX, thisY);
                    }   
                }
            }

            Channel.Viewer.UpdateImage(bmp);
            Channel.DataStream.Write(outputData, false);
            return bmp;
        }

        private static float AnalyzeData(Bitmap bmpTile, Feature feature)
        {
            Rectangle cropRect = feature.GetGeometry();
            Bitmap bmp = new Bitmap(feature.Rectangle.Width, feature.Rectangle.Height);
            using (Graphics g = Graphics.FromImage(bmp))
                g.DrawImage(bmpTile, new Rectangle(Point.Empty, cropRect.Size), cropRect, GraphicsUnit.Pixel);

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            byte threshold = (byte)(255 * feature.Threshold);
            List<byte> rVals = new List<byte>();
            List<byte> rtVals = new List<byte>();
            for (int counter = 2; counter < rgbValues.Length; counter += 3)
            {
                byte r =rgbValues[counter];
                byte rt = (byte)(r > threshold ? 1 : 0);
                rVals.Add(r);
                rtVals.Add(rt);
            }

            bmp.UnlockBits(bmpData);

            int blackVals = rtVals.Where(x => x == 0).Count();
            int whiteVals = rtVals.Where(x => x == 1).Count();
            float score = (float)Math.Round(CalculateShannonEntropy(rVals.ToArray(), rect.Size) + (whiteVals / 2), 3);
            if (blackVals < 10 || whiteVals < 10)
                return 0f; // Look at feature params here
            else
                return score;
        }

        private static float CalculateShannonEntropy(byte[] data, Size size)
        {
            float entropy = 0; // Calculate Shannon entropy
            foreach (var g in data.GroupBy(i => i)) // Turn list into an array of counts
            {
                double val = g.Count() / (double)(size.Width * size.Height);
                entropy -= (float)(val * Math.Log(val, 2));
            }
            return entropy;
        }

        #region Composer Functions

        public static async Task<Bitmap> DrawGrid(Bitmap bmp, int tileRow, int tileColumn)
        {
            ResizeAndRotate(ref bmp);
            Rectangle rectangle = Channel.Project.GetGeometry();
            await Task.Run(() => {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    for (int i = 0; i < Channel.Project.Columns; i++)
                    {
                        for (int j = Channel.Project.Rows - 1; j >= 0; j--)
                        {
                            int thisX = rectangle.X + (int)(i * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchX);
                            int thisY = rectangle.Y + (int)(j * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchY);
                            g.DrawRectangle(new Pen((i == tileColumn - 1 && Channel.Project.Rows - tileRow == j) ? Brushes.HotPink : Brushes.LawnGreen,
                                (float)(Math.Min(bmp.Height, bmp.Width) * 0.005)),
                                thisX, thisY, rectangle.Width, rectangle.Height);
                        }
                    }
                }
            });
            return bmp;
        }

        public static async Task<Bitmap> GenerateSingleTile(Bitmap bmp, int tileRow, int tileColumn, Feature feature)
        {
            return await ProcessImage(bmp, new Point(tileColumn - 1, Channel.Project.Rows - tileRow), feature);
        }

        #endregion

        #region Boilerplate Methods

        public static void ResizeAndRotate(ref Bitmap bmp)
        {
            Bitmap resize = new Bitmap((int)(Channel.Project.Scaling * bmp.Width), (int)(Channel.Project.Scaling * bmp.Height));
            using (Graphics g = Graphics.FromImage(resize))
                g.DrawImage(bmp, 0, 0, resize.Width, resize.Height);
            bmp = RotateImage(resize, Channel.Project.Angle);
        }

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

        private static Color ColorFromHSV(double hue, double fromLow, double fromHigh, byte opacity, double value = 1, double saturation = 1)
        {
            double toLow = 255;
            double toHigh = 0;
            hue = (hue - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
            if (hue == 360) return Color.FromArgb(255, Color.White);
            if (hue == 0) return Color.FromArgb(255, Color.Black);
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = (hue / 60) - Math.Floor(hue / 60);
            value *= 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - (f * saturation)));
            int t = Convert.ToInt32(value * (1 - ((1 - f) * saturation)));
            if (hi == 0)
                return Color.FromArgb(opacity, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(opacity, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(opacity, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(opacity, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(opacity, t, p, v);
            else
                return Color.FromArgb(opacity, v, p, q);
        }

        #endregion
    }
}
