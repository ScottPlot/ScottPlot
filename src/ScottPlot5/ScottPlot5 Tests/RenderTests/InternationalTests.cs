using System.Globalization;

namespace ScottPlotTests.RenderTests;

internal class InternationalTests
{
    [Test]
    public void Test_Tick_DefaultCulture()
    {
        ScottPlot.Plot plot = new();
        plot.Add.Signal(Generate.Sin(100, 500_000));
        plot.Should().RenderInMemoryWithoutThrowing();
        var tickLabels = plot.Axes.Left.TickGenerator.Ticks.Select(x => x.Label);
        tickLabels.Should().Contain("-500,000");
    }

    [Test]
    public void Test_Tick_OtherCulture()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

        ScottPlot.Plot plot = new();
        plot.Add.Signal(Generate.Sin(100, 500_000));
        plot.Should().RenderInMemoryWithoutThrowing();
        var tickLabels = plot.Axes.Left.TickGenerator.Ticks.Select(x => x.Label);
        tickLabels.Should().Contain("-500 000");
    }
}
