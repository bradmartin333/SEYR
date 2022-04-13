using Accord.Imaging.Filters;
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
        private struct Point3D
        {
            public int X { get; set; }
            public int Y { get; set; }
            public float Z { get; set; }
        }

        public static void LoadImage(Bitmap bmp)
        {
            ApplyFilters(ref bmp);
            ProcessImage(bmp);
            bmp.Dispose();
            GC.Collect();
        }

        private static Bitmap ProcessImage(Bitmap bmp, bool singleTile = false, int row = 0, int col = 0, bool needImg = false)
        {
            Rectangle bmpRect = new Rectangle(Point.Empty, bmp.Size);
            BitmapData bmpData = bmp.LockBits(bmpRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int size = bmpData.Stride * bmpData.Height;
            byte[] data = new byte[size];
            Marshal.Copy(bmpData.Scan0, data, 0, size);

            Rectangle rectangle = Channel.Project.GetGeometry();
            Bitmap frame = new Bitmap(bmp.Width, bmp.Height);
            string outputData = string.Empty;
            using (Graphics g = Graphics.FromImage(frame))
            {
                for (int i = 0; i < Channel.Project.Columns; i++)
                {
                    for (int j = 0; j < Channel.Project.Rows; j++)
                    {
                        int thisX = rectangle.X + (int)(i * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchX);
                        int thisY = rectangle.Y - (int)(j * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchY);
                        Rectangle cropRect = new Rectangle(thisX, thisY, rectangle.Width, rectangle.Height);
                        Size scanSize = Channel.Project.GetScanSize(cropRect.Size);
                        (Bitmap tile, double entropy, Point[] hotspots) = GetCropEntropyARGB(data, cropRect, bmpData.Stride, scanSize);
                        outputData += $"{i}\t{j}\t{entropy}\t{hotspots.Length}\t{GenerateTileCode(hotspots)}\n";
                        
                        if (needImg)
                        {
                            HighlightHotspots(ref tile, hotspots, scanSize);
                            if (singleTile && j == row && i == col) return tile;
                            g.DrawImage(tile, thisX, thisY);
                        }
                    }
                }
            }
            bmp.UnlockBits(bmpData);
            Channel.DataStream.Write(outputData);
            return frame;
        }

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

        public static void DrawGrid(ref Bitmap bmp)
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
                        g.DrawRectangle(new Pen(Brushes.LawnGreen, (float)(Math.Min(bmp.Height, bmp.Width) * 0.005)),
                            rectangle.X + thisX, rectangle.Y - thisY, rectangle.Width, rectangle.Height);
                    }
                }
            }
        }

        public static Bitmap GenerateSingleTile(Bitmap bmp, int tileRow, int tileColumn)
        {
            ApplyFilters(ref bmp);
            return ProcessImage(bmp, true, tileRow - 1, tileColumn - 1, true);
        }

        private static (Bitmap, float, Point[]) GetCropEntropyARGB(byte[] data, Rectangle tile, int stride, Size scanSize)
        {
            byte[] croppedBytes = new byte[tile.Width * tile.Height * 3];

            Size hotspotSize = new Size(tile.Width / scanSize.Width + 1, tile.Height / scanSize.Height + 1);
            List<int>[,] hotspotData = new List<int>[hotspotSize.Width, hotspotSize.Height];
            for (int i = 0; i < hotspotSize.Width; i++)
                for (int j = 0; j < hotspotSize.Height; j++)
                    hotspotData[i, j] = new List<int>();

            for (int i = 0; i < tile.Height; i++)
            {
                for (int j = 0; j < tile.Width * 3; j += 3)
                {
                    int idx = (tile.Y * stride) + (i * stride) + (tile.X * 3) + j;
                    int croppedIndex = (i * tile.Width * 3) + j;
                    for (int k = 0; k < 3; k++)
                        if (croppedIndex + k < croppedBytes.Length) croppedBytes[croppedIndex + k] = data[idx + k];
                    if (idx + 2 < data.Length) // Determine if it is wihtin the padding
                        hotspotData[j / 3 / scanSize.Width, i / scanSize.Height].Add(data[idx + 2] << 16 | data[idx + 1] << 8 | data[idx]);
                }
            }

            Bitmap croppedBitmap = new Bitmap(tile.Width, tile.Height);
            BitmapData croppedData = croppedBitmap.LockBits(new Rectangle(0, 0, tile.Width, tile.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            Marshal.Copy(croppedBytes, 0, croppedData.Scan0, croppedBytes.Length);
            croppedBitmap.UnlockBits(croppedData);

            List<Point3D> hotspots = MakeHotspots(hotspotData, hotspotSize.Width, hotspotSize.Height, scanSize);
            Point[] selectedHotspots = hotspots.Where(t => t.Z > Channel.Project.Contrast).Select(t => new Point(t.X, t.Y)).ToArray();
            Channel.Composite.AddHotspots(selectedHotspots, hotspotSize);

            return (croppedBitmap, 0, selectedHotspots);
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

        private static void HighlightHotspots(ref Bitmap bmp, Point[] tiles, Size scanSize)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                foreach (Point tile in tiles)
                    g.FillRectangle(Brushes.LawnGreen, new Rectangle(tile.X * scanSize.Width, tile.Y * scanSize.Height, scanSize.Width, scanSize.Height));
            }
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
    }
}
