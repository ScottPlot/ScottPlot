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
        vert.Text = "Vertical";

        var horiz = plot.Add.HorizontalLine(456.78);
        horiz.Text = "Horizontal";

        plot.SaveTestImage();
    }

    [Test]
    public void Test_AxisLine_Style()
    {
        ScottPlot.Plot plot = new();

        var hl = plot.Add.HorizontalLine(0.5);
        hl.Text = "HLine";
        hl.FontSize = 10;
        hl.FontColor = Colors.Yellow;

        var vl = plot.Add.VerticalLine(0.5);
        vl.Text = "VLine";
        vl.FontSize = 22;
        vl.Color = Colors.Magenta;
        vl.LineWidth = 3;
        vl.LinePattern = LinePattern.Dot;

        plot.SetAxisLimits(-10, 10, -10, 10);

        plot.SaveTestImage();
    }

    [Test]
    public void Test_AxisLine_ZeroWidth()
    {
        ScottPlot.Plot plot = new();

        var hl = plot.Add.HorizontalLine(0.5);
        hl.LineWidth = 0;

        plot.SetAxisLimits(-10, 10, -10, 10);

        plot.SaveTestImage();
    }

    [Test]
    public void Test_AxisLine_NoLabel()
    {
        ScottPlot.Plot plot = new();
        plot.Add.HorizontalLine(0.5);
        plot.SetAxisLimits(-10, 10, -10, 10);
        plot.SaveTestImage();
    }
}
