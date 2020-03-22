using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public abstract class Plottable
    {
        // in ScottPlot 3.1 this class will move to ScottPlot.Plottable.IPlottable
        public bool useParallel = false;
        public bool visible = true;
        public abstract void Render(Settings settings);
        public abstract override string ToString();
        public abstract Config.AxisLimits2D GetLimits();
        public abstract int GetPointCount();
    }
}