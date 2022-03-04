using System;
using NUnit.Framework;

namespace ScottPlotTests.RenderTests;

internal class Quickstart
{
    [Test]
    public void Test_BasicPlot_CanRender()
    {
        double[] xs = ScottPlot.Generate.Consecutive(51);
        double[] ys1 = ScottPlot.Generate.Sin(51);
        double[] ys2 = ScottPlot.Generate.Cos(51);

        var plt = new ScottPlot.Plot();
        plt.AddScatter(xs, ys1);
        plt.AddScatter(xs, ys2);

        TestIO.SaveFig(plt);
    }
}