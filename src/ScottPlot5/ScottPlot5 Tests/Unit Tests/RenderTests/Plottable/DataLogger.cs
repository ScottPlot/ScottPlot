namespace ScottPlotTests.RenderTests.Plottable;

internal class DataLogger
{
    [Test]
    public void Test_EmptyDataLogger_WithDateTimeAxis()
    {
        Plot plt = new();

        var logger = plt.Add.DataLogger();
        plt.Axes.DateTimeTicksBottom();
        plt.Should().SavePngWithoutThrowing("empty");

        logger.Add(new DateTime(2024, 1, 1).ToOADate(), 1);
        logger.Add(new DateTime(2024, 1, 2).ToOADate(), 4);
        logger.Add(new DateTime(2024, 1, 3).ToOADate(), 9);
        plt.Should().SavePngWithoutThrowing("points");
    }
}
