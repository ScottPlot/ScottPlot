using NUnit.Framework;

namespace ScottPlotTests;

internal class Quickstart
{
    [Test]
    public void Test_BasicPlot_CanRender()
    {
        var plt = new ScottPlot.Plot();
        plt.AddDemoSinAndCos();

        string filePath = TestIO.SaveFig(plt);
        Assert.That(System.IO.File.Exists(filePath));
    }
}