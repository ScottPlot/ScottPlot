using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Axis
{
    class LogAxis
    {
        [Test]
        public void Test_Log_Axis()
        {
            var plt = new ScottPlot.Plot(400, 300);

            double[] xs = ScottPlot.DataGen.Range(-10, 10, .1, true);
            double[] ys = ScottPlot.DataGen.Sin(xs, mult: 100);
            double[] logYs = ScottPlot.Tools.Log10(ys);

            foreach (double val in logYs)
                Console.WriteLine(val);

            plt.PlotScatter(xs, logYs);
            plt.YLabel("Log10(Sin(x) * 100)");
            plt.Ticks(logScaleY: true);
            TestTools.SaveFig(plt);
        }
    }
}
