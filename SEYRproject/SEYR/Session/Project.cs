using System;

namespace SEYR.Session
{
    [Serializable()]
    public class Project
    {
        [System.Xml.Serialization.XmlElement("PixelsPerMM")]
        public double PixelsPerMM { get; set; }
    }
}
