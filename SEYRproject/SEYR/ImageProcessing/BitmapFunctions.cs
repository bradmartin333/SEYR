using Accord.Imaging.Filters;
using SEYR.Session;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SEYR.ImageProcessing
{
    internal static class BitmapFunctions
    {
        public static string LoadImage(Bitmap bmp)
        {
            try
            {
                Bitmap test = bmp.Clone(new Rectangle(1, 1, 1, 1), bmp.PixelFormat);
                return test.Size.ToString();
            }
            catch (System.Exception)
            {
                return "Failed to process image";
            }
        }

        public static Bitmap ApplyFilters(Bitmap bmp)
        {
            // Resize incoming image
            Bitmap resize = new Bitmap((int)(Channel.Project.Scaling * bmp.Width), (int)(Channel.Project.Scaling * bmp.Height));
            using (Graphics g = Graphics.FromImage(resize))
                g.DrawImage(bmp, 0, 0, resize.Width, resize.Height);
            Picasso.ThisSize = resize.Size;

            // Clone with necessary pixel format for image filtering
            Bitmap working = resize.Clone(new Rectangle(new Point(0, 0), resize.Size), PixelFormat.Format32bppArgb);
            working = RotateImage(working, Channel.Project.Angle);
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            working = filter.Apply(working);
            Threshold threshold = new Threshold(Channel.Project.Threshold);
            threshold.ApplyInPlace(working);
            return working;
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
