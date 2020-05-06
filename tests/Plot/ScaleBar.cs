using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Plot
{
    class ScaleBar
    {
        [Test]
        public void Test_ScaleBar_Simple()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotSignal(DataGen.Sin(51));
            plt.PlotSignal(DataGen.Cos(51, mult: 1.5));

            plt.PlotScaleBar(5, .25, "5 ms", "250 pA");

            plt.Grid(false);
            plt.Frame(false);
            plt.Ticks(false, false);
            plt.AxisAuto(0);
            plt.TightenLayout(0);

            TestTools.SaveFig(plt);
        }
    }
}
