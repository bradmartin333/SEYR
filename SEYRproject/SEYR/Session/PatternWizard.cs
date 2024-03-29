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
            string[] comboVals = LogStream.Header.Split('\t');
            ComboPatternInterval.Items.AddRange(comboVals.Take(comboVals.Length - 2).ToArray());
            ComboPatternInterval.Text = Channel.Project.PatternIntervalString;
            NumPatternInterval.Value = Channel.Project.PatternIntervalValue;
            NumPatternDeltaMax.Value = Channel.Project.PatternDeltaMax;

            CropUtility();
            FormClosing += PatternWizard_FormClosing;
        }

        private void PatternWizard_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.PatternWizard_Valid)
            {
                if (Properties.Settings.Default.PatternWizard_Maximized)
                {
                    Location = Properties.Settings.Default.PatternWizard_Location;
                    WindowState = FormWindowState.Maximized;
                    Size = Properties.Settings.Default.PatternWizard_Size;
                }
                else if (Properties.Settings.Default.PatternWizard_Minimized)
                {
                    Location = Properties.Settings.Default.PatternWizard_Location;
                    WindowState = FormWindowState.Minimized;
                    Size = Properties.Settings.Default.PatternWizard_Size;
                }
                else
                {
                    Location = Properties.Settings.Default.PatternWizard_Location;
                    Size = Properties.Settings.Default.PatternWizard_Size;
                }
            }
            Properties.Settings.Default.PatternWizard_Valid = true;
        }

        private void PatternWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.PatternWizard_Location = RestoreBounds.Location;
                Properties.Settings.Default.PatternWizard_Size = RestoreBounds.Size;
                Properties.Settings.Default.PatternWizard_Maximized = true;
                Properties.Settings.Default.PatternWizard_Minimized = false;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.PatternWizard_Location = Location;
                Properties.Settings.Default.PatternWizard_Size = Size;
                Properties.Settings.Default.PatternWizard_Maximized = false;
                Properties.Settings.Default.PatternWizard_Minimized = false;
            }
            else
            {
                Properties.Settings.Default.PatternWizard_Location = RestoreBounds.Location;
                Properties.Settings.Default.PatternWizard_Size = RestoreBounds.Size;
                Properties.Settings.Default.PatternWizard_Maximized = false;
                Properties.Settings.Default.PatternWizard_Minimized = true;
            }
            Properties.Settings.Default.Save();
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
            WizardLabel.Text = "Click to create a box around the pattern region. Click again to complete selection.\nClick continue to proceed or load previously saved pattern.";
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save pattern: {ex}", "SEYR");
            }  
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

        List<Point> PatternLocations = new List<Point>();

        private void ScoreUtility(float score = -1f)
        {
            WizardLabel.Text = "Lower the pattern score as much as possible without allowing false positives.";
            PBX.Image = null;
            RTB.Visible = true;
            FlowScore.Visible = true;
            BtnFindPatterns.BackColor = Color.Gold;
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            if (Channel.Pattern != null)
            {
                PatternLocations = new List<Point>();
                Bitmap sourceImage = (Bitmap)InputImage.Clone();
                var tm = new ExhaustiveTemplateMatching(score == -1 ? (float)NumPatternScore.Value : score);

                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                TemplateMatch[] matchings = tm.ProcessImage(sourceImage, Channel.Pattern);
                Channel.DebugStream.Write($"\t\t");
                RTB.Text = $"Found in {Math.Round(sw.Elapsed.TotalSeconds, 3)} seconds\nScore\tCount\n";

                foreach (TemplateMatch m in matchings)
                {
                    Channel.DebugStream.Write($"Pattern match: {m.Similarity} {m.Rectangle}");
                    PatternLocations.Add(m.Rectangle.Center());
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

        private void SaveScore()
        {
            Channel.Project.PatternScore = (float)NumPatternScore.Value;
            Channel.Project.PatternLocations = PatternLocations;
        }

        #endregion

        #region Interval

        private void IntervalUtility()
        {
            WizardLabel.Text = "Configure the pattern follow interval. " +
                "If using callbacks to trigger the pattern interval, leave the internal disabled and click continue.";
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
                    SaveScore();
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

        private void BtnCloseAndReopen_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
            Close();
        }
    }
}
