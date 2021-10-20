using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;
using static SEYR.DataBindings;
using static SEYR.Pipeline;

namespace SEYR
{
    [Serializable]
    public class Grid : INotifyPropertyChanged, ISerializable
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Serialization Members

        protected Grid(SerializationInfo info, StreamingContext context)
        {
            _Scale = info.GetDouble("scale");
            _Angle = info.GetDouble("angle");
            _FilterThreshold = info.GetInt32("filterThreshold");
            _NumberX = info.GetDouble("numX");
            _NumberY = info.GetDouble("numY");
            _PitchX = info.GetDouble("pitchX");
            _PitchY = info.GetDouble("pitchY");
            _Features = (List<Feature>)info.GetValue("features", _Features.GetType());
            _PatternFeature = (Feature)info.GetValue("pattern", _PatternFeature.GetType());
            _PatternBitmap = (Bitmap)info.GetValue("bmp", _PatternBitmap.GetType());
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("scale", _Scale);
            info.AddValue("angle", _Angle);
            info.AddValue("filterThreshold", _FilterThreshold);
            info.AddValue("numX", _NumberX);
            info.AddValue("numY", _NumberY);
            info.AddValue("pitchX", _PitchX);
            info.AddValue("pitchY", _PitchY);
            info.AddValue("features", _Features, _Features.GetType());
            info.AddValue("pattern", _PatternFeature, _PatternFeature.GetType());
            info.AddValue("bmp", _PatternBitmap, _PatternBitmap.GetType());
        }

        #endregion

        public Grid()
        {
            _Scale = 0.25;
            _Angle = 0.0;
            _FilterThreshold = 170;
            _NumberX = 0;
            _NumberY = 0;
            _PitchX = 200;
            _PitchY = 200;
            _Features = new List<Feature>();
        }

        private double _Scale;
        private double _Angle;
        private int _FilterThreshold;
        private double _NumberX;
        private double _NumberY;
        private double _PitchX;
        private double _PitchY;
        private List<Feature> _Features = new List<Feature>();
        private Feature _PatternFeature = new Feature(Rectangle.Empty);
        private Bitmap _PatternBitmap = new Bitmap(1, 1);
        private Feature _ActiveFeature = new Feature(Rectangle.Empty);
        private List<Tile> _Tiles = new List<Tile>();

        public void MakeTiles()
        {
            if (ImageIdx == -1) return;
            if (ImageIdx > DataHandler.Output.Count)
                for (int i = DataHandler.Output.Count; i < ImageIdx; i++)
                {
                    DataHandler.Output.Add(string.Empty);
                }
            else
                DataHandler.Output[ImageIdx - 1] = string.Empty;

            // Each tile contains one of each feature
            // The top-left tile is index 0,0
            // The bottom-right tile is index i,j
            Tiles.Clear();
            for (int i = 0; i < NumberX + 1; i++)
            {
                for (int j = 0; j < NumberY + 1; j++)
                {
                    Tile tile = new Tile(i, j);
                    foreach (Feature feature in Features)
                    {
                        Feature copy = feature.Clone();

                        copy.Rectangle = new Rectangle(
                            (int)(feature.Rectangle.X + (i * PitchX)),
                            (int)(feature.Rectangle.Y + (j * PitchY)),
                            feature.Rectangle.Width, feature.Rectangle.Height);

                        copy.Index = new Point(i, j);

                        tile.Features.Add(copy);
                    }
                    Tiles.Add(tile);
                }
            }

            foreach (Tile tile in Tiles)
                tile.Score(ImageIdx);
            Picasso.ReDraw();
        }

        public double Scale
        {
            get
            {
                return _Scale;
            }
            set
            {
                _Scale = value;
                LoadNewImage(Imaging.OriginalImage);
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public double Angle
        {
            get
            {
                return _Angle;
            }
            set
            {
                _Angle = value;
                LoadNewImage(Imaging.OriginalImage);
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public int FilterThreshold
        {
            get
            {
                return _FilterThreshold;
            }
            set
            {
                _FilterThreshold = value;
                LoadNewImage(Imaging.OriginalImage);
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public double NumberX
        {
            get
            {
                return _NumberX;
            }
            set
            {
                _NumberX = value;
                MakeTiles();
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public double NumberY
        {
            get
            {
                return _NumberY;
            }
            set
            {
                _NumberY = value;
                MakeTiles();
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public double PitchX
        {
            get
            {
                return _PitchX;
            }
            set
            {
                _PitchX = value;
                MakeTiles();
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public double PitchY
        {
            get
            {
                return _PitchY;
            }
            set
            {
                _PitchY = value;
                MakeTiles();
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public List<Feature> Features
        {
            get
            {
                return _Features;
            }
            set
            {
                _Features = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public Feature PatternFeature
        {
            get
            {
                return _PatternFeature;
            }
            set
            {
                _PatternFeature = value;
            }
        }

        public Bitmap PatternBitmap
        {
            get
            {
                return _PatternBitmap;
            }
            set
            {
                _PatternBitmap = value;
            }
        }

        public Feature ActiveFeature
        {
            get
            {
                return _ActiveFeature;
            }
            set
            {
                _ActiveFeature = value;
                if (Application.OpenForms.OfType<Composer>().Any()) LoadFeature(Application.OpenForms.OfType<Composer>().First());
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public List<Tile> Tiles
        {
            get
            {
                return _Tiles;
            }
            set
            {
                _Tiles = value;
            }
        }
    }
}
