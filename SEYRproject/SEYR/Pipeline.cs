using Accord.Imaging.Filters;
using SEYR.Properties;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

namespace SEYR
{
    public static class Pipeline
    {
        // Information String Variables
        public static string RR = "0";
        public static string RC = "0";
        public static string R = "0";
        public static string C = "0";
        public static string X = "0";
        public static string Y = "0";

        /// <summary>
        /// Default is -1 (null)
        /// </summary>
        public static int ImageIdx { get; set; } = -1;

        /// <summary>
        /// Default is 1 (Every Image)
        /// </summary>
        public static int PatternFollowInterval { get; set; } = 1;

        /// <summary>
        /// Device X Pitch
        /// </summary>
        public static double PitchX { get; set; } = -1;

        /// <summary>
        /// Device Y Pitch
        /// </summary>
        public static double PitchY { get; set; } = -1;

        /// <summary>
        /// Number of seconds allowed for patten follow processing
        /// </summary>
        public static int PatternFollowDelay = 100;

        /// <summary>
        /// Scale of incoming image
        /// </summary>
        public static double ImageScale = 0.25;

        /// <summary>
        /// Image is processing
        /// </summary>
        public static bool Working = false;

        /// <summary>
        /// Has been given at least 1 image
        /// </summary>
        public static bool HasImage = false;

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
        private static Thread PatternFollowThread;

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
                for (int i = DataHandler.Output.Count; i < ImageIdx; i++)
                {
                    DataHandler.Output.Add(string.Empty);
                }
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

        public static void LoadNewImage(Bitmap img)
        {
            if (Application.UseWaitCursor) return;
            using (var wc = new WaitCursor())
            {
                Picasso.IncomingSize = img.Size;
                Picasso.ClearGraphics();
                Imaging.OriginalImage = (Bitmap)img.Clone(); // Save unedited photo

                // Resize incoming image
                Bitmap resize = new Bitmap((int)(ImageScale * img.Width), (int)(ImageScale * img.Height));
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
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer()
                    {
                        Interval = (int)(PatternFollowDelay * 1e3),
                        Enabled = true
                    };
                    timer.Tick += Timer_Tick;
                    timer.Start();

                    bool foundPattern = false;
                    PatternFollowThread = new Thread(delegate () { foundPattern = Imaging.FollowPattern(); });
                    PatternFollowThread.Start();

                    while (PatternFollowThread.IsAlive)
                        Application.DoEvents();
                    timer.Dispose();

                    if (foundPattern)
                        Composer.followerPatternToolStripMenuItem.BackColor = SystemColors.Control;
                    else
                        Composer.followerPatternToolStripMenuItem.BackColor = Color.MistyRose;
                }

                MakeTiles();
                Viewer.InsertNewImage(PBX);
            }
            HasImage = true;
        }

        private static void Timer_Tick(object sender, System.EventArgs e)
        {
            PatternFollowThread.Abort();
        }

        #endregion
    }
}
