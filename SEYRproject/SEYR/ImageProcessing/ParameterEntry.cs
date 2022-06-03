using Accord.Imaging;
using Accord.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEYR.ImageProcessing
{
    public partial class ParameterEntry : Form
    {
        private Bitmap Train = null;
        private readonly double DefaultScaling = 0.5;
        private readonly int DefaultThreshold = 170;
        private bool LOADING = false;

        public ParameterEntry(Bitmap bmp)
        {
            InitializeComponent();
            NumScaling.Value = (decimal)DefaultScaling;
            TrackbarThreshold.Value = DefaultThreshold;
            Train = bmp;
            ApplyFilters();
            PBX.MouseDown += PBX_MouseDown;
            PBX.MouseMove += PBX_MouseMove;
            PBX.MouseLeave += PBX_MouseLeave;
        }

        private void BtnAutoSettings_Click(object sender, EventArgs e)
        {
            LOADING = true;
            NumScaling.Value = (decimal)DefaultScaling;
            TrackbarThreshold.Value = DefaultThreshold;
            LOADING = false;
            ApplyFilters();
        }

        private void BtnClearMasks_Click(object sender, EventArgs e)
        {
            Masks.Clear();
            UpdateOverlay();
        }

        private void BtnSaveAndContinue_Click(object sender, EventArgs e)
        {

        }

        private void ApplyFilters()
        {
            // Setup Image
            Bitmap output = new Bitmap(Train, 
                new Size((int)(Train.Width * (double)NumScaling.Value), (int)(Train.Height * (double)NumScaling.Value)));
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            output = filter.Apply(output);
            Threshold threshold = new Threshold(TrackbarThreshold.Value);
            threshold.ApplyInPlace(output);
            SobelEdgeDetector sobel = new SobelEdgeDetector();
            sobel.ApplyInPlace(output);

            // Lock Image
            BitmapData bitmapData = output.LockBits(ImageLockMode.ReadWrite);

            // Find Blobs (with some params - there are a lot more)
            BlobCounter blobCounter = new BlobCounter
            {

            };
            blobCounter.ProcessImage(bitmapData);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            output.UnlockBits(bitmapData);

            Bitmap overlay = new Bitmap(output.Width, output.Height);
            using (Graphics g = Graphics.FromImage(overlay))
            {
                for (int i = 0; i < blobs.Length; i++)
                {
                    Rectangle blobRect = blobs[i].Rectangle;
                    double postTol = 50 * (double)NumScaling.Value;
                    if (PointDist(Post.Center(), blobRect.Center()) < postTol && SizeDiff(Post.Size, blobRect.Size) < postTol)
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Green)), blobs[i].Rectangle);
                    }
                    else if (Masks.Count > 0)
                    {
                        foreach (Rectangle mask in Masks)
                        {
                            if (!blobRect.IntersectsWith(mask))
                            {
                                g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Red)), blobs[i].Rectangle);
                            }
                        }
                    }
                    else
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Red)), blobs[i].Rectangle);
                    }
                }
            }

            PBX.BackgroundImage = output;
            PBX.Image = overlay;
        }

        private void NumScaling_ValueChanged(object sender, EventArgs e)
        {
            if (!LOADING) ApplyFilters();
        }

        private void TrackbarThreshold_Scroll(object sender, EventArgs e)
        {
            if (!LOADING) ApplyFilters();
        }

        private void NumMinWid_ValueChanged(object sender, EventArgs e)
        {
            if (!LOADING) ApplyFilters();
        }

        private void NumMinHgt_ValueChanged(object sender, EventArgs e)
        {
            if (!LOADING) ApplyFilters();
        }

        private void NumMaxWid_ValueChanged(object sender, EventArgs e)
        {
            if (!LOADING) ApplyFilters();
        }

        private void NumMaxHgt_ValueChanged(object sender, EventArgs e)
        {
            if (!LOADING) ApplyFilters();
        }

        #region Crop

        private bool DrawingPost = false;
        private bool DrawingMask = false;
        private Point StartPoint = Point.Empty;
        private Point EndPoint = Point.Empty;
        private List<Rectangle> Masks = new List<Rectangle>();
        private Rectangle Post = new Rectangle();

        private void PBX_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (!DrawingMask) DrawingPost = !DrawingPost;
                    else
                    {
                        StopDrawing();
                        return;
                    }
                    break;
                case MouseButtons.Right:
                    if (!DrawingPost) DrawingMask = !DrawingMask;
                    else
                    {
                        StopDrawing();
                        return;
                    }
                    break;
                default:
                    return;
            }
            
            if (DrawingPost || DrawingMask)
            {
                StartPoint = ZoomMousePos(e.Location);
                EndPoint = StartPoint;
                PBX.Image = null;
            }

            UpdateOverlay(e.Button);
        }

        private void PBX_MouseMove(object sender, MouseEventArgs e)
        {
            if (DrawingPost || DrawingMask) EndPoint = ZoomMousePos(e.Location);
            UpdateOverlay();
        }

        private void PBX_MouseLeave(object sender, EventArgs e)
        {
            if (!LOADING) ApplyFilters();
        }

        private void StopDrawing()
        {
            DrawingPost = false;           
            DrawingMask = false;
            StartPoint = Point.Empty;
            EndPoint = Point.Empty;
            UpdateOverlay();
        }

        private void UpdateOverlay(MouseButtons btn = MouseButtons.None)
        {
            Bitmap bmp = new Bitmap(PBX.BackgroundImage.Width, PBX.BackgroundImage.Height);

            Rectangle rect = new Rectangle(
                Math.Min(StartPoint.X, EndPoint.X),
                Math.Min(StartPoint.Y, EndPoint.Y),
                Math.Abs(EndPoint.X - StartPoint.X),
                Math.Abs(EndPoint.Y - StartPoint.Y));

            if (btn == MouseButtons.Left || DrawingPost) Post = rect;
            if (btn == MouseButtons.Right && !DrawingMask) Masks.Add(rect);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawRectangle(new Pen(Brushes.HotPink, (float)(Math.Min(bmp.Height, bmp.Width) * 0.005)), Post);
                if (DrawingMask) g.FillRectangle(Brushes.LawnGreen, rect);
                foreach (Rectangle mask in Masks)
                    g.FillRectangle(Brushes.LawnGreen, mask);
            }

            PBX.Image = bmp;
        }

        /// <summary>
        /// Method for adjusting mouse pos to pictureBox set to Zoom
        /// </summary>
        /// <param name="click">
        /// Mouse coordinates
        /// </param>
        /// <returns>
        /// Pixel coordinates
        /// </returns>
        private Point ZoomMousePos(Point click)
        {
            Size pbxSize = PBX.Size;
            Size imgSize = PBX.BackgroundImage.Size;
            float ImageAspect = imgSize.Width / (float)imgSize.Height;
            float controlAspect = pbxSize.Width / (float)pbxSize.Height;
            PointF pos = new PointF(click.X, click.Y);
            if (ImageAspect > controlAspect)
            {
                float ratioWidth = imgSize.Width / (float)pbxSize.Width;
                pos.X *= ratioWidth;
                float scale = pbxSize.Width / (float)imgSize.Width;
                float displayHeight = scale * imgSize.Height;
                float diffHeight = pbxSize.Height - displayHeight;
                diffHeight /= 2;
                pos.Y -= diffHeight;
                pos.Y /= scale;
            }
            else
            {
                float ratioHeight = imgSize.Height / (float)pbxSize.Height;
                pos.Y *= ratioHeight;
                float scale = pbxSize.Height / (float)imgSize.Height;
                float displayWidth = scale * imgSize.Width;
                float diffWidth = pbxSize.Width - displayWidth;
                diffWidth /= 2;
                pos.X -= diffWidth;
                pos.X /= scale;
            }
            return new Point((int)pos.X, (int)pos.Y);
        }

        private double PointDist(Point A, Point B)
        {
            return Math.Sqrt(Math.Pow(B.X - A.X, 2) + Math.Pow(B.Y - A.Y, 2));
        }

        private double SizeDiff(Size A, Size B)
        {
            return (PointDist(new Point(B.Width - A.Width, B.Height - A.Height), Point.Empty));
        }

        #endregion
    }
}
