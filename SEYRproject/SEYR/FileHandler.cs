using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEYR
{
    public static class FileHandler
    {
        public static Grid Grid = new Grid();
        public static string FilePath { get; set; } = string.Empty;

        public static string LoadFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open Simple Entropy Yield Routine";
                openFileDialog.Filter = "SEYR (*.seyr) | *.seyr";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    return openFileDialog.FileName;
            }
            return null;
        }

        public static string SaveFile(string title, string filter)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Title = title;
                saveFileDialog.Filter = filter;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    return saveFileDialog.FileName;
            }
            return null;
        }

        public static void WriteParametersToBinaryFile()
        {
            using (Stream stream = File.Open(FilePath, false ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, Grid);
            }
        }

        public static void ReadParametersFromBinaryFile()
        {
            try
            {
                using (Stream stream = File.Open(FilePath, FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    Grid = (Grid)binaryFormatter.Deserialize(stream);
                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("Invalid SEYR File");
                Grid = new Grid();
                return;
            }
        }

        public static void CopyCurrentGridForExcel()
        {
            if (Grid.Features.Count > 1 || Grid.NumberX < 1 || Grid.NumberY < 1)
            {
                MessageBox.Show(
                    text: "This option is only available for grids with a single feature.",
                    caption: "Copy Plot For Excel");
                return;
            }

            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < Grid.NumberY + 1; j++)
            {
                for (int i = 0; i < Grid.NumberX + 1; i++)
                {
                    int state = (int)Grid.Tiles.Where(x => x.Index == new Point(i, j)).First().Features.First().State;
                    sb.Append($"{state}\t");
                }
                sb.Append('\n');
            }
            Clipboard.SetText(sb.ToString());
        }
    }
}
