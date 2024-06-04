namespace ScottPlotTests.RenderTests.Plottable;

internal class AxisLineTests
{
    [Test]
    public void Test_AxisLine_Render()
    {
        ScottPlot.Plot plot = new();
        plot.HideGrid();

        plot.Add.VerticalLine(123.45, width: 1, pattern: LinePattern.Dashed);
        plot.Add.VerticalLine(123.55, width: 2, pattern: LinePattern.DenselyDashed);
        plot.Add.VerticalLine(123.65, width: 3, pattern: LinePattern.Dotted);
        plot.Add.VerticalLine(123.85, width: 10);

        plot.Add.HorizontalLine(123.45, width: 1, pattern: LinePattern.Dashed);
        plot.Add.HorizontalLine(123.55, width: 2, pattern: LinePattern.DenselyDashed);
        plot.Add.HorizontalLine(123.65, width: 3, pattern: LinePattern.Dotted);
        plot.Add.HorizontalLine(123.85, width: 10);

        plot.SaveTestImage();
    }

    [Test]
    public void Test_AxisLine_Label()
    {
        ScottPlot.Plot plot = new();
        plot.HideGrid();

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
        hl.LabelFontSize = 10;
        hl.LabelFontColor = Colors.Yellow;

        var vl = plot.Add.VerticalLine(0.5);
        vl.Text = "VLine";
        vl.LabelFontSize = 22;
        vl.Color = Colors.Magenta;
        vl.LineWidth = 3;
        vl.LinePattern = LinePattern.Dotted;

        plot.Axes.SetLimits(-10, 10, -10, 10);

        plot.SaveTestImage();
    }

    [Test]
    public void Test_AxisLine_ZeroWidth()
    {
        ScottPlot.Plot plot = new();

        var hl = plot.Add.HorizontalLine(0.5);
        hl.LineWidth = 0;

        plot.Axes.SetLimits(-10, 10, -10, 10);

        plot.SaveTestImage();
    }

    [Test]
    public void Test_AxisLine_NoLabel()
    {
        ScottPlot.Plot plot = new();
        plot.Add.HorizontalLine(0.5);
        plot.Axes.SetLimits(-10, 10, -10, 10);
        plot.SaveTestImage();
    }
}
