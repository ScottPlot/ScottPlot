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
            var plt = new ScottPlot.Plot();
            plt.PlotSignal(ScottPlot.DataGen.Sin(100), label: "sin");
            plt.PlotSignal(ScottPlot.DataGen.Cos(100), label: "cos");
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
                new double[] { 1, 2, 1, 2 },
                markerSize: 10
                );

            plt.PlotScatter(
                new double[] { 1, 2, 3, 4 },
                new double[] { 2, 1, 2, 1 }
                );

            plt.YLabel("vertical units");
            plt.XLabel("horizontal units");
            plt.Title(ScottPlot.Tools.GetOsName());
            plt.Legend();

            TestTools.SaveFig(plt, artifact: true);
        }
    }
}
