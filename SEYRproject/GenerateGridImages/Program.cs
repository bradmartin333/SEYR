﻿using System;
using System.Drawing;
using System.IO;

namespace GridImages
{
    public class Program
    {
        static void Main(string[] args)
        {
            OutputImagesRaw();
            OutputImagesRC();
        }

        public static void OutputImagesRaw()
        {
            string dir = "OutputImagesRaw";
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
            Bitmap cell = new Bitmap("cell.png");
            Bitmap back = new Bitmap("back.png");
            Random random = new Random();
            int idx = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Bitmap bmp = new Bitmap(1100, 1100);
                    int deltaX = random.Next(10);
                    int deltaY = random.Next(10);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            for (int l = 0; l < 10; l++)
                            {
                                if (k % 2 == 1 && l % 2 == 1)
                                {
                                    g.DrawImage(random.Next(2) == 1 ? cell : back, new Rectangle(k * 100 + deltaX, l * 100 + deltaY, 100, 100));
                                }
                            }
                        }
                    }
                    ImageConverter converter = new ImageConverter();
                    ImageMagick.MagickImage magickImage = new ImageMagick.MagickImage((byte[])converter.ConvertTo(bmp, typeof(byte[])));
                    magickImage.Rotate(2);
                    Console.WriteLine(string.Format("raw{0}\tdX: {1} dY: {2}", idx, deltaX, deltaY));
                    magickImage.Write(new FileInfo(string.Format(@"{0}\raw{1}.png", dir, idx)));
                    idx++;
                }
            }
        }

        public static void OutputImagesRC()
        {
            string dir = "OutputImages";
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
            Bitmap cell = new Bitmap("cell.png");
            Bitmap back = new Bitmap("back.png");
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Bitmap bmp = new Bitmap(1100, 1100);
                    int deltaX = random.Next(10);
                    int deltaY = random.Next(10);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            for (int l = 0; l < 10; l++)
                            {
                                if (k % 2 == 1 && l % 2 == 1)
                                {
                                    g.DrawImage(random.Next(2) == 1 ? cell : back, new Rectangle(k * 100 + deltaX, l * 100 + deltaY, 100, 100));
                                }
                            }
                        }
                    }
                    ImageConverter converter = new ImageConverter();
                    ImageMagick.MagickImage magickImage = new ImageMagick.MagickImage((byte[])converter.ConvertTo(bmp, typeof(byte[])));
                    magickImage.Rotate(2);
                    Console.WriteLine(string.Format("_R{0}_C{1}\tdX: {2} dY: {3}", i, j, deltaX, deltaY));
                    magickImage.Write(new FileInfo(string.Format(@"{0}\_R{1}_C{2}.png", dir, i, j)));
                }
            }
        }
    }
}