using System.Drawing;
using System.Threading;
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
        /// Gets updated each pattern search
        /// </summary>
        public static bool FoundPattern { get; set; } = false;

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

        public static void LoadNewImage(Bitmap img)
        {
            Working = true;
            using (var wc = new WaitCursor())
            {
                Bitmap clone = (Bitmap)img.Clone();
                Imaging.ApplyFilters(clone);

                // If it is time to look for pattern,
                // make sure the pattern is valid for the current image and SEYR file
                if (ImageIdx % PatternFollowInterval == 0 && 
                    !FileHandler.Grid.PatternFeature.Rectangle.IsEmpty && 
                    FileHandler.Grid.PatternFeature.Rectangle.Width < Picasso.ThisSize.Width && 
                    FileHandler.Grid.PatternFeature.Rectangle.Height < Picasso.ThisSize.Height)
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

                FileHandler.Grid.MakeTiles();
            }
        }

        public static void UpdateFrames()
        {
            Bitmap background = (Bitmap)Imaging.DisplayedImage.Clone();
            Bitmap foreground = (Bitmap)Picasso.Painting.Clone();
            Composer.InsertNewImage(background, foreground);
            background = (Bitmap)Imaging.DisplayedImage.Clone();
            foreground = (Bitmap)Picasso.Painting.Clone();
            Viewer.InsertNewImage(background, foreground);
            Working = false;
        }

        private static void Timer_Tick(object sender, System.EventArgs e)
        {
            PatternFollowThread.Abort();
            FoundPattern = false;
        }
    }
}
