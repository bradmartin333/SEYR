using System;
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
        public double Scaling { get; set; } = 0.25;
        [XmlElement("Threshold")]
        public int Threshold { get; set; } = 128;
        [XmlElement("Angle")]
        public float Angle { get; set; } = 0;
        [XmlElement("OrginX")]
        public int OriginX { get; set; } = 10;
        [XmlElement("OriginY")]
        public int OriginY { get; set; } = 10;
        [XmlElement("PitchX")]
        public int PitchX { get; set; } = 100;
        [XmlElement("PitchY")]
        public int PitchY { get; set; } = 100;
        [XmlElement("Rows")]
        public int Rows { get; set; } = 1;
        [XmlElement("Columns")]
        public int Columns { get; set; } = 1;
        [XmlElement("SizeX")]
        public int SizeX { get; set; } = 500;
        [XmlElement("SizeY")]
        public int SizeY { get; set; } = 500;
        [XmlElement("Density")]
        public int Density { get; set; } = 3;
        [XmlElement("Sampling")]
        public double Sampling { get; set; } = 0.25;

        public Rectangle GetGeometry()
        {
            Point offset = new Point((int)(Channel.Project.ScaledPixelsPerMicron * Channel.Project.OriginX),
                (int)(ImageHeight - (Channel.Project.ScaledPixelsPerMicron * Channel.Project.OriginY)));
            Point size = new Point((int)(Channel.Project.SizeX * Channel.Project.ScaledPixelsPerMicron),
                (int)(Channel.Project.SizeY * Channel.Project.ScaledPixelsPerMicron));
            Rectangle rectangle = new Rectangle(offset.X, offset.Y, size.X, size.Y);
            return rectangle;
        }
    }
}