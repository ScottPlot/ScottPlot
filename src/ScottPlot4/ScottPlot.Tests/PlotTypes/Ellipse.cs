using NUnit.Framework;
using ScottPlotTests;

namespace ScottPlot.Tests.PlotTypes;

internal class Ellipse
{
    [Test]
    public void Test_Ellipse_AddCircle()
    {
        Plot plt = new(400, 300);

        plt.AddCircle(0, 0, 5);
        plt.AddCircle(2, 3, 5);
        plt.AddCircle(2, 3, 0.5);
        plt.AddCircle(0, 0, -1);

        TestTools.SaveFig(plt);
    }

    [Test]
    public void Test_Ellipse_AddElipse()
    {
        Plot plt = new(400, 300);

        plt.AddEllipse(3, 0, 2, 2);
        plt.AddEllipse(2, 3, 5, 1);
        plt.AddEllipse(0, 0, 1, 5);

        TestTools.SaveFig(plt);
    }
}
