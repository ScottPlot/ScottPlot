namespace ScottPlotTests.RenderTests.Plottable;

internal class BarTests
{
    [Test]
    public void Test_Bar_Single()
    {
        Plot plt = new();

        List<Bar> bars = new()
        {
            new Bar() { Position = 1, Value = 5, Error = 1, FillColor = Colors.Red },
            new Bar() { Position = 2, Value = 7, Error = 2, FillColor = Colors.Green },
            new Bar() { Position = 4, Value = 3, Error = .5, FillColor = Colors.Blue, ErrorPositive = false },
            new Bar() { Position = 5, Value = 5, Error = 1, FillColor = Colors.Orange, ErrorNegative = false },
        };

        plt.Add.Bars(bars);
        plt.Should().RenderInMemoryWithoutThrowing();
        plt.Axes.SetLimits(bottom: 0);

        plt.SaveTestImage();
    }
}
