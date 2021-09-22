using System;
using System.Drawing;
using System.Windows.Forms;
using static SEYR.Pipeline;

namespace SEYR
{
    static class Picasso
    {
        public static Size IncomingSize = new Size(1, 1);
        public static Size ThisSize = new Size(1, 1);
        public static double BaseHeight = 600; // Defines scaling dimension of incoming images
        public static Point Offset { get; set; } // XY Offset of Current Image

        private static Rectangle CurrentRect;
        private static Point StartPoint;
        private static bool Clicked;

        /// <summary>
        /// Make a nullable rectangle from 2 points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static Rectangle RectFromPoints(Point a, Point b)
        {
            if (a.IsEmpty && b.IsEmpty)
                return Rectangle.Empty;

            Point min = new Point(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
            Point max = new Point(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));

            return new Rectangle(min, new Size(max.X - min.X, max.Y - min.Y));
        }

        /// <summary>
        /// Toggle pressed state either starting
        /// or finishing a rectangle
        /// </summary>
        /// <param name="composer"></param>
        /// <param name="location"></param>
        /// <param name="BeginOrCancel"></param>
        public static void Click(Composer composer, Point location, bool BeginOrCancel)
        {
            if (FileHandler.ImageDirectoryPath == string.Empty) return;
            Point pos = ZoomMousePos(location);
            if (BeginOrCancel)
            {
                if (Clicked)
                {
                    Clicked = false;
                    Paint(Point.Empty, force: true);
                }
                else
                {
                    StartPoint = pos;
                    Clicked = true;
                }
            }
            else if (Clicked)
            {
                Feature feature = new Feature(CurrentRect);
                Clicked = false;
                if (feature.Valid(PBX.Size))
                {
                    FileHandler.Grid.Features.Add(feature);
                    FileHandler.Grid.ActiveFeature = feature;
                    composer.LoadComboBox();
                    MakeTiles();
                }
                else
                {
                    Paint(Point.Empty, force: true);
                }
            }
            else
            {
                DisplayFeatureInfo(composer, pos);
            }
        }

        /// <summary>
        /// Clear all drawn features
        /// </summary>
        /// <param name="composer"></param>
        public static void ClearGraphics()
        {
            // Scale incoming image and set blank foreground
            double heightRatio = BaseHeight / IncomingSize.Height;
            Bitmap resize = new Bitmap((int)(heightRatio * IncomingSize.Width), (int)BaseHeight);
            PBX.Image = resize;
            ThisSize = resize.Size;
        }

        /// <summary>
        /// Update on every cursor move
        /// and draw current rect
        /// </summary>
        /// <param name="location"></param>
        /// <param name="force"></param>
        public static void Paint(Point location, bool force = false)
        {
            if (Clicked || force)
            {
                Bitmap bitmap = new Bitmap(ThisSize.Width, ThisSize.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    DrawTiles(g);

                    if (!force)
                    {
                        CurrentRect = RectFromPoints(StartPoint, ZoomMousePos(location));
                        g.DrawRectangle(new Pen(Brushes.HotPink, 2), CurrentRect);
                    }
                }
                PBX.Image = bitmap;
                GC.Collect();
            };
        }

        /// <summary>
        /// ReDraw all rectangles with no relevant mouse position
        /// </summary>
        public static void ReDraw()
        {
            Bitmap bitmap = new Bitmap(ThisSize.Width, ThisSize.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                DrawTiles(g);
            };
            PBX.Image = bitmap;
            Application.DoEvents();
        }

        private static void DrawTiles(Graphics g)
        {
            foreach (Tile tile in FileHandler.Grid.Tiles)
            {
                foreach (Feature feature in tile.Features)
                {
                    Rectangle rect = new Rectangle(
                        feature.OriginX - Offset.X,
                        feature.OriginY - Offset.Y,
                        feature.Rectangle.Width,
                        feature.Rectangle.Height);

                    if (feature.Name == FileHandler.Grid.ActiveFeature.Name)
                        g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Gold)), rect);

                    switch (feature.State)
                    {
                        case DataHandler.State.Pass:
                            g.DrawRectangle(new Pen(Brushes.LawnGreen, 3), rect);
                            break;
                        case DataHandler.State.Fail:
                            g.DrawRectangle(new Pen(Brushes.Firebrick, 3), rect);
                            break;
                        case DataHandler.State.Null:
                            g.DrawRectangle(new Pen(Brushes.Blue, 3), rect);
                            break;
                        case DataHandler.State.Misaligned:
                            g.DrawRectangle(new Pen(Brushes.ForestGreen, 3), rect);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static void DisplayFeatureInfo(Composer composer, Point point)
        {
            bool match = false;
            foreach (Tile tile in FileHandler.Grid.Tiles)
            {
                foreach (Feature feature in tile.Features)
                {
                    if (feature.Rectangle.Contains(point))
                    {
                        match = true;
                        composer.lblName.Text = feature.Name;
                        composer.lblIndex.Text = string.Format("{0}, {1}", feature.Index.X, feature.Index.Y);
                        composer.lblScore.Text = feature.Score.ToString();
                    }
                }
            }
            if (!match)
            {
                composer.lblName.Text = "N/A";
                composer.lblIndex.Text = "N/A";
                composer.lblScore.Text = "N/A";
            }
        }

        /// <summary>
        /// Copy paste method for adjusting mouse pos to pictureBox set to Zoom
        /// </summary>
        /// <param name="click"></param>
        /// <returns></returns>
        private static Point ZoomMousePos(Point click)
        {
            float ImageAspect = PBX.Image.Width / (float)PBX.Image.Height;
            float controlAspect = PBX.Width / (float)PBX.Height;
            PointF pos = new PointF(click.X + Offset.X, click.Y + Offset.Y);
            if (ImageAspect > controlAspect)
            {
                float ratioWidth = PBX.Image.Width / (float)PBX.Width;
                pos.X *= ratioWidth;
                float scale = PBX.Width / (float)PBX.Image.Width;
                float displayHeight = scale * PBX.Image.Height;
                float diffHeight = PBX.Height - displayHeight;
                diffHeight /= 2;
                pos.Y -= diffHeight;
                pos.Y /= scale;
            }
            else
            {
                float ratioHeight = PBX.Image.Height / (float)PBX.Height;
                pos.Y *= ratioHeight;
                float scale = PBX.Height / (float)PBX.Image.Height;
                float displayWidth = scale * PBX.Image.Width;
                float diffWidth = PBX.Width - displayWidth;
                diffWidth /= 2;
                pos.X -= diffWidth;
                pos.X /= scale;
            }
            return new Point((int)pos.X, (int)pos.Y);
        }
    }
}
