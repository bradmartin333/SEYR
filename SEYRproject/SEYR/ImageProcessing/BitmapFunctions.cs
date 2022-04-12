using Accord.Imaging.Filters;
using SEYR.Session;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SEYR.ImageProcessing
{
    internal static class BitmapFunctions
    {
        internal static Composite Composite = new Composite();

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

                        g.DrawImage(crop, thisX, thisY);
                    }
                }
            }
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
