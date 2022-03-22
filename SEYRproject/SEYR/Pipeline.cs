using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SEYR
{
    public static class Pipeline
    {
        public static bool Initialized { get; set; } = false;
        public static bool ImageReady { get; set; } = false;

        /// <summary>
        /// Image is processing
        /// </summary>
        public static bool Working { get; set; } = false;

        /// <summary>
        /// Default is -1 (null)
        /// </summary>
        public static int ImageIdx { get; set; } = 1;

        // Information String Variables
        public static int RR { get; set; } = 0;
        public static int RC { get; set; } = 0;
        public static int R { get; set; } = 0;
        public static int C { get; set; } = 0;
        public static double X { get; set; } = -1;
        public static double Y { get; set; } = -1;

        /// <summary>
        /// Default is 1 (Every Image)
        /// </summary>
        public static int PatternFollowInterval { get; set; } = 1;

        /// <summary>
        /// Number of seconds allowed for patten follow processing
        /// </summary>
        public static int PatternFollowDelay { get; set; } = 100;

        /// <summary>
        /// Gets updated each pattern search
        /// </summary>
        public static bool FoundPattern { get; set; } = false;

        public static Composer Composer;
        public static Viewer Viewer;
        public static PixelPitch PixelPitch;
        private static Thread PatternFollowThread;

        /// <summary>
        /// Create the forms necessary for SEYR
        /// </summary>
        /// <param name="patternFollowInterval">
        /// How many images until the next pattern alignment
        /// </param>
        /// <param name="patternFollowDelay">
        /// How many seconds to allow for pattern alignment
        /// </param>
        /// <param name="logStreamerPath">
        /// If provided a valid directory, output will be streamed
        /// to a SEYR_report_ddMMMyyyy_secondsOfDay.txt file
        /// instead of requiring export at the end of a job
        /// </param>
        public static void Initialize(int patternFollowInterval = 1, int patternFollowDelay = 100, string logStreamerPath = null)
        { 
            Composer = new Composer();
            Viewer = new Viewer();
            PixelPitch = new PixelPitch();

            PatternFollowInterval = patternFollowInterval;
            PatternFollowDelay = patternFollowDelay;

            if (!string.IsNullOrEmpty(logStreamerPath))
            {
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(logStreamerPath);
                if (directory.Exists) DataHandler.StreamPath = directory.FullName + $@"\SEYR_report_{System.DateTime.Today.ToString("ddMMMyyyy",System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))}_{System.Math.Round(System.DateTime.Now.Subtract(System.DateTime.Today).TotalSeconds)}.txt";
            }
                
            Initialized = true;
        }

        public static void LoadNewImage(Bitmap img)
        {
            if (!Initialized) return;
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
                    PatternFollowThread = new Thread(delegate () { FoundPattern = Imaging.FollowPattern(); });
                    PatternFollowThread.Start();
                    while (PatternFollowThread.IsAlive)
                        Application.DoEvents();
                }

                FileHandler.Grid.MakeTiles();
            }

            ImageReady = true;
        }

        /// <summary>
        /// Returns lines containing all the data from the last image
        /// </summary>
        /// <returns>
        /// ImageNum, CopyX, CopyY, FeatureName, State, RR, RC, R, C, X, Y, Score
        /// </returns>
        public static string GetData()
        {
            return DataHandler.LastData;
        }

        public static void ClearOutput(bool reloadImage = false)
        {
            if (!Initialized) return;
            DataHandler.Output.Clear();
            Viewer.Clear();
            ImageIdx = 1;
            if (DataHandler.StreamPath != null) System.IO.File.Delete(DataHandler.StreamPath);
            if (reloadImage) LoadNewImage(Imaging.OriginalImage);
        }

        public static void UpdateFrames()
        {
            if (!Initialized) return;
            Bitmap background = (Bitmap)Imaging.DisplayedImage.Clone();
            Bitmap foreground = (Bitmap)Picasso.Painting.Clone();
            Composer.InsertNewImage(background, foreground);
            background = (Bitmap)Imaging.DisplayedImage.Clone();
            foreground = (Bitmap)Picasso.Painting.Clone();
            Viewer.InsertNewImage(background, foreground);
            Working = false;
        }

        /// <summary>
        /// Show and Bring to Front.
        /// Initialize if needed.
        /// </summary>
        public static void Appear()
        {
            if (!Initialized) Initialize();
            Composer.Show();
            Composer.BringToFront();
        }

        private static void Timer_Tick(object sender, System.EventArgs e)
        {
            PatternFollowThread.Abort();
            FoundPattern = false;
        }
    }
}
