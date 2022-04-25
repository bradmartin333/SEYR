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
        /// <param name="imageInfo"></param>
        /// <param name="customFilter"></param>
        public static async Task<double> LoadImage(Bitmap bmp, bool forcePattern, string imageInfo, bool customFilter = false)
        {
            Channel.Project.ImageHeight = bmp.Height;
            Channel.Project.ImageWidth = bmp.Width;
            (Bitmap, double) result;
            if (customFilter)
            {
                result = CustomProcessImage(bmp);
                Channel.CustomImage = result.Item1;
            }
            else
                result = await ProcessImage(bmp, forcePattern, NullPoint, imageInfo);
            return result.Item2; // Percent passing features within image
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
        /// <param name="imageInfo"></param>
        /// <param name="desiredFeature">
        /// Desired feature for Composer
        /// </param>
        /// <param name="graphics">
        /// Return image with no added feature graphics
        /// </param>
        /// <returns>
        /// Either a tile preview or the entire analyzed image
        /// </returns>
        private static async Task<(Bitmap, double)> ProcessImage(Bitmap bmp, bool forcePattern, Point desiredTile, string imageInfo = "", Feature desiredFeature = null, bool graphics = true)
        {
            ResizeAndRotate(ref bmp);

            if (desiredTile == NullPoint) 
                Offset = await FollowPattern(bmp, forcePattern, imageInfo);
            else
            {
                Rectangle cropRect = Channel.Project.GetIndexedGeometry(desiredTile.X, desiredTile.Y, Offset);
                Bitmap crop = new Bitmap(cropRect.Width, cropRect.Height);
                await ProcessTile(desiredTile.X, desiredTile.Y, bmp, ref crop, cropRect, desiredFeature, graphics, imageInfo);
                await Channel.DebugStream.WriteAsync($"Get tile: {desiredTile} and feature: {(desiredFeature == null ? "null" : desiredFeature.Name)}");
                return (crop, 0.0);
            }

            string outputData = string.Empty;
            int pass = 0;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < Channel.Project.Columns; i++)
                {
                    for (int j = Channel.Project.Rows - 1; j >= 0; j--)
                    {
                        Rectangle cropRect = Channel.Project.GetIndexedGeometry(i, j, Offset);
                        Bitmap crop = new Bitmap(cropRect.Width, cropRect.Height);
                        outputData += await ProcessTile(i, j, bmp, ref crop, cropRect, desiredFeature, graphics, imageInfo);
                        if (outputData.EndsWith("True\n")) pass++;
                        g.DrawImage(crop, cropRect.X, cropRect.Y);
                    }   
                }
            }

            Channel.Viewer.UpdateImage(bmp);
            await Channel.DataStream.WriteAsync(outputData);
            return (bmp, pass / Channel.Project.GetNumTiles());
        }

        private static Task<string> ProcessTile(int i, int j, Bitmap bmp, ref Bitmap crop, Rectangle cropRect, Feature desiredFeature, bool graphics, string imageInfo)
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
                        feature.UpdateScore(score);
                        if (score == 0f) // Special pass
                        {
                            g.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.LawnGreen)), feature.GetGeometry());
                            if (desiredFeature != null && feature.Name == desiredFeature.Name) // Special pass selected
                                g.DrawRectangle(new Pen(Color.Black, Channel.Project.ScaledPixelsPerMicron), feature.GetGeometry());
                        }
                        else if (score == -10f) // Special fail
                        {
                            g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Firebrick)), feature.GetGeometry());
                            if (desiredFeature != null && feature.Name == desiredFeature.Name) // Special fail selected
                                g.DrawRectangle(new Pen(Color.Black, Channel.Project.ScaledPixelsPerMicron), feature.GetGeometry());
                        }
                        else // Normal
                        {
                            g.DrawRectangle(new Pen(feature.ColorFromScore(), Channel.Project.ScaledPixelsPerMicron), feature.GetGeometry());
                            if (desiredFeature != null && feature.Name == desiredFeature.Name) // Normal Selected
                                g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Gold)), feature.GetGeometry());
                        }
                        if (desiredFeature == null) outputData += $"{imageInfo}{Channel.Project.Rows - j}\t{i + 1}\t{feature.Name}\t{score}\t{feature.LastPass}\n";
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
            int filterVal = (int)(0.1 * (bmp.Width * bmp.Height));
            float entropy = CalculateShannonEntropy(rVals, rect.Size);
            float score = entropy > 0 ? (float)Math.Round(entropy + (whiteVals / 2), 3) : 0;

            if (blackVals < filterVal || whiteVals < filterVal)
            {
                switch (feature.NullDetection)
                {
                    case Feature.NullDetectionTypes.None:
                        return -10f;
                    case Feature.NullDetectionTypes.Include_Empty:
                        if (whiteVals < filterVal) return 0f;
                        else return -10f;
                    case Feature.NullDetectionTypes.Include_Filled:
                        if (blackVals < filterVal) return 0f;
                        else return -10f;
                    case Feature.NullDetectionTypes.Include_Both:
                        return 0f;
                    default:
                        return -10f;
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

        private static async Task<Point> FollowPattern(Bitmap bmp, bool forcePattern, string imageInfo)
        {
            if (Channel.Project.PatternIntervalValue != 0 && Channel.Pattern != null && DataStream.Header != null)
            {
                if (forcePattern)
                    return await FindPattern(bmp);
                else
                {
                    string[] cols = DataStream.Header.Split('\t');
                    for (int i = 0; i < cols.Length; i++)
                        if (!string.IsNullOrEmpty(cols[i]))
                            if (cols[i] == Channel.Project.PatternIntervalString)
                                if (int.TryParse(imageInfo.Split('\t')[i], out int matchVal))
                                    if (matchVal % Channel.Project.PatternIntervalValue == 0)
                                    {
                                        await Channel.DebugStream.WriteAsync($"Pattern follower interval hit: {imageInfo}", false);
                                        return await FindPattern(bmp);
                                    }
                }
            }
            return Offset;
        }

        private static async Task<Point> FindPattern(Bitmap bmp)
        {
            Bitmap sourceImage = (Bitmap)bmp.Clone();
            Bitmap pattern = (Bitmap)Channel.Pattern.Clone();
            var tm = new ExhaustiveTemplateMatching(Channel.Project.PatternScore);
            TemplateMatch[] matchings = tm.ProcessImage(sourceImage, pattern);
            if (matchings.Length > 0)
            {
                TemplateMatch m = matchings[0];
                foreach (Point point in Channel.Project.PatternLocations)
                {
                    Point delta = new Point(point.X - m.Rectangle.Center().X, point.Y - m.Rectangle.Center().Y);
                    double deltaH = Math.Sqrt(Math.Abs(Math.Pow(delta.X, 2) + Math.Pow(delta.Y, 2)));
                    if (deltaH <= Channel.Project.PatternDeltaMax)
                    {
                        await Channel.DebugStream.WriteAsync(
                            $"Pattern follower delta = {Math.Round(deltaH, 2):F2} px, " +
                            $"{Math.Round(deltaH / Channel.Project.ScaledPixelsPerMicron, 2):F2} µm", showInViewer: true);
                        return delta;
                    }
                }
                if (Channel.Project.PatternLocations.Count == 0)
                    await Channel.DebugStream.WriteAsync($"Pattern location not taught", showInViewer: true);
                else
                    await Channel.DebugStream.WriteAsync($"Failed to find valid pattern. Best score = {m.Similarity}", showInViewer: true);
            }
            else
                await Channel.DebugStream.WriteAsync($"Failed to find pattern.", showInViewer: true);
            return Offset;
        }

        private static (Bitmap, double) CustomProcessImage(Bitmap bmp)
        {
            int dotSize = 5;

            // Setup Image
            Accord.Imaging.Filters.Grayscale filter = new Accord.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721);
            bmp = filter.Apply(bmp);
            Accord.Imaging.Filters.Threshold threshold = new Accord.Imaging.Filters.Threshold(220);
            threshold.ApplyInPlace(bmp);

            // Lock Image
            BitmapData bitmapData = bmp.LockBits(ImageLockMode.ReadWrite);

            // Find Blobs (with some params - there are a lot more)
            BlobCounter blobCounter = new BlobCounter
            {
                FilterBlobs = true,
                MinHeight = 2,
                MinWidth = 2
            };
            blobCounter.ProcessImage(bitmapData);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            bmp.UnlockBits(bitmapData);

            // Draw Dots
            Bitmap overlay = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(overlay))
            {
                g.Clear(Color.Black);
                for (int i = 0; i < blobs.Length; i++)
                {
                    g.DrawEllipse(new Pen(Brushes.Red, dotSize / 2), new Rectangle(
                        (int)(blobs[i].CenterOfGravity.X - dotSize),
                        (int)(blobs[i].CenterOfGravity.Y - dotSize),
                        dotSize * 2, dotSize * 2));
                }
            }

            return (overlay, blobs.Length);
        }

        #region Composer Functions

        public static Bitmap DrawGrid(Bitmap bmp, int tileRow, int tileColumn)
        {
            bmp = new Bitmap(bmp.Width, bmp.Height);
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
            var result = await ProcessImage(bmp, false, new Point(tileColumn - 1, Channel.Project.Rows - tileRow), desiredFeature: feature, graphics: graphics);
            return result.Item1;
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
