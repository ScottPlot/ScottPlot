using System;
using System.Linq;
using NUnit.Framework;
using ScottPlotTests;

namespace ScottPlot.Tests.PlotTypes;

internal class ErrorBar
{
    [Test]
    public void Test_ErrorBar_Legend()
    {
        // demonstrates functionality described in #2432
        // https://github.com/ScottPlot/ScottPlot/issues/2432

        Random rand = new(0);
        int pointCount = 20;

        ScottPlot.Plot plt = new();
        ScottPlot.Plottable.ErrorBar err = plt.AddErrorBars(
            xs: DataGen.Consecutive(pointCount),
            ys: DataGen.RandomNormal(rand, pointCount, mean: 20, stdDev: 2),
            xErrors: DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray(),
            yErrors: DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray());

        err.Label = "Error Label";
        plt.Legend();

        TestTools.SaveFig(plt);
    }
}
