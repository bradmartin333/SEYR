using System.Windows.Forms;

namespace SEYR
{
    public static class DataBindings
    {
        public static void LoadGrid(Composer composer)
        {
            composer.numAngle.DataBindings.Clear();
            composer.numAngle.DataBindings.Add(new Binding("Value", FileHandler.Grid, "Angle", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numFilterThreshold.DataBindings.Clear();
            composer.numFilterThreshold.DataBindings.Add(new Binding("Value", FileHandler.Grid, "FilterThreshold", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numCopyX.DataBindings.Clear();
            composer.numCopyX.DataBindings.Add(new Binding("Value", FileHandler.Grid, "NumberX", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numCopyY.DataBindings.Clear();
            composer.numCopyY.DataBindings.Add(new Binding("Value", FileHandler.Grid, "NumberY", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numCopyPitchX.DataBindings.Clear();
            composer.numCopyPitchX.DataBindings.Add(new Binding("Value", FileHandler.Grid, "PitchX", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numCopyPitchY.DataBindings.Clear();
            composer.numCopyPitchY.DataBindings.Add(new Binding("Value", FileHandler.Grid, "PitchY", false, DataSourceUpdateMode.OnPropertyChanged));

            if (!FileHandler.Grid.PatternFeature.Rectangle.IsEmpty)
                composer.followerPatternNameToolStripMenuItem.Text = FileHandler.Grid.PatternFeature.Name;
            else
                composer.followerPatternNameToolStripMenuItem.Text = @"N\A";

            composer.LoadComboBox();
        }

        public static void LoadFeature(Composer composer)
        {
            composer.numOriginX.DataBindings.Clear();
            composer.numOriginX.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "OriginX", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numOriginY.DataBindings.Clear();
            composer.numOriginY.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "OriginY", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numSizeX.DataBindings.Clear();
            composer.numSizeX.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "SizeX", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numSizeY.DataBindings.Clear();
            composer.numSizeY.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "SizeY", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numPassScore.DataBindings.Clear();
            composer.numPassScore.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "PassScore", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numPassTol.DataBindings.Clear();
            composer.numPassTol.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "PassTol", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numFailScore.DataBindings.Clear();
            composer.numFailScore.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "FailScore", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numFailTol.DataBindings.Clear();
            composer.numFailTol.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "FailTol", false, DataSourceUpdateMode.OnPropertyChanged));

            composer.numAlignTol.DataBindings.Clear();
            composer.numAlignTol.DataBindings.Add(new Binding("Value", FileHandler.Grid.ActiveFeature, "AlignTol", false, DataSourceUpdateMode.OnPropertyChanged));

            FileHandler.Grid.MakeTiles();
        }
    }
}
