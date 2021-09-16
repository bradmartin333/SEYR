using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SEYR
{
    static class FileHandler
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

        public static string SaveFile()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Title = "Save Simple Entropy Yield Routine";
                saveFileDialog.Filter = "SEYR (*.seyr) | *.seyr";
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
            using (Stream stream = File.Open(FilePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                Grid = (Grid)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
