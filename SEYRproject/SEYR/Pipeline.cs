﻿using Accord.Imaging.Filters;
using SEYR.Properties;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEYR
{
    public static class Pipeline
    {
        /// <summary>
        /// RR, RC, R, C, X, Y
        /// </summary>
        public static string InformationString { get; set; } = string.Empty;

        /// <summary>
        /// Default is -1 (null)
        /// </summary>
        public static int ImageIdx { get; set; } = -1;

        /// <summary>
        /// Default is 1 (Every Image)
        /// </summary>
        public static int PatternFollowInterval { get; set; } = 1;

        public static PictureBox PBX = new PictureBox()
        {
            BackgroundImage = Resources.SEYR,
            BackgroundImageLayout = ImageLayout.Zoom,
            Dock = DockStyle.Fill,
            Image = Resources.SEYR,
            SizeMode = PictureBoxSizeMode.Zoom
        };

        public static Composer Composer;
        public static Viewer Viewer;

        public static void Initialize()
        {
            Composer = new Composer();
            Viewer = new Viewer();
        }

        #region Core Functions

        public static void MakeTiles()
        {
            if (ImageIdx == -1) return;

            if (ImageIdx > DataHandler.Output.Count)
                DataHandler.Output.Add(string.Empty);
            else
                DataHandler.Output[ImageIdx - 1] = string.Empty;

            // Each tile contains one of each feature
            // The top-left tile is index 0,0
            // The bottom-right tile is index i,j
            FileHandler.Grid.Tiles.Clear();
            for (int i = 0; i < FileHandler.Grid.NumberX + 1; i++)
            {
                for (int j = 0; j < FileHandler.Grid.NumberY + 1; j++)
                {
                    Tile tile = new Tile(i, j);
                    foreach (Feature feature in FileHandler.Grid.Features)
                    {
                        Feature copy = feature.Clone();

                        copy.Rectangle = new Rectangle(
                            (int)(feature.Rectangle.X + i * FileHandler.Grid.PitchX),
                            (int)(feature.Rectangle.Y + j * FileHandler.Grid.PitchY),
                            feature.Rectangle.Width, feature.Rectangle.Height);

                        copy.Index = new Point(i, j);

                        tile.Features.Add(copy);
                    }
                    FileHandler.Grid.Tiles.Add(tile);
                }
            }

            foreach (Tile tile in FileHandler.Grid.Tiles)
                tile.Score(ImageIdx);
            Picasso.ReDraw();
        }

        public static async Task<bool> LoadNewImage(Bitmap img, bool setup = false)
        {
            using (var wc = new WaitCursor())
            {
                Picasso.IncomingSize = img.Size;
                Picasso.ClearGraphics();
                Imaging.OriginalImage = (Bitmap)img.Clone(); // Save unedited photo

                // Resize incoming image
                double heightRatio = Picasso.BaseHeight / img.Height;
                Bitmap resize = new Bitmap((int)(heightRatio * img.Width), (int)Picasso.BaseHeight);
                using (Graphics g = Graphics.FromImage(resize))
                {
                    g.DrawImage(img, 0, 0, resize.Width, resize.Height);
                }
                img.Dispose();

                // Clone with necessary pixel format for image filtering
                Bitmap working = resize.Clone(new Rectangle(new Point(0, 0), resize.Size), PixelFormat.Format32bppArgb);
                resize.Dispose();

                working = Imaging.RotateImage(working, (float)FileHandler.Grid.Angle);
                Imaging.DisplayedImage = working;

                PBX.BackgroundImage = working;

                Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
                working = filter.Apply(working);

                Threshold threshold = new Threshold(FileHandler.Grid.FilterThreshold);
                threshold.ApplyInPlace(working);

                Imaging.CurrentImage = working; // Save edited photo

                if (ImageIdx % PatternFollowInterval == 0 && !FileHandler.Grid.PatternFeature.Rectangle.IsEmpty)
                {
                    Bitmap clone = (Bitmap)Imaging.CurrentImage.Clone();
                    for (int i = 0; i < 3; i++)
                    {
                        bool foundPattern = await Imaging.FollowPattern(clone);
                        if (foundPattern)
                        {
                            break;
                        }
                    }
                }

                MakeTiles();
                Viewer.InsertNewImage(PBX);
            }
            return true; // Need a return type for non-awaited situations
        }

        #endregion 
    }
}
