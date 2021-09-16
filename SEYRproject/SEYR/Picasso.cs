using System;
using System.Drawing;
using System.Windows.Forms;

namespace SEYR
{
    static class Picasso
    {
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
            Point pos = ZoomMousePos(composer, location);
            if (BeginOrCancel)
            {
                if (Clicked)
                {
                    Clicked = false;
                    Paint(composer, Point.Empty, force: true);
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
                if (feature.Valid(composer.pictureBox.Size))
                {
                    FileHandler.Grid.Features.Add(feature);
                    FileHandler.Grid.ActiveFeature = feature;
                    composer.LoadComboBox();
                }
                else
                {
                    Paint(composer, Point.Empty, force: true);
                }
            }
            else
            {
                DisplayFeatureInfo(composer, pos);
            }
        }

        /// <summary>
        /// Update on every cursor move
        /// and draw current rect
        /// </summary>
        /// <param name="composer"></param>
        /// <param name="location"></param>
        /// <param name="force"></param>
        public static void Paint(Composer composer, Point location, bool force = false)
        {
            if (Clicked || force)
            {
                Bitmap bitmap = new Bitmap(composer.pictureBox.BackgroundImage.Width, composer.pictureBox.BackgroundImage.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    DrawTiles(g);

                    if (!force)
                    {
                        CurrentRect = RectFromPoints(StartPoint, ZoomMousePos(composer, location));
                        g.DrawRectangle(new Pen(Brushes.HotPink, 2), CurrentRect);
                    }
                }
                composer.pictureBox.Image = bitmap;
                GC.Collect();
            };
        }

        /// <summary>
        /// ReDraw all rectangles with no relevant mouse position
        /// </summary>
        /// <param name="composer"></param>
        public static void ReDraw(Composer composer)
        {
            Bitmap bitmap = new Bitmap(composer.pictureBox.BackgroundImage.Width, composer.pictureBox.BackgroundImage.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                DrawTiles(g);
            };
            composer.pictureBox.Image = bitmap;
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
        /// <param name="composer"></param>
        /// <param name="click"></param>
        /// <returns></returns>
        private static Point ZoomMousePos(Composer composer, Point click)
        {
            PictureBox pbx = composer.pictureBox;
            float ImageAspect = pbx.Image.Width / (float)pbx.Image.Height;
            float controlAspect = pbx.Width / (float)pbx.Height;
            PointF pos = new PointF(click.X + Offset.X, click.Y + Offset.Y);
            if (ImageAspect > controlAspect)
            {
                float ratioWidth = pbx.Image.Width / (float)pbx.Width;
                pos.X *= ratioWidth;
                float scale = pbx.Width / (float)pbx.Image.Width;
                float displayHeight = scale * pbx.Image.Height;
                float diffHeight = pbx.Height - displayHeight;
                diffHeight /= 2;
                pos.Y -= diffHeight;
                pos.Y /= scale;
            }
            else
            {
                float ratioHeight = pbx.Image.Height / (float)pbx.Height;
                pos.Y *= ratioHeight;
                float scale = pbx.Height / (float)pbx.Image.Height;
                float displayWidth = scale * pbx.Image.Width;
                float diffWidth = pbx.Width - displayWidth;
                diffWidth /= 2;
                pos.X -= diffWidth;
                pos.X /= scale;
            }
            return new Point((int)pos.X, (int)pos.Y);
        }
    }
}
