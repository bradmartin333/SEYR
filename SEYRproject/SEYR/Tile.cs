using System.Collections.Generic;
using System.Drawing;

namespace SEYR
{
    public class Tile
    {
        public List<Feature> Features { get; set; }
        public Point Index { get; set; }

        public Tile(int i, int j)
        {
            Features = new List<Feature>();
            Index = new Point(i, j);
        }

        public void Score(int ImageIdx)
        {
            foreach (Feature feature in Features)
            {
                Imaging.Score(feature);
                DataHandler.Append(
                    string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\n",
                    ImageIdx,
                    Index.X,
                    Index.Y,
                    feature.Name,
                    feature.State.ToString(),
                    Pipeline.RR,
                    Pipeline.RC,
                    Pipeline.R,
                    Pipeline.C,
                    Pipeline.X,
                    Pipeline.Y)
                );
            }
            DataHandler.NextImage();
        }
    }
}
