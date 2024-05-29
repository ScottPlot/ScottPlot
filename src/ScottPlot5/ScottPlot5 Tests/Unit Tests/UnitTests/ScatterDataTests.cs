using FluentAssertions;
using ScottPlot.DataSources;

namespace ScottPlotTests.UnitTests;

internal class ScatterDataTests
{
    [Test]
    public void Test_ScatterLimits()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);

        ScatterSourceDoubleArray source = new(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(0);
        limits.Right.Should().Be(50);
        limits.Bottom.Should().BeApproximately(-1, .1);
        limits.Top.Should().BeApproximately(1, .1);
    }

    [Test]
    public void Test_ScatterLimits_WithNoRealPoint()
    {
        double[] xs = Generate.NaN(51);
        double[] ys = Generate.NaN(51);

        xs[22] = 5; // single real X
        ys[33] = 7; // single real Y
        // but no real X,Y point

        ScatterSourceDoubleArray source = new(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOnePoint_DoubleArray()
    {
        double[] xs = Generate.NaN(51);
        double[] ys = Generate.NaN(51);

        xs[44] = 5;
        ys[44] = 7;

        ScatterSourceDoubleArray source = new(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOnePoint_CoordinatesArray()
    {
        double[] xs = Generate.NaN(51);
        double[] ys = Generate.NaN(51);

        xs[44] = 5;
        ys[44] = 7;

        Coordinates[] cs = Enumerable
            .Range(0, xs.Length)
            .Select(x => new Coordinates(xs[x], ys[x]))
            .ToArray();

        ScatterSourceCoordinatesArray source = new(cs);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOnePoint_CoordinatesList()
    {
        double[] xs = Generate.NaN(51);
        double[] ys = Generate.NaN(51);

        xs[44] = 5;
        ys[44] = 7;

        List<Coordinates> cs = Enumerable
            .Range(0, xs.Length)
            .Select(x => new Coordinates(xs[x], ys[x]))
            .ToList();

        ScatterSourceCoordinatesList source = new(cs);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOnePoint_CoordinatesGenericArray()
    {
        float[] xs = Enumerable.Range(0, 51).Select(x => float.NaN).ToArray();
        float[] ys = Enumerable.Range(0, 51).Select(x => float.NaN).ToArray();

        xs[44] = 5;
        ys[44] = 7;

        ScatterSourceGenericArray<float, float> source = new(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOnePoint_CoordinatesGenericList()
    {
        List<float> xs = Enumerable.Range(0, 51).Select(x => float.NaN).ToList();
        List<float> ys = Enumerable.Range(0, 51).Select(x => float.NaN).ToList();

        xs[44] = 5;
        ys[44] = 7;

        ScatterSourceGenericList<float, float> source = new(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOneMissingPoint()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);

        xs[44] = double.NaN;
        ys[44] = double.NaN;

        ScatterSourceDoubleArray source = new(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(0);
        limits.Right.Should().Be(50);
        limits.Bottom.Should().BeApproximately(-1, .1);
        limits.Top.Should().BeApproximately(1, .1);
    }

    [Test]
    public void Test_ScatterLimits_MissingLeft()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);

        for (int i = 0; i < 25; i++)
        {
            xs[i] = double.NaN;
            ys[i] = double.NaN;
        }

        ScatterSourceDoubleArray source = new(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(25);
        limits.Right.Should().Be(50);
        limits.Bottom.Should().BeApproximately(-1, .1);
        limits.Top.Should().BeApproximately(0, .1);
    }

    [Test]
    public void Test_ScatterLimits_MissingRight()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);

        for (int i = 26; i < ys.Length; i++)
        {
            xs[i] = double.NaN;
            ys[i] = double.NaN;
        }

        ScatterSourceDoubleArray source = new(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(0);
        limits.Right.Should().Be(25);
        limits.Bottom.Should().BeApproximately(0, .1);
        limits.Top.Should().BeApproximately(1, .1);
    }

    [Test]
    public void Test_Scatter_GetNearest_CoordinatesArray()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);
        Coordinates[] cs = Enumerable
            .Range(0, xs.Length)
            .Select(x => new Coordinates(xs[x], ys[x]))
            .ToArray();

        ScottPlot.Plot plot = new();
        var spDoubleArray = plot.Add.Scatter(cs);

        // force a render so we can get dimension info
        plot.GetImage(600, 400);
        var renderInfo = plot.RenderManager.LastRender;

        Coordinates location = new(25, 0.8);
        DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, maxDistance: 100);
        nearest.Index.Should().Be(20);
        nearest.X.Should().Be(20);
        nearest.Y.Should().BeApproximately(0.58778, .001);
    }

    [Test]
    public void Test_Scatter_GetNearest_CoordinatesList()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);
        List<Coordinates> cs = Enumerable
            .Range(0, xs.Length)
            .Select(x => new Coordinates(xs[x], ys[x]))
            .ToList();

        ScottPlot.Plot plot = new();
        var spDoubleArray = plot.Add.Scatter(cs);

        // force a render so we can get dimension info
        plot.GetImage(600, 400);
        var renderInfo = plot.RenderManager.LastRender;

        Coordinates location = new(25, 0.8);
        DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, maxDistance: 100);
        nearest.Index.Should().Be(20);
        nearest.X.Should().Be(20);
        nearest.Y.Should().BeApproximately(0.58778, .001);
    }

    [Test]
    public void Test_Scatter_GetNearest_DoubleArray()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);

        ScottPlot.Plot plot = new();
        var spDoubleArray = plot.Add.Scatter(xs, ys);

        // force a render so we can get dimension info
        plot.GetImage(600, 400);
        var renderInfo = plot.RenderManager.LastRender;

        Coordinates location = new(25, 0.8);
        DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, maxDistance: 100);
        nearest.Index.Should().Be(20);
        nearest.X.Should().Be(20);
        nearest.Y.Should().BeApproximately(0.58778, .001);
    }

    [Test]
    public void Test_Scatter_GetNearest_GenericArray()
    {
        float[] xs = Generate.Consecutive(51).Select(x => (float)x).ToArray();
        float[] ys = Generate.Sin(51).Select(x => (float)x).ToArray();

        ScottPlot.Plot plot = new();
        var spDoubleArray = plot.Add.Scatter(xs, ys);

        // force a render so we can get dimension info
        plot.GetImage(600, 400);
        var renderInfo = plot.RenderManager.LastRender;

        Coordinates location = new(25, 0.8);
        DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, maxDistance: 100);
        nearest.Index.Should().Be(20);
        nearest.X.Should().Be(20);
        nearest.Y.Should().BeApproximately(0.58778, .001);
    }

    [Test]
    public void Test_Scatter_GetNearest_GenericList()
    {
        List<float> xs = Generate.Consecutive(51).Select(x => (float)x).ToList();
        List<float> ys = Generate.Sin(51).Select(x => (float)x).ToList();

        ScottPlot.Plot plot = new();
        var spDoubleArray = plot.Add.Scatter(xs, ys);

        // force a render so we can get dimension info
        plot.GetImage(600, 400);
        var renderInfo = plot.RenderManager.LastRender;

        Coordinates location = new(25, 0.8);
        DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, maxDistance: 100);
        nearest.Index.Should().Be(20);
        nearest.X.Should().Be(20);
        nearest.Y.Should().BeApproximately(0.58778, .001);
    }
}
