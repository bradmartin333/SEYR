using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;

namespace SEYR.Session
{
    [Serializable()]
    public class Feature
    {
        public enum NullDetectionTypes
        {
            [Display(Name = "None")]
            None,
            [Display(Name = "Include Empty")]
            IncludeEmpty,
            [Display(Name = "Include Filled")]
            IncludeFilled,
            [Display(Name = "Include Both")]
            IncludeBoth,
        }

        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Rectangle")]
        public Rectangle Rectangle { get; set; } = new Rectangle(10, 10, 10, 10);
        [XmlElement("Threshold")]
        public float Threshold { get; set; } = 20f;
        [XmlElement("NullDetection")]
        public NullDetectionTypes NullDetection { get; set; } = NullDetectionTypes.None;

        private List<float> Scores = new List<float>();

        public Feature()
        {
            Name = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(); // Random string
        }

        public Rectangle GetGeometry()
        {
            Point offset = new Point((int)(Channel.Project.ScaledPixelsPerMicron * Rectangle.X),
                (int)(Channel.Project.ScaledPixelsPerMicron * Rectangle.Y));
            Point size = new Point((int)(Channel.Project.ScaledPixelsPerMicron * Rectangle.Width),
                (int)(Channel.Project.ScaledPixelsPerMicron * Rectangle.Height));
            return new Rectangle(offset.X, offset.Y, size.X, size.Y);
        }

        public Feature Clone(bool userClone = false)
        {
            return new Feature()
            {
                Name = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(), // Random string
                Rectangle = new Rectangle(Rectangle.X + 5, Rectangle.Y + 5, Rectangle.Width, Rectangle.Height),
                Threshold = Threshold,
                NullDetection = NullDetection,
            };
        }

        public static string[] GetDisplayNames()
        {
            Type type = typeof(NullDetectionTypes);
            var displaynames = new List<string>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DisplayAttribute), true);
                if (fds.Length == 0) displaynames.Add(name);
                foreach (DisplayAttribute fd in fds)
                    displaynames.Add(fd.Name);
            }
            return displaynames.ToArray();
        }

        internal void ResetScore()
        {
            Scores.Clear();
        }

        internal void AddScore(float score)
        {
            Scores.Add(score);
        }

        internal double GetMinScore()
        {
            return Scores.Min() - 1;
        }

        internal double GetMaxScore()
        {
            return Scores.Max() + 1;
        }
    }
}
