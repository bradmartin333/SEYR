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
        public float PixelsPerMicron { get; set; } = 2.606f;
        [XmlElement("ScaledPixelsPerMicron")]
        public float ScaledPixelsPerMicron { get; set; } = 0.6515f;
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
        [XmlElement("ComposerLocation")]
        public Point ComposerLocation { get; set; } = Point.Empty;
        [XmlElement("ComposerSize")]
        public Size ComposerSize { get; set; } = Size.Empty;
        [XmlElement("ViewerLocation")]
        public Point ViewerLocation { get; set; } = Point.Empty;
        [XmlElement("ViewerSize")]
        public Size ViewerSize { get; set; } = Size.Empty;
        [XmlElement("PatternScore")]
        public float PatternScore { get; set; } = 0.95f;
        [XmlElement("PatternIntervalString")]
        public string PatternIntervalString { get; set; } = "null";
        [XmlElement("PatternIntervalValue")]
        public int PatternIntervalValue { get; set; } = 0;
        [XmlElement("PatternDeltaMax")]
        public int PatternDeltaMax { get; set; } = 10;
        [XmlArray("PatternLocations")]
        public List<Point> PatternLocations { get; set; } = new List<Point>();
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