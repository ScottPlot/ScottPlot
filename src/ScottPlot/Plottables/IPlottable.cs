using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot.Plottables
{
    public interface IPlottable
    {
        bool visible { get; set; }
        int pointCount { get; }
        string label { get; set; }

        Color color { get; set; }
        MarkerShape markerShape { get; set; }
        LineStyle lineStyle { get; set; }

        void Render(Plottables.Context renderContext);
        Config.AxisLimits2D GetLimits();
        string ToString();
    }
}