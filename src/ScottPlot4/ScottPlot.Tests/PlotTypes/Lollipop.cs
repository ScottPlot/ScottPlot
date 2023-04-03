using NUnit.Framework;
using ScottPlotTests;

namespace ScottPlot.Tests.PlotTypes;

internal class Lollipop
{
    [Test]
    public void Test_Lollipop_LineWidth()
    {
        var xs = new double[] { 2, 3, 4, 5 };
        var ys = new double[] { 4, 5, 6, 7 };

        ScottPlot.Plot plt = new(400, 300);
        var lp = plt.AddLollipop(xs, ys);
        lp.LineWidth = 5;
        lp.LollipopRadius = 10;

        TestTools.SaveFig(plt);
    }

    [Test]
    public void Test_Cleveland_LineWidth()
    {
        var xs = new double[] { 2, 3, 4, 5 };
        var ys = new double[] { 4, 5, 6, 7 };

        ScottPlot.Plot plt = new(400, 300);
        var cb = plt.AddClevelandDot(xs, ys);
        cb.LineWidth = 5;
        cb.DotRadius = 10;

        TestTools.SaveFig(plt);
    }
}
