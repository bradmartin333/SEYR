using System;
using System.Drawing;
using System.IO;

namespace GenerateImages
{
    public class Program
    {
        static void Main(string[] args)
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
                        g.FillRectangle(Brushes.White, new Rectangle(0, 0, 1100, 1100));
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
                    bmp.Save(string.Format(@"{0}\raw{1}.png", dir, idx));
                    Console.WriteLine(string.Format("raw{0}\tdX: {1} dY: {2}", idx, deltaX, deltaY));
                    idx++;
                }
            }
        }
    }
}
