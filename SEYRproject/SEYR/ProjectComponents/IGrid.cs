namespace SEYR.ProjectComponents
{
    internal interface IGrid
    {
        int OriginX { get; set; }
        int OriginY { get; set; }
        int PitchX { get; set; }
        int PitchY { get; set; }
        int SizeX { get; set; }
        int SizeY { get; set; }
        int Rows { get; set; }
        int Columns { get; set; }
    }
}
