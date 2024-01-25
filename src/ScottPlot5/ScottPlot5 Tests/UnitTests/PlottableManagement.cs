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
}
