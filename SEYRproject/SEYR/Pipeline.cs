using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEYR
{
    public class Pipeline
    {
        public Composer Composer = new Composer();
        public int PatternFollowInterval
        {
            get {  return Composer.PatternFollowInterval; }
            set { Composer.PatternFollowInterval = value; }
        }

        
    }
}
