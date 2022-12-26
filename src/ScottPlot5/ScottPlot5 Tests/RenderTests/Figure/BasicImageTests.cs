namespace ScottPlotTests.RenderTests.Figure;

internal class BasicImageTests
{
    [Test]
    public void Test_Render_SinCos()
    {
        Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));
        plt.Add.Signal(Generate.Cos(51));

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Render_Image()
    {
        Plot plt = new();

        plt.Plottables.Add(new ScottPlot.Plottables.DebugPoint(2, 3, Colors.Magenta));
        plt.Plottables.Add(new ScottPlot.Plottables.DebugPoint(-7, -4, Colors.LightGreen));

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Render_MousePan()
    {
        Plot plt = new();

        plt.Plottables.Add(new ScottPlot.Plottables.DebugPoint(2, 3, Colors.Magenta));
        plt.Plottables.Add(new ScottPlot.Plottables.DebugPoint(-7, -4, Colors.LightGreen));

        AxisLimits limits = plt.GetAxisLimits();
        plt.SaveTestImage("1");

        plt.SetAxisLimits(limits.WithPan(2, 3));
        plt.SaveTestImage("2");
    }
}
