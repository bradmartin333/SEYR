using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SEYR.Session
{
    public partial class CriteriaWizard : Form
    {
        public readonly List<string[]> Criteria = new List<string[]>();

        private Size _Size;
        private readonly List<Feature> _Features;
        private List<string> _SelectedFeatures = new List<string>();
        private string _LastHover = string.Empty;
        private bool ADDING = false;
        private bool GROUPING = false;

        public CriteriaWizard(Size size, List<Feature> features, List<string[]> criteria)
        {
            InitializeComponent();
            _Size = size;
            _Features = features;
            Criteria = criteria;
            PBX.MouseMove += PBX_MouseMove;
            PBX.MouseUp += PBX_MouseUp;
            MakeBackground();
            RefreshCombo(false);
        }

        private void RefreshCombo(bool select)
        {
            ComboSelector.Items.Clear();
            ComboSelector.Text = "";

            //_SelectedFeatures.Clear();
            //MakeForeground(Point.Empty); // Clear selections

            List<string> names = new List<string>();
            foreach (string[] criterion in Criteria)
                names.Add(string.Join(", ", criterion));
            ComboSelector.Items.AddRange(names.ToArray());
            if (select) 
                ComboSelector.SelectedIndex = names.Count - 1;
            else
                ComboSelector.SelectedIndex = -1;
        }

        private void PBX_MouseUp(object sender, MouseEventArgs e)
        {
            if (_LastHover != null)
            {
                if (_SelectedFeatures.Contains(_LastHover))
                    _SelectedFeatures.Remove(_LastHover);
                else
                    _SelectedFeatures.Add(_LastHover);
            }
        }

        private void PBX_MouseMove(object sender, MouseEventArgs e)
        {
            MakeForeground(ZoomMousePos(e.Location));
        }

        private void MakeForeground(Point point)
        {
            if (_Features.Count == 0) return;
            Bitmap bitmap = new Bitmap(_Size.Width, _Size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                foreach (string selectedFeature in _SelectedFeatures)
                {
                    Feature[] features = _Features.Where(x => x.Name == selectedFeature).ToArray();
                    if (features.Any())
                        g.FillRectangle(new SolidBrush(Color.FromArgb(75, Color.Green)), features[0].Rectangle);
                }
                Feature lastFeature = _LastHover == string.Empty ? _Features[0] : _Features.Where(x => x.Name == _LastHover).First();
                foreach (Feature feature in _Features.OrderBy(x => x.Rectangle.Width * x.Rectangle.Height))
                {
                    if (feature.Rectangle.Contains(point))
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(25, Color.Yellow)), feature.Rectangle);
                        _LastHover = feature.Name;
                        break;
                    }
                }
                PBX.Image = bitmap;
            }
        }

        private void MakeBackground()
        {
            Bitmap bitmap = new Bitmap(_Size.Width, _Size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
                foreach (Feature feature in _Features)
                    g.DrawRectangle(new Pen(Brushes.Black, 2), feature.Rectangle);
            PBX.BackgroundImage = bitmap;
        }

        /// <summary>
        /// Method for adjusting mouse pos to pictureBox set to Zoom
        /// </summary>
        /// <param name="mouse">
        /// Mouse coordinates
        /// </param>
        /// <returns>
        /// Pixel coordinates
        /// </returns>
        private Point ZoomMousePos(Point mouse)
        {
            Size pbxSize = PBX.Size;
            Size imgSize = _Size;
            float ImageAspect = imgSize.Width / (float)imgSize.Height;
            float controlAspect = pbxSize.Width / (float)pbxSize.Height;
            PointF pos = new PointF(mouse.X, mouse.Y);
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

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (Criteria.Count == 20)
            {
                MessageBox.Show("20 criterion max reached -- Try to consolidate using groups", "Criteria Wizard");
                return;
            }

            ADDING = true;
            Criteria.Add(_SelectedFeatures.ToArray());
            RefreshCombo(true);
            ADDING = false;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (ComboSelector.SelectedIndex != -1)
            {
                Criteria.RemoveAt(ComboSelector.SelectedIndex);
                RefreshCombo(false);
            } 
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ComboSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboSelector.SelectedIndex != -1 && !ADDING)
            {
                _SelectedFeatures = Criteria[ComboSelector.SelectedIndex].ToList();
                MakeForeground(Point.Empty);
            }
        }

        private void BtnCreateGroup_Click(object sender, EventArgs e)
        {
            ToggleGroupEdit(true);
        }

        private void BtnFinishGroup_Click(object sender, EventArgs e)
        {
            ToggleGroupEdit(false);
        }

        private void ToggleGroupEdit(bool grouping)
        {
            GROUPING = grouping;
            TxtGroupName.Enabled = grouping;
            BtnFinishGroup.Enabled = grouping;
            BtnAdd.Enabled = !grouping;
            BtnDelete.Enabled = !grouping;
        }

        // HSL Method adapted from https://stackoverflow.com/a/28760071
        // Increment hue by 0.05 to change colors

        private Color ColorFromHue(double h)
        {
            return Color.FromArgb(255, (int)(255 * ChFromHue(h + 1 / 3d)),
                                       (int)(255 * ChFromHue(h)),
                                       (int)(255 * ChFromHue(h - 1 / 3d)));
        }

        private double ChFromHue(double h)
        {
            double m1 = 0.25;
            double m2 = 0.75;
            h = (h + 1d) % 1d;
            if (h < 0) h += 1;
            if (h * 6 < 1) return m1 + (m2 - m1) * 6 * h;
            else if (h * 2 < 1) return m2;
            else if (h * 3 < 2) return m1 + (m2 - m1) * 6 * (2d / 3d - h);
            else return m1;
        }
    }
}
