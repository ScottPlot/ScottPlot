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
            plt.AddScatter(x, sin, label: "sin");
            plt.AddScatter(x, cos, label: "cos");

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

            plt.AddScatter(
                new double[] { 1, 2, 3, 4 },
                new double[] { -1, 1, -1, 1 },
                markerSize: 10
                );

            plt.AddScatter(
                new double[] { 1, 2, 3, 4 },
                new double[] { 1, -1, 1, -1 }
                );

            double[] xs = ScottPlot.DataGen.Range(1, 4, .02, true);
            plt.AddScatter(xs, ScottPlot.DataGen.Sin(xs.Length, 2));

            plt.YLabel("vertical units");
            plt.XLabel("horizontal units");
            plt.Title(ScottPlot.Tools.GetOsName());
            plt.Legend();

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_AxisAuto_DoesNotThrow()
        {
            // This code block is reported to throw an exception on Linux
            // https://github.com/ScottPlot/ScottPlot/issues/1431

            ScottPlot.Plot plt = new(600, 400);
            plt.AddSignal(ScottPlot.DataGen.Sin(51), label: "sin");
            plt.AddSignal(ScottPlot.DataGen.Cos(51), label: "cos");
            plt.Legend(location: ScottPlot.Alignment.UpperCenter);
            plt.YAxis.Label("vertical axis");
            plt.XAxis.Label("horizontal axis");
            plt.XAxis.TickLabelStyle(fontSize: 24);
            plt.YAxis.TickLabelStyle(fontSize: 24);
            plt.Legend(location: ScottPlot.Alignment.UpperCenter);
            plt.AxisAuto(0.05, 0.5); // <= EXCEPTION HERE?
            plt.SetAxisLimits(yMin: 0);
            TestTools.SaveFig(plt);
        }
    }
}
