using SEYR.Properties;
using System.Windows.Forms;

namespace SEYR
{
    public static class Pipeline
    {
        public static PictureBox PBX = new PictureBox()
        {
            BackgroundImage = Resources.SEYR,
            BackgroundImageLayout = ImageLayout.Zoom,
            Dock = DockStyle.Fill,
            Image = Resources.SEYR,
            SizeMode = PictureBoxSizeMode.Zoom
        };
    }
}
