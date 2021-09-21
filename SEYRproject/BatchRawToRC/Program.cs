using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace BatchRawToRC
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please execute program in as a command with specified directory");
                Console.WriteLine(@"> ./BatchRawToRC.exe C:\Users\delta\Documents");
                Console.ReadKey();
                return;
            }

            string[] images = GetImagesFrom(args[0]);
            if (images.Length == 0)
            {
                Console.WriteLine("No images found. Exiting..."); 
                return;
            }
            Console.WriteLine("\nConvert Sqaure Batch Raw .bmp Images to _RX_CX.png Images");

            Console.Write("Enter number of images per batch: ");
            int numPerBatch = int.Parse(Console.ReadLine());
            if (numPerBatch == 0)
            {
                Console.WriteLine("Value of 0. Exiting..."); 
                return;
            }

            string execString = string.Format("\nPress Enter to convert {0} image batches of raw .bmp images to _RX_CX.png format", images.Length / numPerBatch);
            Console.WriteLine(execString);
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Console.WriteLine(execString);
            }
            Console.WriteLine("Conversion Started");

            int batchNum = 0;
            int row = 0;
            int col = 0;
            if (Directory.Exists("Output"))
            {
                Directory.Delete("Output", true);
                Console.WriteLine("Deleted old Output directory");
            }
            Directory.CreateDirectory("Output");
            Console.WriteLine("Create new Output directory\n");
            for (int i = 0; i < images.Length; i++)
            {
                if (i % numPerBatch == 0)
                {
                    batchNum++;
                    Directory.CreateDirectory(string.Format(@"Output\{0}", batchNum));
                    Console.WriteLine(string.Format("Converting Batch {0}", batchNum));
                }
                if (col == Math.Sqrt(numPerBatch))
                {
                    row++;
                    col = 0;
                }
                Bitmap bitmap = new Bitmap(images[i]);
                bitmap.Save(string.Format(@"Output\{0}\_R{1}_C{2}.png", batchNum, row, col));
                col++;
            }

            Console.WriteLine("\nConversion Complete\n");
        }

        static string[] GetImagesFrom(string searchFolder)
        {
            List<string> filesFound = new List<string>();
            SearchOption searchOption = SearchOption.AllDirectories;
            string[] filters = new string[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, string.Format("*.{0}", filter), searchOption));
            }
            string[] fileArr = filesFound.ToArray();
            NumericComparer ns = new NumericComparer();
            Array.Sort(fileArr, ns);
            return fileArr;
        }
    }
}
