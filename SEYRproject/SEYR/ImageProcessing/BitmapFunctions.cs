﻿using Accord.Imaging.Filters;
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
            SliceImage(bmp);
            bmp.Dispose();
            GC.Collect();
        }

        private static Bitmap SliceImage(Bitmap bmp, bool singleTile = false, int row = 0, int col = 0, bool needImg = false)
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
                        (Bitmap tile, double entropy) = GetCropEntropyARGB(data, cropRect, bmpData.Stride);
                        
                        Point[] focusTiles = GetTiles(tile);
                        outputData += $"{i}\t{j}\t{GenerateTileCode(focusTiles)}\n";
                        
                        if (needImg)
                        {
                            HighlightTiles(ref tile, focusTiles);
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
                        g.DrawRectangle(new Pen(Brushes.LawnGreen, (float)(Math.Min(bmp.Height, bmp.Width) * 0.005)),
                            rectangle.X + thisX, rectangle.Y - thisY, rectangle.Width, rectangle.Height);
                    }
                }
            }
        }

        internal static Bitmap GenerateSingleTile(Bitmap bmp, int tileRow, int tileColumn)
        {
            ApplyFilters(ref bmp);
            return SliceImage(bmp, true, tileRow - 1, tileColumn - 1, true);
        }

        /// <summary>
        /// Get ROI within image
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns>
        /// Tiles within a grid that have entropies in the highest 2 histogram bins
        /// </returns>
        private static Point[] GetTiles(Bitmap bmp)
        {
            Rectangle bmpRect = new Rectangle(Point.Empty, bmp.Size);
            BitmapData bmpData = bmp.LockBits(bmpRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int size = bmpData.Stride * bmpData.Height;
            byte[] data = new byte[size];
            Marshal.Copy(bmpData.Scan0, data, 0, size);

            Size scanSize = new Size((int)Math.Round(bmp.Width / (double)Channel.Project.Density, MidpointRounding.AwayFromZero), 
                (int)Math.Round(bmp.Height / (double)Channel.Project.Density, MidpointRounding.AwayFromZero));
            List<Point3D> tiles = new List<Point3D>();

            for (int i = 0; i < bmp.Width; i += scanSize.Width)
                for (int j = 0; j < bmp.Height; j += scanSize.Height)
                {
                    Rectangle rect = new Rectangle(i, j, scanSize.Width, scanSize.Height);
                    if (bmpRect.Contains(rect)) tiles.Add(new Point3D() { X = i, Y = j, Z = GetCropEntropyARGB(data, rect, bmpData.Stride).Item2 });
                }

            bmp.UnlockBits(bmpData);
            Point[] selectedTiles = tiles.Where(t => t.Z > Channel.Project.Contrast).Select(t => new Point(t.X, t.Y)).ToArray();
            Channel.Composite.AddTiles(selectedTiles, bmp.Size);
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
        private static (Bitmap, float) GetCropEntropyARGB(byte[] data, Rectangle tile, int stride)
        {
            byte[] croppedBytes = new byte[tile.Width * tile.Height * 3];

            for (int i = 0; i < tile.Height; i++)
            {
                for (int j = 0; j < tile.Width * 3; j += 3)
                {
                    int origIndex = (tile.Y * stride) + (i * stride) + (tile.X * 3) + j;
                    int croppedIndex = (i * tile.Width * 3) + j;
                    for (int k = 0; k < 3; k++)
                        if (croppedIndex + k < croppedBytes.Length) croppedBytes[croppedIndex + k] = data[origIndex + k];
                }
            }

            Bitmap croppedBitmap = new Bitmap(tile.Width, tile.Height);
            BitmapData croppedData = croppedBitmap.LockBits(new Rectangle(0, 0, tile.Width, tile.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            Marshal.Copy(croppedBytes, 0, croppedData.Scan0, croppedBytes.Length);
            croppedBitmap.UnlockBits(croppedData);

            List<int> counts = new List<int>(); // Each int is a ARGB value of a pixel
            for (int i = tile.Left; i < tile.Right; i++)
                for (int j = tile.Top; j < tile.Bottom; j++)
                {
                    int idx = (i * 3) + (j * stride); // Find starting index of pixel in data
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
            return (croppedBitmap, entropy);
        }

        /// <summary>
        /// Colorize the used tiles from the grid training image
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="tiles"></param>
        private static void HighlightTiles(ref Bitmap bmp, Point[] tiles)
        {
            Size scanSize = new Size(bmp.Width / Channel.Project.Density, bmp.Height / Channel.Project.Density);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                foreach (Point tile in tiles)
                {
                    Rectangle rect = new Rectangle(tile.X, tile.Y, scanSize.Width, scanSize.Height);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.LawnGreen)), rect);
                }
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
