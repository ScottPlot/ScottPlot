using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public abstract class Plottable
    {
        public bool visible = true;
        public abstract void Render(Settings settings);
        public abstract override string ToString();
        public abstract Config.AxisLimits2D GetLimits();
        public abstract int GetPointCount();
        public abstract Config.LegendItem[] GetLegendItems();
    }
}
