using Accord.Imaging;
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
        /// <param name="stamp"></param>
        public static async Task<double> LoadImage(Bitmap bmp, bool forcePattern, string imageInfo, bool stamp = false)
        {
            Channel.Project.ImageHeight = bmp.Height;
            Channel.Project.ImageWidth = bmp.Width;
            if (stamp)
            {
                (Bitmap, Bitmap, double) stampResult = await ProcessStampImage(bmp, imageInfo);
                return stampResult.Item3;
            }
            else
            {
                (Bitmap, double) result = await ProcessImage(bmp, forcePattern, NullPoint, imageInfo);
                return result.Item2;
            }    
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
        /// Either a tile preview or the entire analyzed image and
        /// the percent null fail features within image as a tuple
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
                return (crop, 0.0);
            }

            string outputData = string.Empty;
            int nullFailTotal = 0;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < Channel.Project.Columns; i++)
                {
                    for (int j = Channel.Project.Rows - 1; j >= 0; j--)
                    {
                        Rectangle cropRect = Channel.Project.GetIndexedGeometry(i, j, Offset);
                        Bitmap crop = new Bitmap(cropRect.Width, cropRect.Height);
                        (string newData, int numNullFail) = await ProcessTile(i, j, bmp, ref crop, cropRect, desiredFeature, graphics, imageInfo);
                        outputData += newData;
                        nullFailTotal += numNullFail;
                        g.DrawImage(crop, cropRect.X, cropRect.Y);
                    }   
                }
            }

            Channel.Viewer.UpdateImage(bmp);
            await Channel.DataStream.WriteAsync(outputData, false);
            return (bmp, nullFailTotal / Channel.Project.GetNumTotalFeatures());
        }

        private static Task<(string, int)> ProcessTile(int i, int j, Bitmap bmp, ref Bitmap crop, Rectangle cropRect, Feature desiredFeature, bool graphics, string imageInfo)
        {
            string outputData = string.Empty;
            int numNullFail = 0;
            using (Graphics g = Graphics.FromImage(crop))
            {
                g.DrawImage(bmp, new Rectangle(Point.Empty, crop.Size), cropRect, GraphicsUnit.Pixel);
                if (graphics)
                {
                    Bitmap tile = (Bitmap)crop.Clone();
                    foreach (Feature feature in Channel.Project.Features)
                    {
                        (float score, string compression) = AnalyzeData(tile, feature, imageInfo);
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
                            numNullFail++;
                        }
                        else // Normal
                        {
                            g.DrawRectangle(new Pen(feature.ColorFromScore(), Channel.Project.ScaledPixelsPerMicron), feature.GetGeometry());
                            if (desiredFeature != null && feature.Name == desiredFeature.Name) // Normal Selected
                                g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Gold)), feature.GetGeometry());
                        }
                        bool pass = feature.LastPass;
                        if (desiredFeature == null) outputData += $"{imageInfo}{Channel.Project.Rows - j}\t{i + 1}\t{feature.Name}\t{score:f3}\t{pass}\t{compression}\n";
                    }
                }
            }
            return Task.FromResult((outputData, numNullFail));
        }

        private static (float, string) AnalyzeData(Bitmap bmpTile, Feature feature, string imageInfo)
        {
            Rectangle cropRect = feature.GetPaddedGeometry();
            Bitmap bmp = new Bitmap(cropRect.Width, cropRect.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
                g.DrawImage(bmpTile, new Rectangle(0, 0, cropRect.Width, cropRect.Height), cropRect, GraphicsUnit.Pixel);

            (byte[] rVals, byte[] rtVals) = GetPixelData(bmp, (byte)(feature.Threshold * 255));
            string compression = feature.SaveImage ? Compress(rVals) : "";
            if (rVals.Max() == 0)
            {
                Channel.DebugStream.Write($"NULL IMG {imageInfo}");
                return (-10f, compression);
            }
                
            int blackVals = rtVals.Where(x => x == 0).Count();
            int whiteVals = rtVals.Where(x => x == 1).Count();
            int filterVal = (int)(feature.NullFilterPercentage * (bmp.Width * bmp.Height));
            float entropy = CalculateShannonEntropy(rVals, cropRect.Size);
            float score = entropy > 0 ? (float)Math.Round(entropy + (whiteVals / 2), 3) : 0;

            if (blackVals < filterVal || whiteVals < filterVal)
            {
                switch (feature.NullDetection)
                {
                    case Feature.NullDetectionTypes.None:
                        return (-10f, compression);
                    case Feature.NullDetectionTypes.Include_Empty:
                        if (whiteVals < filterVal) return (0f, compression);
                        else return (-10f, compression);
                    case Feature.NullDetectionTypes.Include_Filled:
                        if (blackVals < filterVal) return (0f, compression);
                        else return (-10f, compression);
                    case Feature.NullDetectionTypes.Include_Both:
                        return (0f, compression);
                    default:
                        return (-10f, compression);
                }
            }
            else
                return (score, compression);
        }

        public static string Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Fastest))
                dstream.Write(data, 0, data.Length);
            return Convert.ToBase64String(output.ToArray());
        }

        private static (byte[], byte[]) GetPixelData(Bitmap bmp, float threshold)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int bytes = Math.Abs(bmpData.Width * 3) * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);

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
            return (rVals.ToArray(), rtVals.ToArray());
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
            if (Channel.Pattern != null && LogStream.Header != null)
            {
                if (forcePattern)
                    return await FindPattern(bmp);
                else if (Channel.Project.PatternIntervalValue != 0)
                {
                    string[] cols = LogStream.Header.Split('\t');
                    for (int i = 0; i < cols.Length; i++)
                        if (!string.IsNullOrEmpty(cols[i]))
                            if (cols[i] == Channel.Project.PatternIntervalString)
                                if (int.TryParse(imageInfo.Split('\t')[i], out int matchVal))
                                    if (matchVal % Channel.Project.PatternIntervalValue == 0)
                                    {
                                        await Channel.DebugStream.WriteAsync($"PR interval hit: {imageInfo}");
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

            try
            {
                var tm = new ExhaustiveTemplateMatching(Channel.Project.PatternScore);
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                TemplateMatch[] matchings = tm.ProcessImage(sourceImage, pattern);

                if (matchings.Length > 0)
                {
                    TemplateMatch m = matchings[0];
                    double smallestDelta = double.MaxValue;
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
                        if (deltaH < smallestDelta) smallestDelta = deltaH;
                    }
                    if (Channel.Project.PatternLocations.Count == 0)
                        await Channel.DebugStream.WriteAsync($"Pattern location not taught", showInViewer: true);
                    else
                        await Channel.DebugStream.WriteAsync($"Failed to find valid pattern. Best score = " +
                            $"{Math.Round(m.Similarity, 2)}, Smallest delta = {Math.Round(smallestDelta / Channel.Project.ScaledPixelsPerMicron, 2):F2} µm",
                            showInViewer: true);
                }
                else
                    await Channel.DebugStream.WriteAsync($"Failed to find pattern.", showInViewer: true);
                return Offset;
            }
            catch (Exception)
            {
                return Offset;
            }
        }

        #region Stamp Functions

        internal static double StampScaling;
        internal static int StampThreshold;
        internal static List<Rectangle> StampPosts = new List<Rectangle>();
        internal static List<Rectangle> StampMasks = new List<Rectangle>();

        internal static async Task<(Bitmap, Bitmap, double)> ProcessStampImage(Bitmap bmp, string imageInfo = "", bool setup = false)
        {
            // Setup Image
            Bitmap output = new Bitmap(bmp, new Size((int)(bmp.Width * StampScaling), (int)(bmp.Height * StampScaling)));
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            output = filter.Apply(output);
            Threshold threshold = new Threshold(StampThreshold);
            threshold.ApplyInPlace(output);
            SobelEdgeDetector sobel = new SobelEdgeDetector();
            sobel.ApplyInPlace(output);

            // Init Counters
            int postsPresent = 0;
            int debrisInPosts = 0;
            int debrisOnMesa = 0;

            // Process Image
            BitmapData bitmapData = output.LockBits(ImageLockMode.ReadWrite);
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.ProcessImage(bitmapData);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            output.UnlockBits(bitmapData);
            Bitmap overlay = new Bitmap(output.Width, output.Height);
            using (Graphics g = Graphics.FromImage(overlay))
            {
                foreach (Blob blob in blobs)
                {
                    bool drawn = false;
                    bool masked = false;
                    if (StampPosts.Count > 0)
                    {
                        foreach (Rectangle post in StampPosts)
                        {
                            if (PointDist(post.Center(), blob.Rectangle.Center()) < 75 * StampScaling &&
                                SizeDiff(post.Size, blob.Rectangle.Size) < 25 * StampScaling)
                            {
                                g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Green)), blob.Rectangle);
                                postsPresent++;
                                drawn = true;
                            }
                            else if (BlobInRegion(blob, post))
                                debrisInPosts += HighlightBlob(output, ref overlay, blob);
                        }          
                    }
                    if (StampMasks.Count > 0 && !drawn)
                    {
                        foreach (Rectangle mask in StampMasks)
                        {
                            if (!masked && BlobInRegion(blob, mask))
                                masked = true;
                        } 
                    }
                    if (!masked && !drawn)
                        debrisOnMesa += HighlightBlob(output, ref overlay, blob);
                }
            }

            if (!setup) Channel.Viewer.UpdateImage(output, overlay);
            if (!string.IsNullOrEmpty(imageInfo)) await Channel.StampStream.WriteAsync($"{imageInfo}{postsPresent}\t{debrisInPosts}\t{debrisOnMesa}");
            return (output, overlay, blobs.Length);
        }

        private static double PointDist(Point A, Point B)
        {
            return Math.Sqrt(Math.Pow(B.X - A.X, 2) + Math.Pow(B.Y - A.Y, 2));
        }

        private static double SizeDiff(Size A, Size B)
        {
            return (PointDist(new Point(B.Width - A.Width, B.Height - A.Height), Point.Empty));
        }

        private static int HighlightBlob(Bitmap a, ref Bitmap b, Blob blob)
        {
            int count = 0;
            for (int i = blob.Rectangle.Left; i < blob.Rectangle.Right; i++)
                for (int j = blob.Rectangle.Top; j < blob.Rectangle.Bottom; j++)
                    if (a.GetPixel(i, j) == Color.FromArgb(255, 255, 255, 255))
                    {
                        b.SetPixel(i, j, Color.Red);
                        count++;
                    }
            return count; // Almost equal to blob.Area
        }

        private static bool BlobInRegion(Blob blob, Rectangle rect)
        {
            return rect.Contains(new Point(blob.Rectangle.Left, blob.Rectangle.Top)) &&
                rect.Contains(new Point(blob.Rectangle.Right, blob.Rectangle.Bottom));
        }

        #endregion

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
                        int thisX = (int)(rectangle.X + (i * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchX) - Offset.X);
                        int thisY = (int)(rectangle.Y + (j * Channel.Project.ScaledPixelsPerMicron * Channel.Project.PitchY) - Offset.Y);
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
