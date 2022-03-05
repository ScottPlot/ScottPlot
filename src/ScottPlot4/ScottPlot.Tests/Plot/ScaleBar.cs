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
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51, mult: 1.5));

            plt.AddScaleBar(5, .25, "5 ms", "250 pA");

            plt.Grid(false);
            plt.Frameless();
            plt.XAxis.Ticks(false);
            plt.YAxis.Ticks(false);
            plt.AxisAuto(0);
            plt.Frameless();

            TestTools.SaveFig(plt);
        }
    }
}
