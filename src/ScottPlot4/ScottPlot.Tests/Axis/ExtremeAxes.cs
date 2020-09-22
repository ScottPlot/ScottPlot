using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Axis
{
    class ExtremeAxes
    {
        [Test]
        public void Test_Axis_VeryBigNumbers()
        {
            var plt = new ScottPlot.Plot();

            plt.PlotSignal(DataGen.Sin(51));
            plt.PlotSignal(DataGen.Cos(51));

            plt.Axis(y1: -10e50, y2: 10e50);
            plt.TightenLayout();

            TestTools.SaveFig(plt);
        }
    }
}
