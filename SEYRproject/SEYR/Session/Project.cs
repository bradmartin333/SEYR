using SEYR.ProjectComponents;
using System;
using System.Xml.Serialization;

namespace SEYR.Session
{
    [Serializable()]
    public class Project : IFilters, IGrid, ITile
    {
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
        public int Density { get; set; } = 100;
    }
}