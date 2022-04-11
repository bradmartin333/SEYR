using System.Drawing;

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
    }
}
