using System.Collections.Generic;
using System.Drawing;

namespace SEYR.Session
{
    public interface IProject
    {
        float Angle { get; set; }
        int Columns { get; set; }
        int OriginX { get; set; }
        int OriginY { get; set; }
        int PitchX { get; set; }
        int PitchY { get; set; }
        int Rows { get; set; }
        float Scaling { get; set; }
        int SizeX { get; set; }
        int SizeY { get; set; }
        float PatternScore { get; set; }
        string PatternIntervalString { get; set; }
        int PatternIntervalValue { get; set; }
        int PatternDeltaMax { get; set; }
        List<Point> PatternLocations { get; set; }
        List<Feature> Features { get; set; }
    }
}