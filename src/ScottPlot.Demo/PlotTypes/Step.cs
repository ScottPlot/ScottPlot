using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Step
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Step Plot Quickstart";
            public string description { get; } = "Step plots are really just scatter plots whose points are connected by elbows rather than straight lines.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotStep(x, sin);
                plt.PlotStep(x, cos);
            }
        }
    }
}
