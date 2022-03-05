using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.PlottableRenderTests
{
    class ScatterMutable
    {
        [Test]
        public void Test_Scatter_Mutable()
        {
            var plt = new ScottPlot.Plot();
            var scatter = new ScottPlot.Plottable.ScatterPlotList<double>();
            plt.Add(scatter);

            TestTools.SaveFig(plt, "no_points");

            scatter.Add(1, 1);
            plt.AxisAuto();
            TestTools.SaveFig(plt, "one_point");

            scatter.Add(2, 2);
            plt.AxisAuto();
            TestTools.SaveFig(plt, "two_points");

            scatter.AddRange(new double[] { 3, 4, 5 }, new double[] { 1, 6, 3 });
            plt.AxisAuto();
            TestTools.SaveFig(plt, "many_points");
        }
    }
}
