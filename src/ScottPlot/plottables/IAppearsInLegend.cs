using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    public interface IAppearsInLegend
    {
        Config.LegendItem[] GetLegendItems();
    }
}
