using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Style
{
    class Legend
    {
        public class CustomLegend : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Legend";
            public string description { get; } = "A legend is available to display data that was plotted using the 'label' argument. Arguments for Legend() allow the user to define its position.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin, label: "sin");
                plt.PlotScatter(x, cos, label: "cos");
                plt.Legend();
            }
        }
    }
}
