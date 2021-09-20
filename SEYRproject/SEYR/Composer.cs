using Accord.Imaging.Filters;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SEYR.DataBindings;

namespace SEYR
{
    public partial class Composer : Form
    {
        public PictureBox pictureBox = Pipeline.PBX;
        private bool ComboBoxOverride = false; // Allows for ComboBox refresh without losing selection
        private bool RunningAllImages = false;

        public Composer()
        {
            InitializeComponent();
            KeyDown += Composer_KeyDown;

            panel.Controls.Add(pictureBox);
            
            // Init mouse and keyboard handlers
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseUp += PictureBox_MouseUp;
            pictureBox.MouseMove += PictureBox_MouseMove;
            comboBoxRects.KeyDown += ComboBoxRects_KeyDown;

            FileHandler.Grid.ActiveFeature = new Feature(Rectangle.Empty);
            LoadGrid(this);
        }

        private void Composer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && RunningAllImages)
                RunningAllImages = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHandler.Viewer.Show();
            FileHandler.Viewer.BringToFront();
        }

        #region Core Functions

        public void MakeTiles()
        {
            if (FileHandler.ImageIdx == 0) return;

            if (FileHandler.ImageIdx > DataHandler.Output.Count)
                DataHandler.Output.Add(string.Empty);
            else
                DataHandler.Output[FileHandler.ImageIdx - 1] = string.Empty;

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
                tile.Score(FileHandler.ImageIdx);
            Picasso.ReDraw(this);
        }

        public async Task LoadNewImage(Bitmap img, bool setup = false)
        {
            if (FileHandler.ImageDirectoryPath == string.Empty || setup)
            {
                Imaging.OriginalImage = (Bitmap)pictureBox.Image;
                Imaging.DisplayedImage = (Bitmap)pictureBox.Image;
                Imaging.CurrentImage = (Bitmap)pictureBox.Image;
                return;
            }

            using (var wc = new WaitCursor())
            {
                FileHandler.ImageIdx++;
                Picasso.IncomingSize = img.Size;
                Picasso.ClearGraphics(this);
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

                Threshold threshold = new Threshold(FileHandler.Grid.FilterThreshold);
                threshold.ApplyInPlace(working);

                Imaging.CurrentImage = working; // Save edited photo

                if (FileHandler.ImageIdx % FileHandler.PatternFollowInterval == 0 && !FileHandler.Grid.PatternFeature.Rectangle.IsEmpty)
                {
                    Bitmap clone = (Bitmap)Imaging.CurrentImage.Clone();
                    for (int i = 0; i < 3; i++)
                    {
                        bool foundPattern = await Imaging.FollowPattern(clone);
                        if (foundPattern)
                        {
                            break;
                        }
                    }
                }

                MakeTiles();
                progressBar.Invoke((MethodInvoker)delegate () { progressBar.Value = FileHandler.ImageIdx; });
                FileHandler.Viewer.InsertNewImage(pictureBox);
            }
        }

        #endregion 

        #region PictureBox Bindings

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

        #endregion

        #region Feature Management

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

            comboBoxRects.EndUpdate();
            ComboBoxOverride = false;
        }

        private void comboBoxRects_SelectedIndexChanged(object sender, EventArgs e)
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

        private void btnRemoveRect_Click(object sender, EventArgs e)
        {
            FileHandler.Grid.Features.Remove(FileHandler.Grid.Features.Find(x => x.Equals(FileHandler.Grid.ActiveFeature)));
            FileHandler.Grid.ActiveFeature = new Feature(Rectangle.Empty);
            LoadComboBox();
            MakeTiles();
        }

        private void btnCopyRect_Click(object sender, EventArgs e)
        {
            Feature feature = FileHandler.Grid.ActiveFeature.Clone(userClone: true);
            while (FileHandler.Grid.Features.FindAll(x => x.Name == feature.Name).Count > 0)
            {
                feature.Name += " Clone"; // Find an unique name
            }
            FileHandler.Grid.Features.Add(feature);
            FileHandler.Grid.ActiveFeature = feature;
            LoadComboBox();
            MakeTiles();
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

        #endregion

        #region Follower Pattern

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
            followerPatternNameToolStripMenuItem.Text = FileHandler.Grid.PatternFeature.Name;
            Picasso.ReDraw(this);
        }

        private void trainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBoxRects.SelectedIndex == -1) return;
            FileHandler.Grid.PatternFeature = FileHandler.Grid.ActiveFeature;
            LoadFollowerPattern();
        }

        private void forgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHandler.Grid.PatternFeature = new Feature(Rectangle.Empty);
            FileHandler.Grid.PatternBitmap = new Bitmap(1, 1);
            followerPatternNameToolStripMenuItem.Text = "N/A";
            Picasso.Offset = Point.Empty;
            Picasso.ReDraw(this);
        }

        private void followerPatternNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileHandler.Grid.PatternFeature.Rectangle.IsEmpty) return;

            Form form = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                Text = FileHandler.Grid.PatternFeature.Name,
                Icon = this.Icon
            };

            PictureBox pictureBox = new PictureBox()
            {
                Image = FileHandler.Grid.PatternBitmap,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };

            form.Controls.Add(pictureBox);
            form.Show();
        }

        #endregion

        #region Alignment

        private void btnTrainAlignment_Click(object sender, EventArgs e)
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
                _ = new Alignment();
            }
        }

        public void DisplayAlignmentStatus()
        {
            if (FileHandler.Grid.ActiveFeature.CheckAlign)
                btnTrainAlignment.BackColor = Color.LawnGreen;
            else
                btnTrainAlignment.BackColor = Color.Transparent;
        }

        #endregion

        #region Image Loading

        private async Task<bool> LoadNewImageFromDir()
        {
            if (Application.UseWaitCursor == true) return false;
            if (FileHandler.ImageDirectoryPath == string.Empty || FileHandler.ImageIdx == FileHandler.Images.Length)
                return false;
            Bitmap bitmap = new Bitmap(FileHandler.Images[FileHandler.ImageIdx]);
            await LoadNewImage((Bitmap)bitmap.Clone());
            return true;
        }

        private void openImageDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pathBuffer = FileHandler.OpenDirectory("Select a directory containing target images");
            if (pathBuffer == null)
                return;
            else
                FileHandler.ImageDirectoryPath = pathBuffer;
            progressBar.Value = 0;
            progressBar.Maximum = FileHandler.Images.Length;
        }

        private async void nextImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LoadNewImageFromDir();
        }

        private async void runAllImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningAllImages = true;
            Text = "SEYR Composer          Press ESC to cancel Run All";
            while (true)
            {
                Application.DoEvents();
                bool result = await LoadNewImageFromDir();
                if (!result || !RunningAllImages) break;
            }
            Text = "SEYR Composer";
        }

        private async void startOverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHandler.ImageIdx = 0;
            await LoadNewImageFromDir();
        }

        private async void goBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHandler.ImageIdx -= 2;
            if (FileHandler.ImageIdx < 0) FileHandler.ImageIdx = 0;
            await LoadNewImageFromDir();
        }

        #endregion

        #region File Management

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHandler.FilePath = string.Empty;
            FileHandler.Grid = new Grid();
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pathBuffer = FileHandler.LoadFile();
            if (pathBuffer == null)
                return;
            else
                FileHandler.FilePath = pathBuffer;
            FileHandler.ReadParametersFromBinaryFile();
            LoadGrid(this);
            await LoadNewImage(Imaging.OriginalImage, true);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileHandler.FilePath != string.Empty)
                FileHandler.WriteParametersToBinaryFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pathBuffer = FileHandler.SaveFile("Save Simple Entropy Yield Routine", "SEYR (*.seyr) | *.seyr");
            if (pathBuffer == null)
                return;
            else
                FileHandler.FilePath = pathBuffer;
            FileHandler.WriteParametersToBinaryFile();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pathBuffer = FileHandler.SaveFile("Save Simple Entropy Yield Routine Report", "Text File (*.txt) | *.txt");
            if (pathBuffer == null)
                return;
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (string str in DataHandler.Output)
                {
                    sb.Append(str);
                }
                File.WriteAllText(pathBuffer, sb.ToString());
            }
        }

        #endregion  
    }
}
