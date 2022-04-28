using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SEYR.Session
{
    public partial class CriteriaWizard : Form
    {
        public readonly List<Feature[]> Criteria = new List<Feature[]>();

        private Size _Size;
        private readonly List<Feature> _Features;
        private List<Feature> _SelectedFeatures = new List<Feature>();
        private Feature _LastHover = null;
        private bool ADDING = false;

        public CriteriaWizard(Size size, List<Feature> features, List<Feature[]> criteria)
        {
            InitializeComponent();
            _Size = size;
            _Features = features;
            Criteria = criteria;
            PBX.MouseMove += PBX_MouseMove;
            PBX.MouseUp += PBX_MouseUp;
            MakeBackground();
        }

        private void RefreshCombo(bool select)
        {
            _SelectedFeatures.Clear();
            ComboSelector.Items.Clear();
            ComboSelector.Text = "";
            MakeForeground(Point.Empty);
            
            List<string> names = new List<string>();
            foreach (Feature[] criterion in Criteria)
                names.Add(string.Join(", ", criterion.Select(x => x.Name).ToArray()));
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
            Bitmap bitmap = new Bitmap(_Size.Width, _Size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                foreach (Feature selectedFeature in _SelectedFeatures)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(75, Color.Green)), selectedFeature.Rectangle);
                foreach (Feature feature in _Features)
                    if (feature.Rectangle.Contains(point) && (_LastHover != feature || _LastHover.Rectangle.Contains(feature.Rectangle)))
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(25, Color.Yellow)), feature.Rectangle);
                        _LastHover = feature;
                        break;
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
    }
}
