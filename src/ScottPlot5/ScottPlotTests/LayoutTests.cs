using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests
{
    class LayoutTests
    {
        [Test]
        public void Test_Layout_ResizesForBigLabels()
        {
            var plt = new Plot();
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);
            plt.PlotScatter(xs, ys);
            plt.Scale(0, 50, -1, 1);
            TestTools.SaveFig(plt);
        }
    }
}
