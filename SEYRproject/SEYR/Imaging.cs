using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using Accord.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using static SEYR.Pipeline;
using System.Drawing.Imaging;
using Accord.Imaging.Filters;
using Accord.Imaging.Moments;

namespace SEYR
{
    static class Imaging
    {
        public static Bitmap OriginalImage = new Bitmap(1, 1);
        public static Bitmap DisplayedImage = new Bitmap(1, 1);
        public static Bitmap CurrentImage = new Bitmap(1, 1);

        public static void ApplyFilters(Bitmap img)
        {
            OriginalImage = (Bitmap)img.Clone(); // Save unedited photo

            // Resize incoming image
            Bitmap resize = new Bitmap((int)(ImageScale * img.Width), (int)(ImageScale * img.Height));
            using (Graphics g = Graphics.FromImage(resize))
            {
                g.DrawImage(img, 0, 0, resize.Width, resize.Height);
            }
            Picasso.ThisSize = resize.Size;

            // Clone with necessary pixel format for image filtering
            Bitmap working = resize.Clone(new Rectangle(new Point(0, 0), resize.Size), PixelFormat.Format32bppArgb);
            working = RotateImage(working, (float)FileHandler.Grid.Angle);
            DisplayedImage = working;

            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            working = filter.Apply(working);

            Threshold threshold = new Threshold(FileHandler.Grid.FilterThreshold);
            threshold.ApplyInPlace(working);

            CurrentImage = working; // Save edited photo
        }

        private static double Scan(Bitmap colorImg, Bitmap filteredImg)
        {
            List<double> pixelVals = new List<double>();
            int whitePixels = 0;
            int blackPixels = 0;
            for (int i = 0; i < filteredImg.Width; i++)
            {
                for (int j = 0; j < filteredImg.Height; j++)
                {
                    Color color = colorImg.GetPixel(i, j);
                    pixelVals.Add(color.R);

                    Color binary = filteredImg.GetPixel(i, j);
                    if (binary.R == 255) whitePixels++;
                    if (binary.R == 0) blackPixels++;
                }
            }

            if (whitePixels < 10 || blackPixels < 10)
                return 0.0;
            else
                return Math.Round(Statistics.Entropy(pixelVals.ToArray()), 1) + whitePixels / 2;
            // Add a fraction of the number of white pixels to simulate a rotation filter
        }

        /// <summary>
        /// Score an individial feature based on the Current Image
        /// </summary>
        /// <param name="feature"></param>
        public static void Score(Feature feature)
        {
            if (feature.Rectangle.IsEmpty) return;
            Crop crop = new Crop(feature.OffsetRectangle);
            Bitmap colorImg = crop.Apply(DisplayedImage);
            Bitmap filteredImg = crop.Apply(CurrentImage);
            feature.Score = Scan(colorImg, filteredImg);
            if (Math.Abs(feature.Score - feature.PassScore) <= feature.PassTol)
            {
                if (feature.CheckAlign)
                {
                    try
                    {
                        RawMoments rawMoments = new RawMoments(filteredImg);
                        int diff = (int)Math.Abs(MathNet.Numerics.Distance.Euclidean(
                            new double[] { rawMoments.CenterY, rawMoments.CenterY },
                            new double[] { feature.WeightedCenter.X, feature.WeightedCenter.Y }));
                        if (diff > feature.AlignTol)
                            feature.State = DataHandler.State.Misaligned;
                        else
                            feature.State = DataHandler.State.Pass;
                    }
                    catch (Exception)
                    {
                        feature.State = DataHandler.State.Fail;
                    }
                }
                else
                    feature.State = DataHandler.State.Pass;
            } 
            else if (Math.Abs(feature.Score - feature.FailScore) <= feature.FailTol)
                feature.State = DataHandler.State.Fail;
            else
                feature.State = DataHandler.State.Null;
        }

        /// <summary>
        /// Find the XY offset of the Current Image from the training image
        /// </summary>
        public static bool FollowPattern()
        {
            if (FileHandler.Grid.PatternFeature.Rectangle.IsEmpty) return false;
            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.9f);

            Bitmap source = (Bitmap)CurrentImage.Clone();
            if (FileHandler.Grid.PatternBitmap.Size.Width > source.Width || FileHandler.Grid.PatternBitmap.Size.Height > source.Height) return false;
            TemplateMatch[] matchings = tm.ProcessImage(source, FileHandler.Grid.PatternBitmap);
            if (matchings.Length == 0) return false;

            Point point = matchings[0].Rectangle.Location;
            Feature matchFeature = null;
            foreach (Tile tile in FileHandler.Grid.Tiles)
            {
                foreach (Feature feature in tile.Features)
                {
                    double distance = MathNet.Numerics.Distance.Euclidean(
                        new double[] { point.X, point.Y },
                        new double[] { feature.OriginX, feature.OriginY });

                    // Restrict matching radius to a fraction of the tile pitch
                    if (distance < Math.Min(FileHandler.Grid.PitchX, FileHandler.Grid.PitchY) * 0.75)
                    {
                        matchFeature = feature;
                        break;
                    }
                }
                if (matchFeature != null) break;
            }
            if (matchFeature != null)
            {
                Picasso.Offset = new Point(matchFeature.OriginX - point.X, matchFeature.OriginY - point.Y);
                return true;
            }
            return false;
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
        public static Bitmap RotateImage(Bitmap img, float rotationAngle)
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
