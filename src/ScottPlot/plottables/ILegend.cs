using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.plottables
{
    public interface ILegend
    {
        Config.LegendItem[] GetLegendItems();
    }
}
