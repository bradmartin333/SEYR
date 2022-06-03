using System;
using System.Drawing;
using System.Windows.Forms;
using static SEYR.ImageProcessing.BitmapFunctions;

namespace SEYR.ImageProcessing
{
    public partial class StampParameterEntry : Form
    {
        private readonly Bitmap Train = null;
        private readonly bool LOADING = false;
        private static bool FirstOpen = true;

        public StampParameterEntry(Bitmap bmp)
        {
            InitializeComponent();
            Train = bmp;
            if (!FirstOpen)
            {
                NumScaling.Value = (decimal)StampScaling;
                TrackbarThreshold.Value = StampThreshold;
            }
            ApplyFilters();
            PBX.MouseDown += PBX_MouseDown;
            PBX.MouseMove += PBX_MouseMove;
            PBX.MouseLeave += PBX_MouseLeave;
            FirstOpen = false;
        }

        private void BtnClearMasks_Click(object sender, EventArgs e)
        {
            StampPosts.Clear();
            StampMasks.Clear();
            UpdateOverlay();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NumScaling_ValueChanged(object sender, EventArgs e)
        {
            if (!LOADING) ApplyFilters();
        }

        private void TrackbarThreshold_Scroll(object sender, EventArgs e)
        {
            if (!LOADING) ApplyFilters();
        }

        private async void ApplyFilters()
        {
            StampScaling = (double)NumScaling.Value;
            StampThreshold = TrackbarThreshold.Value;
            LblThreshold.Text = StampThreshold.ToString();
            (Bitmap, Bitmap, double) stampResult = await ProcessStampImage(Train, setup: true);
            PBX.BackgroundImage = stampResult.Item1;
            PBX.Image = stampResult.Item2;
        }

        private bool DrawingPost = false;
        private bool DrawingMask = false;
        private Point StartPoint = Point.Empty;
        private Point EndPoint = Point.Empty;

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

            if (btn == MouseButtons.Left && !DrawingPost) StampPosts.Add(rect);
            if (btn == MouseButtons.Right && !DrawingMask) StampMasks.Add(rect);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                if (DrawingMask) g.FillRectangle(Brushes.LawnGreen, rect);
                else if (DrawingPost) g.DrawRectangle(new Pen(Brushes.HotPink, (float)(Math.Min(bmp.Height, bmp.Width) * 0.005)), rect);
                foreach (Rectangle post in StampPosts)
                    g.DrawRectangle(new Pen(Brushes.HotPink, (float)(Math.Min(bmp.Height, bmp.Width) * 0.005)), post);
                foreach (Rectangle mask in StampMasks)
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
    }
}
