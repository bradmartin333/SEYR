using SEYR.Session;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
        public static void LoadImage(Bitmap bmp)
        {
            Channel.Project.ImageHeight = bmp.Height;
            Channel.Project.ImageWidth = bmp.Width;
            ProcessImage(bmp, NullTile);
        }

        /// <summary>
        /// Analyze an image and output it's string representation
        /// </summary>
        /// <param name="bmp">
        /// Filtered image
        /// </param>
        /// <param name="singleTile">
        /// Desired tile preview for Wizard
        /// </param>
        /// <returns>
        /// Either a tile preview or the entire analyzed image
        /// </returns>
        private static (Bitmap, float) ProcessImage(Bitmap bmp, Point singleTile)
        {
            ResizeAndRotate(ref bmp);

            Rectangle bmpRect = new Rectangle(Point.Empty, bmp.Size);
            BitmapData bmpData = bmp.LockBits(bmpRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int size = bmpData.Stride * bmpData.Height;
            byte[] data = new byte[size];
            Marshal.Copy(bmpData.Scan0, data, 0, size);

            (Rectangle rectangle, Size scanSize) = Channel.Project.GetGeometry();
            Size hotspotSize = new Size(rectangle.Width / scanSize.Width + 1, rectangle.Height / scanSize.Height + 1);
            Bitmap frame = new Bitmap(bmp.Width, bmp.Height);
            string outputData = string.Empty;

            using (Graphics g = Graphics.FromImage(frame))
            {
                for (int i = 0; i < Channel.Project.Columns; i++)
                {
                    for (int j = 0; j < Channel.Project.Rows; j++)
                    {
                        int thisX = rectangle.X + (int)(i * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchX);
                        int thisY = rectangle.Y + (int)(j * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchY);
                        Rectangle cropRect = new Rectangle(thisX, thisY, rectangle.Width, rectangle.Height);
                        (Bitmap tile, float entropy, Point[] hotspots) = AnalyzeData(data, cropRect, bmpData.Stride, scanSize, hotspotSize);
                        if (Math.Abs(Channel.Project.Score - entropy) > Channel.Project.Tolerance)
                            outputData += $"{i}\t{j}\t{entropy}\t{hotspots.Length}\tnull\n"; // Did not pass score
                        else
                        {
                            outputData += $"{i}\t{j}\t{entropy}\t{hotspots.Length}\t{GenerateTileCode(hotspots)}\n";
                            Channel.Composite.AddHotspots(hotspots, hotspotSize);
                            if (singleTile != NullTile)
                            {
                                HighlightHotspots(ref tile, hotspots, scanSize);
                                if (i == singleTile.X && j == singleTile.Y) return (tile, entropy);
                                g.DrawImage(tile, thisX, thisY);
                            }
                        }
                    }
                }
            }

            bmp.UnlockBits(bmpData);
            Channel.DataStream.Write(outputData);
            return (frame, 0f);
        }

        /// <summary>
        /// The SEYR Meat where BitmapData turns into hotspots
        /// </summary>
        /// <param name="data">
        /// Entire image's bitmap data
        /// </param>
        /// <param name="tile">
        /// Region to crop from image
        /// </param>
        /// <param name="stride">
        /// Stride of bitmap data
        /// </param>
        /// <param name="scanSize">
        /// ROI within cropped region
        /// </param>
        /// <param name="hotspotSize">
        /// Size of ROI within ROI
        /// </param>
        /// <returns></returns>
        private static (Bitmap, float, Point[]) AnalyzeData(byte[] data, Rectangle tile, int stride, Size scanSize, Size hotspotSize)
        {
            byte threshold = (byte)(255 * Channel.Project.Threshold);
            byte[] croppedBytes = new byte[tile.Width * tile.Height * 3];

            List<int>[,] hotspotData = new List<int>[hotspotSize.Width, hotspotSize.Height];
            List<int> hotspotData1D = new List<int>();
            for (int i = 0; i < hotspotSize.Width; i++)
                for (int j = 0; j < hotspotSize.Height; j++)
                    hotspotData[i, j] = new List<int>();

            for (int i = 0; i < tile.Height; i++)
            {
                for (int j = 0; j < tile.Width * 3; j += 3)
                {
                    int idx = (tile.Y * stride) + (i * stride) + (tile.X * 3) + j;
                    int croppedIndex = (i * tile.Width * 3) + j;
                    for (int k = 0; k < 3; k++) // Pixel data formatted BGR
                        if (croppedIndex + k < croppedBytes.Length) croppedBytes[croppedIndex + k] = data[idx + k];
                    if (idx + 2 < data.Length) // Determine if it is wihtin the padding
                    {
                        byte R = (byte)(data[idx + 2] > threshold ? 255 : 0);
                        byte G = (byte)(data[idx + 1] > threshold ? 255 : 0);
                        byte B = (byte)(data[idx + 0] > threshold ? 255 : 0);
                        int RGB = R << 16 | G << 8 | B;
                        hotspotData[j / 3 / scanSize.Width, i / scanSize.Height].Add(RGB);
                        hotspotData1D.Add(RGB);
                    }
                }
            }

            Bitmap croppedBitmap = new Bitmap(tile.Width, tile.Height);
            BitmapData croppedData = croppedBitmap.LockBits(new Rectangle(0, 0, tile.Width, tile.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            Marshal.Copy(croppedBytes, 0, croppedData.Scan0, croppedBytes.Length);
            croppedBitmap.UnlockBits(croppedData);

            List<Point3D> hotspots = MakeHotspots(hotspotData, hotspotSize.Width, hotspotSize.Height, scanSize);
            Point[] selectedHotspots = hotspots.Where(t => t.Z > Channel.Project.Contrast).Select(t => new Point(t.X, t.Y)).ToArray();
            float entropy = (float)Math.Round(CalculateShannonEntropy(hotspotData1D.ToArray(), tile.Size), 3);

            return (croppedBitmap, entropy, selectedHotspots);
        }

        private static List<Point3D> MakeHotspots(List<int>[,] data, int cols, int rows, Size size)
        {
            List<Point3D> hotspots = new List<Point3D>();
            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                    hotspots.Add(new Point3D() { X = i, Y = j, Z = CalculateShannonEntropy(data[i, j].ToArray(), size) });
            return hotspots;
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

        public static void ApplyManualThreshold(ref Bitmap bmp)
        {
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetThreshold(Channel.Project.Threshold);
            using (Graphics g = Graphics.FromImage(bmp))
                g.DrawImage(bmp, new Rectangle(Point.Empty, bmp.Size), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttr);
        }

        #endregion

        #region Wizard Functions

        public static void DrawGrid(ref Bitmap bmp)
        {
            ResizeAndRotate(ref bmp);
            Rectangle rectangle = Channel.Project.GetGeometry().Item1;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < Channel.Project.Columns; i++)
                {
                    for (int j = 0; j < Channel.Project.Rows; j++)
                    {
                        int thisX = rectangle.X + (int)(i * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchX);
                        int thisY = rectangle.Y + (int)(j * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchY);
                        g.DrawRectangle(new Pen(Brushes.LawnGreen, (float)(Math.Min(bmp.Height, bmp.Width) * 0.005)),
                            thisX, thisY, rectangle.Width, rectangle.Height);
                    }
                }
            }
        }

        public static (Bitmap, float) GenerateSingleTile(Bitmap bmp, int tileRow, int tileColumn)
        {
            return ProcessImage(bmp, new Point(tileColumn - 1, tileRow - 1));
        }

        #endregion

        #region Data Representation

        private static string GenerateTileCode(Point[] focusTiles)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Point tile in focusTiles)
                stringBuilder.Append($"{tile.X} {tile.Y} ");
            using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString())))
            {
                using (var compressedStream = new MemoryStream())
                {
                    using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Fastest, false))
                        uncompressedStream.CopyTo(compressorStream);
                    return Convert.ToBase64String(compressedStream.ToArray());
                }
            }
        }

        private static void HighlightHotspots(ref Bitmap bmp, Point[] tiles, Size scanSize)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                foreach (Point tile in tiles)
                    g.FillRectangle(Brushes.LawnGreen, new Rectangle(tile.X * scanSize.Width, tile.Y * scanSize.Height, scanSize.Width, scanSize.Height));
            }
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
