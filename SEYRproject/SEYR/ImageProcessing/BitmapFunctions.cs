using SEYR.Session;
using System;
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
        /// <param name="singleTile">
        /// Desired tile preview for Composer
        /// </param>
        /// <returns>
        /// Either a tile preview or the entire analyzed image
        /// </returns>
        private static async Task<(Bitmap, float)> ProcessImage(Bitmap bmp, Point singleTile)
        {
            ResizeAndRotate(ref bmp);
            Rectangle rectangle = Channel.Project.GetGeometry();
            Bitmap frame = new Bitmap(bmp.Width, bmp.Height);
            string outputData = string.Empty;

            using (Graphics g = Graphics.FromImage(frame))
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
                            g2.DrawImage(bmp, new Rectangle(Point.Empty, crop.Size), cropRect, GraphicsUnit.Pixel);
                        float entropy = await Task.Run(() => AnalyzeData(ref crop));
                        g.DrawImage(crop, thisX, thisY);
                        if (i == singleTile.X && j == singleTile.Y) return (crop, entropy);
                    }
                }
            }

            Channel.DataStream.Write(outputData);
            return (frame, 0f);
        }

        private static float AnalyzeData(ref Bitmap bmp)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            byte threshold = (byte)(255 * 1);
            for (int counter = 2; counter < rgbValues.Length; counter += 3)
            {
                byte r = rgbValues[counter];
            }

            bmp.UnlockBits(bmpData);
            return 0;
        }

        private static float CalculateShannonEntropy(int[] data, Size size)
        {
            float entropy = 0; // Calculate Shannon entropy
            foreach (var g in data.GroupBy(i => i)) // Turn list into an array of counts
            {
                double val = g.Count() / (double)(size.Width * size.Height);
                entropy -= (float)(val * Math.Log(val, 2));
            }
            return entropy;
        }


        #region Image Filters

        public static void ResizeAndRotate(ref Bitmap bmp)
        {
            Bitmap resize = new Bitmap((int)(Channel.Project.Scaling * bmp.Width), (int)(Channel.Project.Scaling * bmp.Height));
            using (Graphics g = Graphics.FromImage(resize))
                g.DrawImage(bmp, 0, 0, resize.Width, resize.Height);
            bmp = RotateImage(resize, Channel.Project.Angle);
        }

        #endregion

        #region Composer Functions

        public static void DrawGrid(ref Bitmap bmp, int tileRow, int tileColumn)
        {
            ResizeAndRotate(ref bmp);
            Rectangle rectangle = Channel.Project.GetGeometry();
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
        }

        public static async Task<(Bitmap, float)> GenerateSingleTile(Bitmap bmp, int tileRow, int tileColumn)
        {
            return await ProcessImage(bmp, new Point(tileColumn - 1, Channel.Project.Rows - tileRow));
        }

        #endregion

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
