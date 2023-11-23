namespace ScottPlotTests.RenderTests.Plottable;

internal class AxisLineTests
{
    [Test]
    public void Test_AxisLine_Render()
    {
        ScottPlot.Plot plot = new();
        plot.DisableGrid();

        plot.Add.VerticalLine(123.45, width: 1, pattern: LinePattern.Dash);
        plot.Add.VerticalLine(123.55, width: 2, pattern: LinePattern.Dot);
        plot.Add.VerticalLine(123.75, width: 10);

        plot.Add.HorizontalLine(123.45, width: 1, pattern: LinePattern.Dash);
        plot.Add.HorizontalLine(123.55, width: 2, pattern: LinePattern.Dot);
        plot.Add.HorizontalLine(123.75, width: 10);

        plot.SaveTestImage();
    }

    [Test]
    public void Test_AxisLine_Label()
    {
        ScottPlot.Plot plot = new();
        plot.DisableGrid();

        var vert = plot.Add.VerticalLine(123.45);
        vert.Label.Text = "Vertical";

        var horiz = plot.Add.HorizontalLine(456.78);
        horiz.Label.Text = "Horizontal";

        plot.SaveTestImage();
    }
}
