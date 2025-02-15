namespace ScottPlotTests.UnitTests.PrimitiveTests;

public class LinePatternTests
{
    [Test]
    public void Test_EveryLinePattern_CanBeRendered()
    {
        List<LinePattern> patterns = [];
        patterns.AddRange(LinePattern.GetAllPatterns());
        patterns.Add(new([2, 2, 5, 10], 0, "Custom"));

        Plot plot = new();
        for (int i = 0; i < patterns.Count; i++)
        {
            var line = plot.Add.Line(0, i - 1, 10, i);
            line.LinePattern = patterns[i];
            line.LineWidth = 2;
            plot.Add.Text(patterns[i].Name, 10, i);
        }

        plot.Axes.Frameless();
        plot.HideGrid();
        plot.Axes.SetLimitsX(-1, 15);

        plot.SaveTestImage();
    }
}
