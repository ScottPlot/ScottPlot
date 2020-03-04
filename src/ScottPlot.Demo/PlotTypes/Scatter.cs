using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Scatter
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Scatter Plot Quickstart";
            public string description { get; } = "Scatter plots are best for small numbers of paired X/Y data points. For evenly-spaced data points Signal is much faster.";

            public void Render(Plot plt)
            {
                int pointCount = 50;
                double[] dataXs = DataGen.Consecutive(pointCount);
                double[] dataSin = DataGen.Sin(pointCount);
                double[] dataCos = DataGen.Cos(pointCount);

                plt.PlotScatter(dataXs, dataSin);
                plt.PlotScatter(dataXs, dataCos);
                plt.Title("ScottPlot Quickstart");
                plt.XLabel("Time (seconds)");
                plt.YLabel("Potential (V)");
            }
        }
    }
}
