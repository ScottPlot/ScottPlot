using ScottPlot.Plottables;

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
        plt.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void Test_Scatter_Empty_StepDisplay_Render()
    {
        ScottPlot.Plot plt = new();
        double[] xs = { };
        double[] ys = { };
        var sp = plt.Add.Scatter(xs, ys);

        sp.ConnectStyle = ConnectStyle.StepHorizontal;
        plt.Should().RenderInMemoryWithoutThrowing();

        sp.ConnectStyle = ConnectStyle.StepVertical;
        plt.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void Test_Scatter_SinglePoint_Render()
    {
        ScottPlot.Plot plt = new();
        double[] xs = { 1 };
        double[] ys = { 1 };
        plt.Add.Scatter(xs, ys);
        plt.Should().RenderInMemoryWithoutThrowing();
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
        plt.Should().RenderInMemoryWithoutThrowing();
        plt.SaveTestImage();

        Assert.That(plt.Axes.GetLimits().Rect.Area, Is.Not.Zero);
    }

    [Test]
    public void Test_GetStepDisplayPixels_Right()
    {
        ScottPlot.Plot plt = new();
        double[] xs = { 1, 2, 3, 4, 5 };
        double[] ys = { 2, 4, 5, 8, 10 };
        double[] expectedXs = { 1, 2, 2, 3, 3, 4, 4, 5, 5 };
        double[] expectedYs = { 2, 2, 4, 4, 5, 5, 8, 8, 10 };

        var pixels = Enumerable.Range(0, 5).Select(x => new Pixel(xs[x], ys[x])).ToArray();

        var result = Scatter.GetStepDisplayPixels(pixels, true);

        var expectedPixels = Enumerable.Range(0, 9).Select(x => new Pixel(expectedXs[x], expectedYs[x])).ToArray();

        Assert.That(result, Is.EquivalentTo(expectedPixels));
    }

    [Test]
    public void Test_GetStepDisplayPixels_Left()
    {
        ScottPlot.Plot plt = new();
        double[] xs = { 1, 2, 3, 4, 5 };
        double[] ys = { 2, 4, 5, 8, 10 };
        double[] expectedXs = { 1, 1, 2, 2, 3, 3, 4, 4, 5 };
        double[] expectedYs = { 2, 4, 4, 5, 5, 8, 8, 10, 10 };

        var pixels = Enumerable.Range(0, 5).Select(x => new Pixel(xs[x], ys[x])).ToArray();

        var result = Scatter.GetStepDisplayPixels(pixels, false);

        var expectedPixels = Enumerable.Range(0, 9).Select(x => new Pixel(expectedXs[x], expectedYs[x])).ToArray();

        Assert.That(result, Is.EquivalentTo(expectedPixels));
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
        plt.Axes.DateTimeTicksBottom();
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
