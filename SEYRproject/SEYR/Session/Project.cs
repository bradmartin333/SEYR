using SEYR.ProjectComponents;
using System;

namespace SEYR.Session
{
    [Serializable()]
    public class Project : IFilters
    {
        [System.Xml.Serialization.XmlElement("PixelsPerMM")]
        public double PixelsPerMM { get; set; } = 5.212e-3;
        public double Scaling { get; set; } = 0.25;
        public int Threshold { get; set; } = 128;
        public float Angle { get; set; } = 0;
    }
}