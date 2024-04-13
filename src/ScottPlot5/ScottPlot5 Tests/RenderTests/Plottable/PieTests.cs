using ScottPlot.Plottables;

namespace ScottPlotTests.RenderTests.Plottable;

internal class PieTests
{
    [Test]
    public void Test_Pie_Render()
    {
        Plot plt = new();

        PieSlice[] slices =
        {
            new(6, Colors.Red),
            new(4, Colors.Blue),
            new(3, Colors.Green),
            new(1, Colors.DarkCyan),
        };

        var pie = plt.Add.Pie(slices);
        pie.ExplodeFraction = .1;

        pie.Slices.Should().HaveCount(4);

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Pie_Legend()
    {
        Plot plt = new();

        // start with all data the same size
        List<PieSlice> slices = new()
        {
            new PieSlice(5, Colors.Red, "Alarm"),
            new PieSlice(5, Colors.Green, "Run"),
            new PieSlice(5, Colors.Blue, "Chill"),
        };

        var pie = plt.Add.Pie(slices);
        pie.LineStyle.Color = ScottPlot.Colors.Transparent;

        plt.Legend.IsVisible = true;
        plt.SaveTestImage();
    }
}
