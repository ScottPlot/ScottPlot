using NUnit.Framework;

namespace ScottPlot.Tests.PlotTypes;

internal class DataPlots
{
    [Test]
    public void Test_DataLogger_NoPoints()
    {
        ScottPlot.Plot plt = new();
        plt.AddDataLogger();
        plt.Render();
    }

    [Test]
    public void Test_DataLogger_SinglePoint()
    {
        ScottPlot.Plot plt = new();
        var dl = plt.AddDataLogger();
        dl.Add(42, 69);
        plt.Render();
    }

    [Test]
    public void Test_DataStreamer_NoPoints()
    {
        ScottPlot.Plot plt = new();
        double[] values = new double[10];
        plt.AddDataStreamer(values);
        plt.Render();
    }

    [Test]
    public void Test_DataStreamer_SinglePoint()
    {
        ScottPlot.Plot plt = new();
        double[] values = new double[10];
        var ds = plt.AddDataStreamer(values);
        ds.Add(42);
        plt.Render();
    }
}
