namespace ScottPlotTests.UnitTests;

internal class PlottableManagement
{
    [Test]
    public void Test_Plot_RemoveInstance()
    {
        double[] xs = Generate.Consecutive();
        double[] ys = Generate.Sin();

        Plot myPlot = new();

        // add scatter plots
        var sp1 = myPlot.Add.Scatter(xs, ys);
        var sp2 = myPlot.Add.Scatter(xs, ys);
        var sp3 = myPlot.Add.Scatter(xs, ys);

        // add signal plots
        var sig1 = myPlot.Add.Signal(ys);
        var sig2 = myPlot.Add.Signal(ys);
        var sig3 = myPlot.Add.Signal(ys);

        // add duplicates
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sig2);
        myPlot.Add.Plottable(sig2);

        myPlot.PlottableList.Count.Should().Be(10);

        myPlot.Remove(sig1); // one instance
        myPlot.PlottableList.Count.Should().Be(9);

        myPlot.Remove(sp2); // 3 instances
        myPlot.PlottableList.Count.Should().Be(6);
    }

    [Test]
    public void Test_Plot_RemoveType()
    {
        double[] xs = Generate.Consecutive();
        double[] ys = Generate.Sin();

        Plot myPlot = new();

        // add scatter plots
        var sp1 = myPlot.Add.Scatter(xs, ys);
        var sp2 = myPlot.Add.Scatter(xs, ys);
        var sp3 = myPlot.Add.Scatter(xs, ys);

        // add signal plots
        var sig1 = myPlot.Add.Signal(ys);
        var sig2 = myPlot.Add.Signal(ys);
        var sig3 = myPlot.Add.Signal(ys);

        // add duplicates
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sig2);
        myPlot.Add.Plottable(sig2);

        myPlot.PlottableList.Count.Should().Be(10);

        myPlot.Remove(sig1.GetType());
        myPlot.PlottableList.Count.Should().Be(5);

        myPlot.Remove(typeof(ScottPlot.Plottables.Scatter));
        myPlot.PlottableList.Count.Should().Be(0);
    }

    [Test]
    public void Test_Plot_RemoveTypeGeneric()
    {
        double[] xs = Generate.Consecutive();
        double[] ys = Generate.Sin();

        Plot myPlot = new();

        // add scatter plots
        var sp1 = myPlot.Add.Scatter(xs, ys);
        var sp2 = myPlot.Add.Scatter(xs, ys);
        var sp3 = myPlot.Add.Scatter(xs, ys);

        // add signal plots
        var sig1 = myPlot.Add.Signal(ys);
        var sig2 = myPlot.Add.Signal(ys);
        var sig3 = myPlot.Add.Signal(ys);

        // add duplicates
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sig2);
        myPlot.Add.Plottable(sig2);

        myPlot.PlottableList.Count.Should().Be(10);

        myPlot.Remove<ScottPlot.Plottables.Signal>();
        myPlot.PlottableList.Count.Should().Be(5);

        myPlot.Remove<ScottPlot.Plottables.Scatter>();
        myPlot.PlottableList.Count.Should().Be(0);
    }

    [Test]
    public void Test_Plot_RemoveTypePredicate()
    {
        double[] xs = Generate.Consecutive();
        double[] ys = Generate.Sin();

        Plot myPlot = new();

        var sp1 = myPlot.Add.Scatter(xs, ys);
        sp1.LegendText = "AAA";

        var sp2 = myPlot.Add.Scatter(xs, ys);
        sp2.LegendText = "ABC";

        var sp3 = myPlot.Add.Scatter(xs, ys);
        sp3.LegendText = "ABAB";

        var sp4 = myPlot.Add.Scatter(xs, ys);
        sp4.LegendText = "LOLOLOLOLOLOL";
        sp4.Color = Colors.Magenta;

        myPlot.PlottableList.Count.Should().Be(4);

        // match label content
        myPlot.Remove<ScottPlot.Plottables.Scatter>(x => x.LegendText!.Contains('B'));
        myPlot.PlottableList.Count.Should().Be(2);

        // match style options
        myPlot.Remove<ScottPlot.Plottables.Scatter>(x => x.Color == Colors.Magenta);
        myPlot.PlottableList.Count.Should().Be(1);
    }

    [Test]
    public void Test_Plot_GetPlottables()
    {
        double[] xs = Generate.Consecutive();
        double[] ys = Generate.Sin();

        Plot myPlot = new();

        // add scatter plots
        var sp1 = myPlot.Add.Scatter(xs, ys);
        var sp2 = myPlot.Add.Scatter(xs, ys);
        var sp3 = myPlot.Add.Scatter(xs, ys);

        // add signal plots
        var sig1 = myPlot.Add.Signal(ys);
        var sig2 = myPlot.Add.Signal(ys);

        myPlot.GetPlottables().Count().Should().Be(5);
        myPlot.GetPlottables<ScottPlot.Plottables.Scatter>().Count().Should().Be(3);
        myPlot.GetPlottables<ScottPlot.Plottables.Signal>().Count().Should().Be(2);
    }

    [Test]
    public void NullTextTest()
    {
        Plot myPlot = new Plot();
        myPlot.Add.Text(null!, new Coordinates());
        myPlot.RenderManager.Render(new SkiaSharp.SKCanvas(new SkiaSharp.SKBitmap()), new PixelRect());
        Assert.Pass();
    }
}
