using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEYR.Session
{
    internal class Project
    {
        public double PixelsPerMM;

        public Project(double pixelsPerMM)
        {
            PixelsPerMM = pixelsPerMM;
        }
    }
}
