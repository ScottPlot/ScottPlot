using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Axis
{
    class RotatedTicks
    {
        [Test]
        public void Test_Rotated_AllAxes()
        {
            var plt = new ScottPlot.Plot(400, 300);

            plt.XAxis2.Ticks(true);
            plt.YAxis2.Ticks(true);
            TestTools.SaveFig(plt, "1");

            plt.XAxis.TickLabelStyle(rotation: 45);
            plt.XAxis2.TickLabelStyle(rotation: 45);
            plt.YAxis.TickLabelStyle(rotation: 45);
            plt.YAxis2.TickLabelStyle(rotation: 45);
            TestTools.SaveFig(plt, "2");

            plt.XAxis.TickLabelStyle(rotation: 90);
            plt.XAxis2.TickLabelStyle(rotation: 90);
            plt.YAxis.TickLabelStyle(rotation: 90);
            plt.YAxis2.TickLabelStyle(rotation: 90);
            TestTools.SaveFig(plt, "3");
        }

        [Test]
        public void Test_Ruler_AllAxes()
        {
            var plt = new ScottPlot.Plot(400, 300);

            plt.XAxis2.Ticks(true);
            plt.YAxis2.Ticks(true);

            plt.XAxis.RulerMode(true);
            plt.XAxis2.RulerMode(true);
            plt.YAxis.RulerMode(true);
            plt.YAxis2.RulerMode(true);

            TestTools.SaveFig(plt);
        }
    }
}
