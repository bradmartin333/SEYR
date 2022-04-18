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
        public float Threshold { get; set; } = 0.2f;
        [XmlElement("NullDetection")]
        public NullDetectionTypes NullDetection { get; set; } = NullDetectionTypes.None;

        private List<float> _Scores = new List<float>();
        internal List<float> Scores { get => _Scores; set => _Scores = value; }
        
        
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

        public Feature Clone()
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

        internal (float, float[]) GetScoreInfo()
        {
            float[] theseScores = (float[])Scores.ToArray().Clone();
            if (theseScores.Length > 0)
                return (theseScores.Last(), theseScores);
            else
                return (0f, new float[] { 0f });
        }

        public Color ColorFromScore(double value = 1, double saturation = 1, byte opacity = 255)
        {
            (float hueIn, float[] scores) = GetScoreInfo();
            double fromLow;
            double fromHigh;
            float[] cleanScores = scores.Where(x => x > 0f && x < 1000f).ToArray();
            if (cleanScores.Count() > 5)
            {
                fromLow = cleanScores.Min();
                fromHigh = cleanScores.Max();
            }
            else
                return Color.Black;
            double toLow = 255;
            double toHigh = 0;
            double hue = (double)((hueIn - fromLow) * (toHigh - toLow) / (fromHigh - fromLow)) + toLow;
            if (hue == 360) return Color.FromArgb(255, Color.White);
            if (hue == 0) return Color.FromArgb(255, Color.Black);
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = (hue / 60) - Math.Floor(hue / 60);
            value *= 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - (f * saturation)));
            int t = Convert.ToInt32(value * (1 - ((1 - f) * saturation)));
            if (hi == 0)
                return Color.FromArgb(opacity, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(opacity, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(opacity, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(opacity, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(opacity, t, p, v);
            else
                return Color.FromArgb(opacity, v, p, q);
        }
    }
}
