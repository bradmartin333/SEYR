using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace SEYR.Session
{
    [Serializable()]
    public class Project : IProject
    {
        [XmlElement("ImageHeight")]
        public int ImageHeight { get; set; } = 1;
        [XmlElement("ImageWidth")]
        public int ImageWidth { get; set; } = 1;
        [XmlElement("PixelsPerMicron")]
        public double PixelsPerMicron { get; set; } = 2.606;
        [XmlElement("ScaledPixelsPerMicron")]
        public double ScaledPixelsPerMicron { get; set; } = 0.6515;
        [XmlElement("Scaling")]
        public float Scaling { get; set; } = 0.25f;
        [XmlElement("Angle")]
        public float Angle { get; set; } = 0;
        [XmlElement("OrginX")]
        public int OriginX { get; set; } = 10;
        [XmlElement("OriginY")]
        public int OriginY { get; set; } = 10;
        [XmlElement("PitchX")]
        public int PitchX { get; set; } = 10;
        [XmlElement("PitchY")]
        public int PitchY { get; set; } = 10;
        [XmlElement("Rows")]
        public int Rows { get; set; } = 1;
        [XmlElement("Columns")]
        public int Columns { get; set; } = 1;
        [XmlElement("SizeX")]
        public int SizeX { get; set; } = 10;
        [XmlElement("SizeY")]
        public int SizeY { get; set; } = 10;
        [XmlElement("PatternScore")]
        public float PatternScore { get; set; } = 0.95f;
        [XmlArray("Features")]
        public List<Feature> Features { get; set; } = new List<Feature>();
        
        public Rectangle GetGeometry()
        {
            Point offset = new Point((int)(Channel.Project.ScaledPixelsPerMicron * Channel.Project.OriginX),
                (int)(Channel.Project.ScaledPixelsPerMicron * Channel.Project.OriginY));
            Point size = new Point((int)(Channel.Project.ScaledPixelsPerMicron * Channel.Project.SizeX),
                (int)(Channel.Project.ScaledPixelsPerMicron * Channel.Project.SizeY));
            return new Rectangle(offset.X, offset.Y, size.X, size.Y);
        }
    }
}