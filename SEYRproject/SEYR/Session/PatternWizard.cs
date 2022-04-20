﻿using Accord.Imaging;
using SEYR.ImageProcessing;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SEYR.Session
{
    public partial class PatternWizard : Form
    {
        private enum WizardState
        {
            Null,
            Crop,
            Score,
            Interval,
        }

        private WizardState State = WizardState.Null;
        private readonly Bitmap InputImage;

        public PatternWizard(Bitmap bmp)
        {
            InitializeComponent();
            PBX.MouseDown += PBX_MouseDown;
            PBX.MouseMove += PBX_MouseMove;
            BitmapFunctions.ResizeAndRotate(ref bmp);
            InputImage = bmp;

            NumPatternScore.Value = (decimal)Channel.Project.PatternScore;
            ComboPatternInterval.Items.AddRange(DataStream.Header.Split('\t'));
            ComboPatternInterval.Text = Channel.Project.PatternIntervalString;
            NumPatternInterval.Value = Channel.Project.PatternIntervalValue;
            NumPatternDeltaMax.Value = Channel.Project.PatternDeltaMax;

            CropUtility();
        }

        #region Crop

        private bool DrawingRect = false;
        private Point StartPoint = Point.Empty;
        private Point EndPoint = Point.Empty;
        private Rectangle CropRectangle = Rectangle.Empty;

        private void PBX_MouseDown(object sender, MouseEventArgs e)
        {
            if (State != WizardState.Crop) return;
            DrawingRect = !DrawingRect;
            if (DrawingRect)
            {
                StartPoint = ZoomMousePos(e.Location);
                EndPoint = StartPoint;
                PBX.Image = null;
            }
            UpdateCrop();
        }

        private void PBX_MouseMove(object sender, MouseEventArgs e)
        {
            if (State != WizardState.Crop) return;
            if (DrawingRect) EndPoint = ZoomMousePos(e.Location);
            UpdateCrop();
        }

        private void CropUtility()
        {
            State = WizardState.Crop;
            WizardLabel.Text = "Click then drag mouse to select a crop region. Click again to finish.\nClick continue to save new pattern or to proceed with already saved pattern.";
            PBX.BackgroundImage = InputImage;
        }

        private void UpdateCrop()
        {
            if (StartPoint == EndPoint) return;
            Bitmap bmp = new Bitmap(InputImage.Width, InputImage.Height);
            CropRectangle = new Rectangle(
                Math.Min(StartPoint.X, EndPoint.X),
                Math.Min(StartPoint.Y, EndPoint.Y),
                Math.Abs(EndPoint.X - StartPoint.X),
                Math.Abs(EndPoint.Y - StartPoint.Y));
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawRectangle(new Pen(Brushes.HotPink, (float)(Math.Min(bmp.Height, bmp.Width) * 0.005)), CropRectangle);
            }
            PBX.Image = bmp;
        }

        private void SavePattern()
        {
            if (CropRectangle == Rectangle.Empty) return;
            Bitmap crop = new Bitmap(CropRectangle.Width, CropRectangle.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(crop))
            {
                g.DrawImage(InputImage, new Rectangle(Point.Empty, CropRectangle.Size), CropRectangle, GraphicsUnit.Pixel);
            }
            crop.Save(Channel.PatternPath);
            Channel.Pattern = crop;
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
            Size imgSize = InputImage.Size;
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

        #endregion

        #region Score

        private void ScoreUtility(float score = -1f)
        {
            WizardLabel.Text = "Decrease the pattern score as much as possible without allowing false positives.";
            PBX.Image = null;
            RTB.Visible = true;
            RTB.Text = "Score\tCount\n";
            FlowScore.Visible = true;
            BtnFindPatterns.BackColor = Color.Gold;
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            if (Channel.Pattern != null)
            {
                List<Point> patternLocations = new List<Point>();
                Bitmap sourceImage = (Bitmap)InputImage.Clone();
                var tm = new ExhaustiveTemplateMatching(score == -1 ? (float)NumPatternScore.Value : score);
                TemplateMatch[] matchings = tm.ProcessImage(sourceImage, Channel.Pattern);
                foreach (TemplateMatch m in matchings)
                {
                    Channel.DebugStream.Write($"Pattern match: {m.Similarity} {m.Rectangle}");
                    patternLocations.Add(m.Rectangle.Center());
                    using (Graphics g = Graphics.FromImage(sourceImage))
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.HotPink)), m.Rectangle);
                    }
                }
                UpdateRTB(matchings);
                PBX.Image = sourceImage;
            }
            Cursor = Cursors.Default;
            BtnFindPatterns.BackColor = Color.White;
        }

        private void UpdateRTB(TemplateMatch[] matchings)
        {
            var groups = matchings.GroupBy(x => Math.Round(x.Similarity, 2));
            foreach (var group in groups)
                RTB.Text += $"{group.Key:F2}\t{group.Count()}\n";
            RTB.Text += $"\nTotal\t{matchings.Length}";
        }

        private void BtnFindPatterns_Click(object sender, EventArgs e)
        {
            ScoreUtility((float)NumPatternScore.Value);
        }

        private void ScoreScore()
        {
            Channel.Project.PatternScore = (float)NumPatternScore.Value;
        }

        #endregion

        #region Interval

        private void IntervalUtility()
        {
            WizardLabel.Text = "Configure the pattern follow criteria.";
            RTB.Visible = false;
            FlowScore.Visible = false;
            FlowInterval.Visible = true;
            FlowDelta.Visible = true;
        }

        private void SaveInterval()
        {
            Channel.Project.PatternIntervalString = ComboPatternInterval.Text;
            Channel.Project.PatternIntervalValue = (int)NumPatternInterval.Value;
            Channel.Project.PatternDeltaMax = (int)NumPatternDeltaMax.Value;
        }

        #endregion

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            switch (State)
            {
                case WizardState.Null:
                    break;
                case WizardState.Crop:
                    SavePattern();
                    State = WizardState.Score;
                    ScoreUtility();
                    break;
                case WizardState.Score:
                    ScoreScore();
                    State = WizardState.Interval;
                    IntervalUtility();
                    break;
                case WizardState.Interval:
                    SaveInterval();
                    DialogResult = DialogResult.OK;
                    Close();
                    break;
            }
        }
    }
}