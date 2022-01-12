using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SEYR
{
    [Serializable]
    public class Feature : INotifyPropertyChanged, ISerializable
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Serialization Members

        protected Feature(SerializationInfo info, StreamingContext context)
        {
            _Name = info.GetString("name");
            _Rectangle = (Rectangle)info.GetValue("rect", _Rectangle.GetType());
            _PassScore = info.GetDouble("passScore");
            _PassTol = info.GetDouble("passTol");
            _FailScore = info.GetDouble("failScore");
            _FailTol = info.GetDouble("failTol");
            _WeightedCenter = (Point)info.GetValue("weightedCenter", _WeightedCenter.GetType());
            _AlignTol = info.GetInt32("alignTol");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", _Name);
            info.AddValue("rect", _Rectangle);
            info.AddValue("passScore", _PassScore);
            info.AddValue("passTol", _PassTol);
            info.AddValue("failScore", _FailScore);
            info.AddValue("failTol", _FailTol);
            info.AddValue("weightedCenter", _WeightedCenter);
            info.AddValue("alignTol", _AlignTol);
        }

        #endregion

        public Feature(Rectangle rect)
        {
            _Name = string.Format("{0}, {1}", rect.X, rect.Y);
            _Rectangle = rect;
            _PassScore = 100.0;
            _PassTol = 50.0;
            _FailScore = 2.0;
            _FailTol = 20.0;
            _AlignTol = 0;
        }

        public bool Equals(Feature feature)
        {
            if (feature == null) return false;
            return _Name.Equals(feature._Name) &&
                _PassScore.Equals(feature._PassScore) &&
                _PassTol.Equals(feature._PassTol) &&
                _FailScore.Equals(feature._FailScore) &&
                _FailTol.Equals(feature._FailTol);
        }

        public bool Valid()
        {
            Point[] corners = new Point[]
            {
                new Point(_Rectangle.Left, _Rectangle.Top),
                new Point(_Rectangle.Left, _Rectangle.Bottom),
                new Point(_Rectangle.Right, _Rectangle.Top),
                new Point(_Rectangle.Right, _Rectangle.Bottom)
            };

            Rectangle rect = new Rectangle(0, 0, Picasso.ThisSize.Width, Picasso.ThisSize.Height);

            foreach (Point corner in corners)
            {
                if (!rect.Contains(corner)) return false;
            }
            return true;
        }

        /// <summary>
        /// Return the copy of a feature
        /// Optionally append "Clone" to the name
        /// </summary>
        /// <param name="userClone"></param>
        /// <returns></returns>
        public Feature Clone(bool userClone = false)
        {
            return new Feature(_Rectangle)
            {
                Name = _Name + (userClone ? " Clone" : ""),
                PassScore = _PassScore,
                PassTol = _PassTol,
                FailScore = _FailScore,
                FailTol = _FailTol,
                WeightedCenter = _WeightedCenter,
                AlignTol = AlignTol
            };
        }

        public void CopyActiveScoreAndTolerance()
        {
            Feature active = FileHandler.Grid.ActiveFeature;
            _PassScore = active.PassScore;
            _PassTol = active.PassTol;
            _FailScore = active.FailScore;
            _FailTol = active.FailTol;
        }

        #region NonSerialized Members

        public Point Index { get; set; }
        public double Score { get; set; }
        public DataHandler.State State { get; set; }

        public Rectangle OffsetRectangle
        {
            get
            {
                return new Rectangle(
                    _Rectangle.X - Picasso.Offset.X, 
                    _Rectangle.Y - Picasso.Offset.Y, 
                    _Rectangle.Width, 
                    _Rectangle.Height
                );
            }
        }

        public int OriginX
        {
            get
            {
                return _Rectangle.X;
            }
            set
            {
                _Rectangle.X = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public int OriginY
        {
            get
            {
                return _Rectangle.Y;
            }
            set
            {
                _Rectangle.Y = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public int SizeX
        {
            get
            {
                return _Rectangle.Width;
            }
            set
            {
                _Rectangle.Width = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public int SizeY
        {
            get
            {
                return _Rectangle.Height;
            }
            set
            {
                _Rectangle.Height = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public bool CheckAlign
        {
            get
            {
                return !_WeightedCenter.IsEmpty;
            }
        }

        #endregion

        #region Serialized Members

        private string _Name;
        private Rectangle _Rectangle;
        private double _PassScore;
        private double _PassTol;
        private double _FailScore;
        private double _FailTol;
        private Point _WeightedCenter = Point.Empty;
        private int _AlignTol;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return _Rectangle;
            }
            set
            {
                _Rectangle = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public double PassScore
        {
            get
            {
                return _PassScore;
            }
            set
            {
                _PassScore = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public double PassTol
        {
            get
            {
                return _PassTol;
            }
            set
            {
                _PassTol = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public double FailScore
        {
            get
            {
                return _FailScore;
            }
            set
            {
                _FailScore = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public double FailTol
        {
            get
            {
                return _FailTol;
            }
            set
            {
                _FailTol = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public Point WeightedCenter
        {
            get
            {
                return _WeightedCenter;
            }
            set
            {
                _WeightedCenter = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        public int AlignTol
        {
            get
            {
                return _AlignTol;
            }
            set
            {
                _AlignTol = value;
                if (PropertyChanged != null) NotifyPropertyChanged();
            }
        }

        #endregion
    }
}
