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
                DataHandler.OutputString += string.Format("{0}\t{1}\t{2}\t{3}\t{4}\n", ImageIdx, Index.X, Index.Y, feature.Name, feature.State.ToString());
            }
        }
    }
}
