namespace ScottPlotTests.RenderTests.Plottable;

internal class ScatterTests
{
    [Test]
    public void Test_Scatter_Empty_Render()
    {
        ScottPlot.Plot plt = new();
        double[] xs = { };
        double[] ys = { };
        plt.Add.Scatter(xs, ys);
        Assert.DoesNotThrow(() => plt.Render());
    }

    [Test]
    public void Test_Scatter_SinglePoint_Render()
    {
        ScottPlot.Plot plt = new();
        double[] xs = { 1 };
        double[] ys = { 1 };
        plt.Add.Scatter(xs, ys);
        plt.Render();
        plt.SaveTestImage();

        Assert.That(plt.GetAxisLimits().Rect.Area, Is.Not.Zero);
    }


    [Test]
    public void Test_Scatter_RepeatedYs_Render()
    {
        ScottPlot.Plot plt = new();
        double[] xs = { 1, 2, 3, 4, 5 };
        double[] ys = { 7, 7, 7, 7, 7 };
        plt.Add.Scatter(xs, ys);
        plt.Render();
        plt.SaveTestImage();

        Assert.That(plt.GetAxisLimits().Rect.Area, Is.Not.Zero);
    }
}
