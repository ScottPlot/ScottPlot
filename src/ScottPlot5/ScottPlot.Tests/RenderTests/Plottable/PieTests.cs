namespace ScottPlot.TestsV5.RenderTests.Plottable;

internal class PieTests
{
    [Test]
    public void Test_Pie_Render()
    {
        Plot plt = new();

        Plottables.PieSlice[] slices =
        {
            new(6, Colors.Red),
            new(4, Colors.Blue),
            new(3, Colors.Green),
            new(1, Colors.DarkCyan),
        };

        plt.Plottables.AddPie(slices);

        plt.SaveTestImage();
    }
}
