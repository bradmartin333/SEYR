﻿using Accord.Imaging.Filters;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace SEYR
{
    public partial class Composer : Form
    {
        #region Temporary Image Loading

        private int ImageIdx = 0;
        private int PatternFollowInterval = 400;

        public void NewImage(Bitmap bitmap, int idx)
        {
            try
            {
                BringToFront();
                LoadNewImage(bitmap);
                ImageIdx = idx;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        #endregion

        private bool ComboBoxOverride = false; // Allows for ComboBox refresh without losing selection

        public Composer(Bitmap img)
        {
            InitializeComponent();

            _ = new ImageLoader(this);

            // Scale incoming image and set blank foreground
            double heightRatio = Picasso.BaseHeight / img.Height;
            Bitmap resize = new Bitmap((int)(heightRatio * img.Width), (int)Picasso.BaseHeight);
            pictureBox.Image = resize;

            // Init mouse and keyboard handlers
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseUp += PictureBox_MouseUp;
            pictureBox.MouseMove += PictureBox_MouseMove;
            comboBoxRects.KeyDown += ComboBoxRects_KeyDown;

            FileHandler.Grid.ActiveFeature = new Feature(Rectangle.Empty);
            LoadGrid();
            Show();
            LoadNewImage(img);
        }

        public void LoadNewImage(Bitmap img)
        {
            using (var wc = new WaitCursor())
            {
                Imaging.OriginalImage = (Bitmap)img.Clone(); // Save unedited photo

                // Resize incoming image
                double heightRatio = Picasso.BaseHeight / img.Height;
                Bitmap resize = new Bitmap((int)(heightRatio * img.Width), (int)Picasso.BaseHeight);
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

                pictureBox.BackgroundImage = working;

                Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
                working = filter.Apply(working);

                Threshold threshold = new Threshold(170);
                threshold.ApplyInPlace(working);

                Imaging.CurrentImage = working; // Save edited photo

                if (ImageIdx % PatternFollowInterval == 0)
                    for (int i = 0; i < 3; i++)
                    {
                        bool foundPattern = Imaging.FollowPattern();
                        if (foundPattern)
                        {
                            Debug.WriteLine(string.Format("Pattern Offset: {0}", Picasso.Offset));
                            break;
                        }
                    }

                //DocumentSkewChecker skewChecker = new DocumentSkewChecker();
                //double angle = skewChecker.GetSkewAngle(working);
                //Debug.WriteLine(angle);

                LoadComboBox();
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    break;
                case MouseButtons.Right:
                    break;
                case MouseButtons.Middle:
                    pictureBox.BackgroundImage = Imaging.CurrentImage;
                    break;
                default:
                    break;
            }
        }


        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    Picasso.Click(this, e.Location, false);
                    break;
                case MouseButtons.Right:
                    Picasso.Click(this, e.Location, true);
                    break;
                case MouseButtons.Middle:
                    pictureBox.BackgroundImage = Imaging.DisplayedImage;
                    break;
                default:
                    break;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Picasso.Paint(this, e.Location);
        }

        private void btnLoad_Click(object sender, System.EventArgs e)
        {
            string pathBuffer = FileHandler.LoadFile();
            if (pathBuffer == null)
                return;
            else
                FileHandler.FilePath = pathBuffer;
            FileHandler.ReadParametersFromBinaryFile();
            LoadGrid();
            LoadNewImage(Imaging.OriginalImage);
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            string pathBuffer = FileHandler.SaveFile();
            if (pathBuffer == null)
                return;
            else
                FileHandler.FilePath = pathBuffer;
            FileHandler.WriteParametersToBinaryFile();
        }

        private void LoadGrid()
        {
            numAngle.DataBindings.Clear();
            numAngle.DataBindings.Add(new Binding("Value", FileHandler.Grid, "Angle", false, DataSourceUpdateMode.OnPropertyChanged));

            numCopyX.DataBindings.Clear();
            numCopyX.DataBindings.Add(new Binding("Value", FileHandler.Grid, "NumberX", false, DataSourceUpdateMode.OnPropertyChanged));

            numCopyY.DataBindings.Clear();
            numCopyY.DataBindings.Add(new Binding("Value", FileHandler.Grid, "NumberY", false, DataSourceUpdateMode.OnPropertyChanged));

            numCopyPitchX.DataBindings.Clear();
            numCopyPitchX.DataBindings.Add(new Binding("Value", FileHandler.Grid, "PitchX", false, DataSourceUpdateMode.OnPropertyChanged));

            numCopyPitchY.DataBindings.Clear();
            numCopyPitchY.DataBindings.Add(new Binding("Value", FileHandler.Grid, "PitchY", false, DataSourceUpdateMode.OnPropertyChanged));

            if (!FileHandler.Grid.PatternFeature.Rectangle.IsEmpty) 
                lblFollowerPattern.Text = FileHandler.Grid.PatternFeature.Name;
        }

        public void LoadFeature()
        {
            numOriginX.DataBindings.Clear();
            numOriginX.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "OriginX", false, DataSourceUpdateMode.OnPropertyChanged));

            numOriginY.DataBindings.Clear();
            numOriginY.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "OriginY", false, DataSourceUpdateMode.OnPropertyChanged));

            numSizeX.DataBindings.Clear();
            numSizeX.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "SizeX", false, DataSourceUpdateMode.OnPropertyChanged));

            numSizeY.DataBindings.Clear();
            numSizeY.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "SizeY", false, DataSourceUpdateMode.OnPropertyChanged));

            numPassScore.DataBindings.Clear();
            numPassScore.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "PassScore", false, DataSourceUpdateMode.OnPropertyChanged));

            numPassTol.DataBindings.Clear();
            numPassTol.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "PassTol", false, DataSourceUpdateMode.OnPropertyChanged));

            numFailScore.DataBindings.Clear();
            numFailScore.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "FailScore", false, DataSourceUpdateMode.OnPropertyChanged));

            numFailTol.DataBindings.Clear();
            numFailTol.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "FailTol", false, DataSourceUpdateMode.OnPropertyChanged));

            numAlignTol.DataBindings.Clear();
            numAlignTol.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "AlignTol", false, DataSourceUpdateMode.OnPropertyChanged));

            LoadComboBox();
        }

        public void LoadComboBox()
        {
            ComboBoxOverride = true; // Not a user index change
            comboBoxRects.BeginUpdate(); // Makes GUI look cleaner
            
            comboBoxRects.Text = "";

             // Update data source
            BindingSource bindingSource = new BindingSource() { DataSource = FileHandler.Grid.Features };
            comboBoxRects.DataSource = bindingSource;
            comboBoxRects.DisplayMember = "Name";

            // Find active feature and set as selected item
            comboBoxRects.SelectedIndex = -1;
            int activeIdx = FileHandler.Grid.Features.FindIndex(x => x.Equals(FileHandler.Grid.ActiveFeature));
            if (activeIdx != -1) comboBoxRects.SelectedIndex = activeIdx;

            MakeTiles();

            comboBoxRects.EndUpdate();
            ComboBoxOverride = false;
        }

        public void MakeTiles()
        {
            // Each tile contains one of each feature
            // The top-left tile is index 0,0
            // The bottom-right tile is index i,j
            FileHandler.Grid.Tiles.Clear();
            for (int i = 0; i < FileHandler.Grid.NumberX + 1; i++)
            {
                for (int j = 0; j < FileHandler.Grid.NumberY + 1; j++)
                {
                    Tile tile = new Tile(i ,j);
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
            Picasso.ReDraw(this);
        }

        private void comboBoxRects_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (!ComboBoxOverride)
            {
                foreach (Feature feature in FileHandler.Grid.Features)
                {
                    if (comboBoxRects.GetItemText(comboBoxRects.SelectedItem) == feature.Name)
                        FileHandler.Grid.ActiveFeature = feature;
                }
            }

            DisplayAlignmentStatus();
        }

        private void btnRemoveRect_Click(object sender, System.EventArgs e)
        {
            FileHandler.Grid.Features.Remove(FileHandler.Grid.Features.Find(x => x.Equals(FileHandler.Grid.ActiveFeature)));
            FileHandler.Grid.ActiveFeature = new Feature(Rectangle.Empty);
            LoadComboBox();
        }

        private void btnCopyRect_Click(object sender, System.EventArgs e)
        {
            Feature feature = FileHandler.Grid.ActiveFeature.Clone(userClone: true);
            while (FileHandler.Grid.Features.FindAll(x => x.Name == feature.Name).Count > 0)
            {
                feature.Name += " Clone"; // Find an unique name
            }
            FileHandler.Grid.Features.Add(feature);
            FileHandler.Grid.ActiveFeature = feature;
            LoadComboBox();
        }

        private void ComboBoxRects_KeyDown(object sender, KeyEventArgs e)
        {
            if (FileHandler.Grid.Features.Count == 0) return;

            if (e.KeyCode == Keys.Enter)
            {
                // Make sure it is an unique name
                foreach (Feature feature in FileHandler.Grid.Features)
                {
                    if (comboBoxRects.Text == feature.Name)
                    {
                        comboBoxRects.Text = FileHandler.Grid.ActiveFeature.Name;
                        return;
                    }
                }

                // Update selected feature's name
                Feature updateFeature = FileHandler.Grid.Features.Find(x => x.Equals(FileHandler.Grid.ActiveFeature));
                updateFeature.Name = comboBoxRects.Text;
                FileHandler.Grid.ActiveFeature = updateFeature;
                LoadComboBox();
            }
        }

        private void btnTrainPattern_Click(object sender, System.EventArgs e)
        {
            FileHandler.Grid.PatternFeature = FileHandler.Grid.ActiveFeature;
            LoadFollowerPattern();
        }

        private void LoadFollowerPattern()
        {
            Rectangle cropRect = FileHandler.Grid.PatternFeature.Rectangle;
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(Imaging.CurrentImage, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
            }
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            target = filter.Apply(target);
            FileHandler.Grid.PatternBitmap = target;
            lblFollowerPattern.Text = FileHandler.Grid.PatternFeature.Name;
            Imaging.FollowPattern();
            Picasso.ReDraw(this);
        }

        private void buttonForgetPattern_Click(object sender, System.EventArgs e)
        {
            FileHandler.Grid.PatternFeature = new Feature(Rectangle.Empty);
            FileHandler.Grid.PatternBitmap = new Bitmap(1, 1);
            lblFollowerPattern.Text = "N/A";
            Picasso.Offset = Point.Empty;
            Picasso.ReDraw(this);
        }

        private void btnTrainAlignment_Click(object sender, System.EventArgs e)
        {
            if (comboBoxRects.SelectedIndex == -1)
            {
                MessageBox.Show("Select a feature to train alignment");
                return;
            }

            if (Application.OpenForms.OfType<Alignment>().Any())
                Application.OpenForms.OfType<Alignment>().First().BringToFront();
            else
            {
                Alignment alignment = new Alignment();
            }
        }

        public void DisplayAlignmentStatus()
        {
            if (FileHandler.Grid.ActiveFeature.CheckAlign)
                btnTrainAlignment.BackColor = Color.LawnGreen;
            else
                btnTrainAlignment.BackColor = Color.Transparent;
        }
    }
}
