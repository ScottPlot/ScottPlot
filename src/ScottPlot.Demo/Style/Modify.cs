using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.Style
{
    class Modify
    {
        public class ModifyAfterPlot : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Modify styles after plotting";
            public string description { get; } = "Styles are typically defined as arguments when data is initially plotted. However, plotting functions return objects which contain style information that can be modified after it has been plotted. In some cases these properties allow more extensive customization than the initial function arguments.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                var plot1 = plt.PlotScatter(x, sin);
                var plot2 = plt.PlotScatter(x, cos);

                plot1.lineWidth = 5;
                plot1.markerShape = MarkerShape.openCircle;
                plot1.markerSize = 20;

                plot2.color = Color.Magenta;
            }
        }
    }
}
