using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class Mouse
    {
        // mouse tracking
        public Point downLoc = new Point(0, 0);
        public double[] downLimits = new double[4];

        // mouse middle-click-zooming
        public Point downMiddle = new Point(0, 0);
        public Point currentLoc = new Point(0, 0);
        public bool rectangleIsHappening = false;
    }
}
