using Accord.Imaging.Filters;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static SEYR.DataBindings;
using static SEYR.Pipeline;

namespace SEYR
{
    public partial class Composer : Form
    {
        private bool ComboBoxOverride = false; // Allows for ComboBox refresh without losing selection
        private bool RunningAllImages = false;

        public Composer()
        {
            InitializeComponent();

            // Init mouse and keyboard handlers
            PBX.MouseDown += PictureBox_MouseDown;
            PBX.MouseUp += PictureBox_MouseUp;
            PBX.MouseMove += PictureBox_MouseMove;
            KeyDown += Composer_KeyDown;
            comboBoxRects.KeyDown += ComboBoxRects_KeyDown;

            LoadGrid(this);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
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
            Pipeline.Viewer.Show();
            Pipeline.Viewer.BringToFront();
        }

        private void deselectFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHandler.Grid.ActiveFeature = new Feature(Rectangle.Empty);
            LoadComboBox();
        }

        private void pixelPitchCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pipeline.PixelPitch.Show();
            Pipeline.PixelPitch.BringToFront();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(text: "Coming Soon", caption: "SEYR Help");
        }

        private void copyToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var wc = new WaitCursor())
            {
                FileHandler.CopyCurrentGridForExcel();
            }
        }

        #region PictureBox Bindings

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!ImageReady) return;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                        PBX.BackgroundImage = Imaging.CurrentImage;
                    break;
                case MouseButtons.Right:
                    break;
                case MouseButtons.Middle:
                    PBX.BackgroundImage = Imaging.CurrentImage;
                    break;
                default:
                    break;
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (!ImageReady) return;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                        PBX.BackgroundImage = Imaging.DisplayedImage;
                    else
                        Picasso.Click(this, e.Location, false);
                    break;
                case MouseButtons.Right:
                    Picasso.Click(this, e.Location, true);
                    break;
                case MouseButtons.Middle:
                    PBX.BackgroundImage = Imaging.DisplayedImage;
                    break;
                default:
                    break;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Picasso.Paint(e.Location);
        }

        public Size GetPBXSize()
        {
            return PBX.Size;
        }

        public void ClearGraphics()
        {
            PBX.BackgroundImage = new Bitmap(Picasso.ThisSize.Width, Picasso.ThisSize.Height);
            PBX.Image = new Bitmap(Picasso.ThisSize.Width, Picasso.ThisSize.Height);
        }

        public void InsertNewImage(Bitmap background, Bitmap foreground)
        {
            ClearGraphics();
            PBX.BackgroundImage = background;
            PBX.Image = foreground;
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

            comboBoxRects.BackColor = SystemColors.Window;

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
            comboBoxRects.BackColor = SystemColors.Window;
        }

        private void btnRemoveRect_Click(object sender, EventArgs e)
        {
            FileHandler.Grid.Features.Remove(FileHandler.Grid.Features.Find(x => x.Equals(FileHandler.Grid.ActiveFeature)));
            FileHandler.Grid.ActiveFeature = new Feature(Rectangle.Empty);
            LoadComboBox();
            FileHandler.Grid.MakeTiles();
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
            FileHandler.Grid.MakeTiles();
        }

        private void ComboBoxRects_KeyDown(object sender, KeyEventArgs e)
        {
            if (FileHandler.Grid.Features.Count == 0 || FileHandler.Grid.ActiveFeature.Equals(new Feature(Rectangle.Empty))) return;

            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(comboBoxRects.Text))
                {
                    comboBoxRects.Text = FileHandler.Grid.ActiveFeature.Name;
                    return;
                }

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
            else
            {
                comboBoxRects.BackColor = Color.PaleGoldenrod; // Indicate that there are unsaved changes
            }
        }

        private void copyCurrentScoreAndToleranceForAllFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileHandler.Grid.ActiveFeature.Rectangle == Rectangle.Empty)
            {
                MessageBox.Show("No feature selected", "Copy Parameters");
                return;
            }
            foreach (Feature feature in FileHandler.Grid.Features)
            {
                feature.CopyActiveScoreAndTolerance();
            }
            FileHandler.Grid.MakeTiles();
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
            Picasso.ReDraw();
        }

        private void trainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBoxRects.SelectedIndex == -1) return;
            FileHandler.Grid.PatternFeature = FileHandler.Grid.ActiveFeature;
            LoadFollowerPattern();
            followerPatternToolStripMenuItem.BackColor = SystemColors.Control;
        }

        private void forgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHandler.Grid.PatternFeature = new Feature(Rectangle.Empty);
            FileHandler.Grid.PatternBitmap = new Bitmap(1, 1);
            followerPatternNameToolStripMenuItem.Text = "N/A";
            Picasso.Offset = Point.Empty;
            Picasso.ReDraw();
            followerPatternToolStripMenuItem.BackColor = SystemColors.Control;
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

        #region File Management

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHandler.FilePath = string.Empty;
            FileHandler.Grid = new Grid();
            LoadGrid(this);
            LoadFeature(this);
            LoadNewImage(Imaging.OriginalImage);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pathBuffer = FileHandler.LoadFile();
            if (pathBuffer == null)
                return;
            else
                FileHandler.FilePath = pathBuffer;
            FileHandler.ReadParametersFromBinaryFile();
            LoadGrid(this);
            LoadNewImage(Imaging.OriginalImage);
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

        #region Advanced Tools

        private void detectFilterThresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ImageReady) return;
            int detectedVal = 170;

            using (var wc = new WaitCursor())
            {
                try
                {
                    detectedVal = AdvancedTools.Filtering.DetectThreshold();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Advanced Tools");
                }
            }

            numFilterThreshold.Value = detectedVal;
        }

        #endregion
    }
}
