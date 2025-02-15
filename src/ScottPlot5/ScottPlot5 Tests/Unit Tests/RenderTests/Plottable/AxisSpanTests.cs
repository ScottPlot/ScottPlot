namespace ScottPlotTests.RenderTests.Plottable;

internal class AxisSpanTests
{
    [Test]
    public void Test_AxisSpan_ExtremelyNarrow()
    {
        ScottPlot.Plot plot = new();

        for (int i = 0; i < 10; i++)
        {
            double width = 1e-10;
            plot.Add.VerticalSpan(i, i + width);
            plot.Add.HorizontalSpan(i, i + width);
        }

        plot.HideGrid();
        plot.SaveTestImage();
    }
}
