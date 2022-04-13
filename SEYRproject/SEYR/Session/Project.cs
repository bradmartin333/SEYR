﻿using System;
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
        [XmlElement("Threshold")]
        public float Threshold { get; set; } = 0.5f;
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
        [XmlElement("Density")]
        public int Density { get; set; } = 3;
        [XmlElement("Contrast")]
        public double Contrast { get; set; } = 0.25;
        [XmlElement("Score")]
        public int Score { get; set; } = 100;
        [XmlElement("Tolerance")]
        public int Tolerance { get; set; } = 50;

        public Rectangle GetGeometry()
        {
            Point offset = new Point((int)(Channel.Project.ScaledPixelsPerMicron * Channel.Project.OriginX),
                (int)(ImageHeight - (Channel.Project.ScaledPixelsPerMicron * Channel.Project.OriginY)));
            Point size = new Point((int)(Channel.Project.SizeX * Channel.Project.ScaledPixelsPerMicron),
                (int)(Channel.Project.SizeY * Channel.Project.ScaledPixelsPerMicron));
            // Need the width to be a factor of 12 to crop with Bitmap data
            Rectangle rectangle = new Rectangle(offset.X, offset.Y, ((int)Math.Round(size.X / 12.0)) * 12, size.Y);
            return rectangle;
        }

        public Size GetScanSize(Size size)
        {
            return new Size((int)Math.Round(size.Width / (double)Channel.Project.Density, MidpointRounding.AwayFromZero),
                (int)Math.Round(size.Height / (double)Channel.Project.Density, MidpointRounding.AwayFromZero));
        }
    }
}