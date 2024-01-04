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

        Assert.That(plt.Axes.GetLimits().Rect.Area, Is.Not.Zero);
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

        Assert.That(plt.Axes.GetLimits().Rect.Area, Is.Not.Zero);
    }

    [Test]
    public void Test_Scatter_AddGenericArray()
    {
        float[] xs = { 1, 2, 3, 4 };
        UInt16[] ys = { 1, 3, 2, 4 };

        ScottPlot.Plot plt = new();
        plt.Add.Scatter(xs, ys);
        plt.SaveTestImage();
    }

    [Test]
    public void Test_Scatter_DateTimeXs()
    {
        DateTime firstDay = new(2024, 01, 01);
        DateTime[] days = Generate.DateTime.Days(365, firstDay);
        double[] values = Generate.RandomWalk(days.Length);

        ScottPlot.Plot plt = new();
        plt.Add.Scatter(days, values);
        plt.Axes.DateTimeTicks(Edge.Bottom);
        plt.SaveTestImage();
    }

    [Test]
    public void Test_Scatter_AddGenericList()
    {
        List<float> xs = new() { 1, 2, 3, 4 };
        List<UInt16> ys = new() { 1, 3, 2, 4 };

        ScottPlot.Plot plt = new();
        plt.Add.Scatter(xs, ys);
        plt.SaveTestImage();
    }
}
