using Accord.Imaging;
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
        private static readonly Point NullPoint = new Point(-1, -1);
        private static Point Offset = Point.Empty;

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
        /// <param name="forcePattern"></param>
        public static async Task LoadImage(Bitmap bmp, bool forcePattern)
        {
            Channel.Project.ImageHeight = bmp.Height;
            Channel.Project.ImageWidth = bmp.Width;
            await ProcessImage(bmp, forcePattern, NullPoint);
        }

        /// <summary>
        /// Analyze an image and output it's string representation
        /// </summary>
        /// <param name="bmp">
        /// Filtered image
        /// </param>
        /// <param name="forcePattern">
        /// User wants to test pattern right now
        /// </param>
        /// /// <param name="desiredTile">
        /// Desired tile preview for Composer
        /// </param>
        /// <param name="desiredFeature">
        /// Desired feature for Composer
        /// </param>
        /// <param name="graphics">
        /// Return image with no added feature graphics
        /// </param>
        /// <returns>
        /// Either a tile preview or the entire analyzed image
        /// </returns>
        private static async Task<Bitmap> ProcessImage(Bitmap bmp, bool forcePattern, Point desiredTile, Feature desiredFeature = null, bool graphics = true)
        {
            ResizeAndRotate(ref bmp);

            if (desiredTile == NullPoint ) 
                Offset = await FollowPattern(bmp, forcePattern);
            else
            {
                Rectangle cropRect = Channel.Project.GetIndexedGeometry(desiredTile.X, desiredTile.Y, Offset);
                Bitmap crop = new Bitmap(cropRect.Width, cropRect.Height);
                await ProcessTile(desiredTile.X, desiredTile.Y, bmp, ref crop, cropRect, desiredFeature, graphics);
                await Channel.DebugStream.Write($"Get tile: {desiredTile} and feature: {(desiredFeature == null ? "null" : desiredFeature.Name)}");
                return crop;
            }

            string outputData = string.Empty;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < Channel.Project.Columns; i++)
                {
                    for (int j = Channel.Project.Rows - 1; j >= 0; j--)
                    {
                        Rectangle cropRect = Channel.Project.GetIndexedGeometry(i, j, Offset);
                        Bitmap crop = new Bitmap(cropRect.Width, cropRect.Height);
                        outputData += await ProcessTile(i, j, bmp, ref crop, cropRect, desiredFeature, graphics);
                        g.DrawImage(crop, cropRect.X, cropRect.Y);
                    }   
                }
            }
            Channel.Viewer.UpdateImage(bmp);
            await Channel.DataStream.Write(outputData, false);
            return bmp;
        }

        private static Task<string> ProcessTile(int i, int j, Bitmap bmp, ref Bitmap crop, Rectangle cropRect, Feature desiredFeature, bool graphics)
        {
            string outputData = string.Empty;
            using (Graphics g = Graphics.FromImage(crop))
            {
                g.DrawImage(bmp, new Rectangle(Point.Empty, crop.Size), cropRect, GraphicsUnit.Pixel);
                if (graphics)
                {
                    Bitmap tile = (Bitmap)crop.Clone();
                    foreach (Feature feature in Channel.Project.Features)
                    {
                        float score = AnalyzeData(tile, feature);
                        feature.Scores.Add(score);
                        if (score == 1000f) // Special pass
                        {
                            g.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.LawnGreen)), feature.GetGeometry());
                            if (desiredFeature != null && feature.Name == desiredFeature.Name) // Special pass selected
                                g.DrawRectangle(new Pen(Color.Black, Channel.Project.ScaledPixelsPerMicron), feature.GetGeometry());
                        }
                        else if (score > 0) // Normal
                        {
                            g.DrawRectangle(
                                new Pen(feature.ColorFromScore(), Channel.Project.ScaledPixelsPerMicron),
                                feature.GetGeometry());
                            if (desiredFeature != null && feature.Name == desiredFeature.Name) // Normal Selected
                                g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Gold)), feature.GetGeometry());
                        }
                        else // Special fail
                        {
                            g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Firebrick)), feature.GetGeometry());
                            if (desiredFeature != null && feature.Name == desiredFeature.Name) // Special fail selected
                                g.DrawRectangle(new Pen(Color.Black, Channel.Project.ScaledPixelsPerMicron), feature.GetGeometry());
                        }
                        if (desiredFeature == null) outputData += $"{Channel.OutputData}\t{Channel.Project.Rows - j}\t{i + 1}\t{feature.Name}\t{score}\n";
                    }
                }
            }
            return Task.FromResult(outputData);
        }

        private static float AnalyzeData(Bitmap bmpTile, Feature feature)
        {
            Rectangle cropRect = feature.GetGeometry();
            Bitmap bmp = new Bitmap(feature.Rectangle.Width, feature.Rectangle.Height);
            using (Graphics g = Graphics.FromImage(bmp))
                g.DrawImage(bmpTile, new Rectangle(Point.Empty, cropRect.Size), cropRect, GraphicsUnit.Pixel);

            (byte[] rVals, byte[] rtVals, Rectangle rect) = GetPixelData(bmp, (byte)(feature.Threshold * 255));
            int blackVals = rtVals.Where(x => x == 0).Count();
            int whiteVals = rtVals.Where(x => x == 1).Count();
            int filterVal = (int)(0.2 * (bmp.Width * bmp.Height));
            float entropy = CalculateShannonEntropy(rVals, rect.Size);
            float score = entropy > 0 ? (float)Math.Round(entropy + (whiteVals / 2), 3) : 0;

            if (blackVals < filterVal || whiteVals < filterVal)
            {
                switch (feature.NullDetection)
                {
                    case Feature.NullDetectionTypes.None:
                        return 0f;
                    case Feature.NullDetectionTypes.IncludeEmpty:
                        if (whiteVals < filterVal) return 1000f;
                        else return 0f;
                    case Feature.NullDetectionTypes.IncludeFilled:
                        if (blackVals < filterVal) return 1000f;
                        else return 0f;
                    case Feature.NullDetectionTypes.IncludeBoth:
                        return 1000f;
                    default:
                        return 0f;
                }
            }
            else
                return score;
        }

        private static (byte[], byte[], Rectangle) GetPixelData(Bitmap bmp, float threshold)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            List<byte> rVals = new List<byte>();
            List<byte> rtVals = new List<byte>();
            for (int counter = 2; counter < rgbValues.Length; counter += 3)
            {
                byte r = rgbValues[counter];
                byte rt = (byte)(r > threshold ? 1 : 0);
                rVals.Add(r);
                rtVals.Add(rt);
            }

            bmp.UnlockBits(bmpData);
            return (rVals.ToArray(), rtVals.ToArray(), rect);
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

        private static async Task<Point> FollowPattern(Bitmap bmp, bool forcePattern)
        {
            if (Channel.Pattern != null && DataStream.Header != null && Channel.Project.PatternIntervalValue != 0)
            {
                if (forcePattern)
                    return await FindPattern(bmp);
                else
                {
                    string[] cols = DataStream.Header.Split('\t');
                    for (int i = 0; i < cols.Length; i++)
                        if (!string.IsNullOrEmpty(cols[i]))
                            if (cols[i] == Channel.Project.PatternIntervalString)
                                if (int.TryParse(Channel.OutputData.Split('\t')[i], out int matchVal))
                                    if (matchVal % Channel.Project.PatternIntervalValue == 0)
                                    {
                                        await Channel.DebugStream.Write($"Pattern follower interval hit: {Channel.OutputData}");
                                        return await FindPattern(bmp);
                                    }
                }
            }
            return NullPoint;
        }

        private static async Task<Point> FindPattern(Bitmap bmp)
        {
            Bitmap sourceImage = (Bitmap)bmp.Clone();
            Bitmap pattern = (Bitmap)Channel.Pattern.Clone();
            var tm = new ExhaustiveTemplateMatching(Channel.Project.PatternScore);
            TemplateMatch[] matchings = tm.ProcessImage(sourceImage, pattern);
            TemplateMatch m = matchings[0];
            foreach (Point point in Channel.Project.PatternLocations)
            {
                Point delta = new Point(point.X - m.Rectangle.Center().X, point.Y - m.Rectangle.Center().Y);
                int deltaH = (int)Math.Sqrt(Math.Abs(Math.Pow(delta.X, 2) + Math.Pow(delta.Y, 2)));
                if (deltaH <= Channel.Project.PatternDeltaMax)
                {
                    await Channel.DebugStream.Write($"Pattern follower delta = {deltaH}", showInViewer: true);
                    return delta;
                }    
            }
            if (Channel.Project.PatternLocations.Count == 0)
                await Channel.DebugStream.Write($"Pattern location not taught", showInViewer: true);
            else
                await Channel.DebugStream.Write($"Failed to find valid pattern. Best score = {m.Similarity}", showInViewer: true);
            return NullPoint;
        }

        #region Composer Functions

        public static async Task<Bitmap> DrawGrid(Bitmap bmp, int tileRow, int tileColumn)
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
            return bmp;
        }

        public static async Task<Bitmap> GenerateSingleTile(Bitmap bmp, int tileRow, int tileColumn, Feature feature, bool graphics = true)
        {
            return await ProcessImage(bmp, false, new Point(tileColumn - 1, Channel.Project.Rows - tileRow), feature, graphics);
        }

        #endregion

        #region Boilerplate Methods

        public static void ResizeAndRotate(ref Bitmap bmp)
        {
            Bitmap resize = new Bitmap((int)(Channel.Project.Scaling * bmp.Width), (int)(Channel.Project.Scaling * bmp.Height), PixelFormat.Format24bppRgb);
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
            Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb);
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

        #endregion
    }
}
