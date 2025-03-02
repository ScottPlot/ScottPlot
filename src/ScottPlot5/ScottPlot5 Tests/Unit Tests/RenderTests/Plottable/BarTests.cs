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

    [Test]
    public void Test_Bar_TextBelow()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/4760

        List<Bar> bars = [];
        for (int i = 0; i < 10; i++)
        {
            double value = Generate.RandomInteger(-100, 100);
            bars.Add(new()
            {
                Position = i,
                Value = value,
                Label = value.ToString(),
                FillColor = Colors.C0,
                LineWidth = 0,
            });
        }

        Plot plot = new();
        var barPlot = plot.Add.Bars(bars);
        barPlot.ValueLabelStyle.FontSize = 18;
        barPlot.ValueLabelStyle.Bold = true;

        plot.Axes.Margins(0.1, 0.25); // increase vertical margins to make room for labels

        plot.Add.HorizontalLine(0, 1, Colors.Black, LinePattern.DenselyDashed);
        plot.HideGrid();
        plot.SaveTestImage();
    }
}
