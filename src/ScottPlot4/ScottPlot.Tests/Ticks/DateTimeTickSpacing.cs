using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Ticks
{
    class DateTimeTickSpacing
    {
        [Test]
        public void Test_Spacing_MonthTicks()
        {
            int pointCount = 650;
            var rand = new Random(0);
            double[] values = ScottPlot.DataGen.RandomWalk(rand, pointCount);

            var plt = new ScottPlot.Plot(600, 400);
            var sig = plt.AddSignal(values);
            sig.OffsetX = new DateTime(1985, 1, 1).ToOADate();
            plt.XAxis.DateTimeFormat(true);
            TestTools.SaveFig(plt);
        }
    }
}
