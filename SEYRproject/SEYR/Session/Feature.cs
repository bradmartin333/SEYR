using System;
using System.Drawing;
using System.Xml.Serialization;

namespace SEYR.Session
{
    [Serializable()]
    public class Feature
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Rectangle")]
        public Rectangle Rectangle { get; set; }
        [XmlElement("PassScore")]
        public float PassScore { get; set; } = 100f;
        [XmlElement("PassTolerance")]
        public float PassTolerance { get; set; } = 50f;
        [XmlElement("FailScore")]
        public float FailScore { get; set; } = 0f;
        [XmlElement("FailTolerance")]
        public float FailTolerance { get; set; } = 20f;
        [XmlElement("Threshold")]
        public float Threshold { get; set; } = 20f;
    }
}
