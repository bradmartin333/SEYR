using SEYR.Properties;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEYR
{
    public static class Pipeline
    {
        // Information String Variables
        public static string RR { get; set; } = "0";
        public static string RC { get; set; } = "0";
        public static string R { get; set; } = "0";
        public static string C { get; set; } = "0";
        public static string X { get; set; } = "0";
        public static string Y { get; set; } = "0";

        /// <summary>
        /// Default is -1 (null)
        /// </summary>
        public static int ImageIdx { get; set; } = -1;

        /// <summary>
        /// Default is 1 (Every Image)
        /// </summary>
        public static int PatternFollowInterval { get; set; } = 1;

        /// <summary>
        /// Number of seconds allowed for patten follow processing
        /// </summary>
        public static int PatternFollowDelay { get; set; } = 100;

        /// <summary>
        /// Device X Pitch
        /// </summary>
        public static double PitchX { get; set; } = -1;

        /// <summary>
        /// Device Y Pitch
        /// </summary>
        public static double PitchY { get; set; } = -1;

        /// <summary>
        /// Scale of incoming image
        /// </summary>
        public static double ImageScale { get; set; } = 1.00;

        /// <summary>
        /// Image is processing
        /// </summary>
        public static bool Working { get; set; } = false;

        /// <summary>
        /// Has been given at least 1 image
        /// </summary>
        public static bool HasImage { get; set; } = false;

        /// <summary>
        /// Gets updated each pattern search
        /// </summary>
        public static bool FoundPattern { get; set; } = false;

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

        public static void Initialize(int patternFollowInterval = 1, double pitchX = -1, double pitchY = -1, int patternFollowDelay = 100, double imageScale = 1.00)
        { 
            Composer = new Composer();
            Viewer = new Viewer();

            PatternFollowInterval = patternFollowInterval;
            PitchX = pitchX;
            PitchY = pitchY;
            PatternFollowDelay = patternFollowDelay;
            ImageScale = imageScale;
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
            Working = true;
            using (var wc = new WaitCursor())
            {
                Bitmap clone = (Bitmap)img.Clone();
                Imaging.ApplyFilters(clone);
                clone.Dispose();

                // If it is time to look for pattern,
                // make sure the pattern is valid for the current image and SEYR file
                if (ImageIdx % PatternFollowInterval == 0 && 
                    !FileHandler.Grid.PatternFeature.Rectangle.IsEmpty && 
                    FileHandler.Grid.PatternFeature.Rectangle.Width < Picasso.IncomingSize.Width && 
                    FileHandler.Grid.PatternFeature.Rectangle.Height < Picasso.IncomingSize.Height)
                {
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer()
                    {
                        Interval = (int)(PatternFollowDelay * 1e3),
                        Enabled = true
                    };
                    timer.Tick += Timer_Tick;
                    timer.Start();

                    PatternFollowThread = new Thread(delegate () { FoundPattern = Imaging.FollowPattern(); });
                    PatternFollowThread.Start();
                    while (PatternFollowThread.IsAlive)
                        Application.DoEvents();
                    timer.Dispose();
                }

                MakeTiles();

                Bitmap background = (Bitmap)PBX.BackgroundImage.Clone();
                Bitmap foreground = (Bitmap)PBX.Image.Clone();
                Viewer.InsertNewImage(background, foreground);
            }
            HasImage = true;
            Working = false;
        }

        private static void Timer_Tick(object sender, System.EventArgs e)
        {
            PatternFollowThread.Abort();
            FoundPattern = false;
        }

        #endregion
    }
}
