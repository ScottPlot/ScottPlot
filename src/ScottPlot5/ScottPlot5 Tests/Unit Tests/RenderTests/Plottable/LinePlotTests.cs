using ScottPlot.Plottables;

namespace ScottPlotTests.RenderTests.Plottable;

internal class LinePlotTests
{
    [Test]
    public void Test_LinePlotSet()
    {
        Coordinates start = new Coordinates(0, 0);
        Coordinates end = new Coordinates(5, 6);

        LinePlot lp = new();

        lp.Line = new CoordinateLine(start, end);

        var newStart = lp.Line.Start;
        var newEnd = lp.Line.End;

        newStart.Should().Be(start);
        newEnd.Should().Be(end);
    }

    [Test]
    public void Test_LinePlotGet()
    {
        Coordinates start = new Coordinates(0, 0);
        Coordinates end = new Coordinates(5, 6);

        LinePlot lp = new();

        lp.Start = start;
        lp.End = end;

        var cl = new CoordinateLine(start, end);

        cl.Should().Be(lp.Line);
    }
}
