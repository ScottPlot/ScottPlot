namespace ScottPlot_Tests.RenderTests.Plottable;

internal class PieTests
{
    [Test]
    public void Test_Pie_Render()
    {
        Plot plt = new();

        ScottPlot.Plottables.PieSlice[] slices =
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
}
