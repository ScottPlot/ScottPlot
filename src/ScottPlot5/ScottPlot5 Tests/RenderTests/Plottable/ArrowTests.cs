namespace ScottPlotTests.RenderTests.Plottable;

internal class ArrowTests
{
    [Test]
    public void Test_Arrow_Render()
    {
        ScottPlot.Plot myPlot = new();

        double[] values = Generate.Sin();
        myPlot.Add.Signal(values);

        Coordinates arrowTip = new(25, 0);
        Coordinates arrowBase = arrowTip.WithDelta(5, .5);
        var arrow = myPlot.Add.Arrow(arrowBase, arrowTip);

        myPlot.SaveTestImage();
    }
}
