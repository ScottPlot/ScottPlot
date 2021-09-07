using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    class Arrow
    {
        [Test]
        public void Test_Arrow_Render()
        {
            var plt = new ScottPlot.Plot(400, 300);
            var arrow1 = plt.AddArrow(1, 2, 3, 4);
            var arrow2 = plt.AddArrow(1 + 2, 2, 3 + 2, 4);
            arrow2.MinimumLengthPixels = 100;
            plt.SetAxisLimits(-10, 10, -10, 10);
            TestTools.SaveFig(plt);
        }
    }
}
