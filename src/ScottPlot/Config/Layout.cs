using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class Layout
    {
        // settings here relate to positioning of objects on the figure

        public int padOnAllSides = 5; // useful for intentionally adding spacing
        public int[] paddingBySide = new int[] { 5, 5, 5, 5 }; // X1, X2, Y1, Y2

        public bool displayAxisFrames = true;
        public bool[] displayFrameByAxis = new bool[] { true, true, true, true };

        public bool tighteningOccurred = false;
    }
}
