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
}
