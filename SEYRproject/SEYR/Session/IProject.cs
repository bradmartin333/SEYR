namespace SEYR.Session
{
    public interface IProject
    {
        float Angle { get; set; }
        int Columns { get; set; }
        int Density { get; set; }
        int OriginX { get; set; }
        int OriginY { get; set; }
        int PitchX { get; set; }
        int PitchY { get; set; }
        int Rows { get; set; }
        float Contrast { get; set; }
        float Scaling { get; set; }
        int SizeX { get; set; }
        int SizeY { get; set; }
        float Threshold { get; set; }
        float Score { get; set; }
        float Tolerance { get; set; }
    }
}