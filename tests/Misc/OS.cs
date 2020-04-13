using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlotTests.Misc
{
    public class OS
    {
        [Test]
        public void Test_Plot_Basic()
        {
            int pointCount = 51;
            double[] x = ScottPlot.DataGen.Consecutive(pointCount);
            double[] sin = ScottPlot.DataGen.Sin(pointCount);
            double[] cos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotScatter(x, sin, label: "sin");
            plt.PlotScatter(x, cos, label: "cos");

            plt.YLabel("vertical units");
            plt.XLabel("horizontal units");

            plt.Title(ScottPlot.Tools.GetOsName());
            plt.Legend();

            TestTools.SaveFig(plt, artifact: true);
        }

        [Test]
        public void Test_Marker_Precision()
        {
            var plt = new ScottPlot.Plot();

            plt.PlotScatter(
                new double[] { 1, 2, 3, 4 },
                new double[] { -1, 1, -1, 1 },
                markerSize: 10
                );

            plt.PlotScatter(
                new double[] { 1, 2, 3, 4 },
                new double[] { 1, -1, 1, -1 }
                );

            double[] xs = ScottPlot.DataGen.Range(1, 4, .02, true);
            plt.PlotScatter(xs, ScottPlot.DataGen.Sin(xs.Length, 2));

            plt.YLabel("vertical units");
            plt.XLabel("horizontal units");
            plt.Title(ScottPlot.Tools.GetOsName());
            plt.Legend();

            TestTools.SaveFig(plt);
        }
    }
}
