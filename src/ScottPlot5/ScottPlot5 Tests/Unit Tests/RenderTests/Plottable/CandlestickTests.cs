namespace ScottPlotTests.RenderTests.Plottable;

internal class CandlestickTests
{
    [Test]
    public void Test_Candlestick_NegativeX()
    {
        ScottPlot.Plot plot = new();

        List<OHLC> price = [];

        DateTime startDate = new(1899, 12, 25); // NOTE: 1900 is the OADate rollover
        OHLC ohlc = new(100, 103, 99, 102, startDate, TimeSpan.FromDays(1));

        for (int i = 0; i < 10; i++)
        {
            price.Add(ohlc.ShiftedBy(i).ShiftedBy(TimeSpan.FromDays(i)));
        }

        plot.Add.Candlestick(price);

        plot.Should().SavePngWithoutThrowing();
    }

    [Test]
    public void Test_OHLC_NegativeX()
    {
        ScottPlot.Plot plot = new();

        List<OHLC> price = [];

        DateTime startDate = new(1899, 12, 25); // NOTE: 1900 is the OADate rollover
        OHLC ohlc = new(100, 103, 99, 102, startDate, TimeSpan.FromDays(1));

        for (int i = 0; i < 10; i++)
        {
            price.Add(ohlc.ShiftedBy(i).ShiftedBy(TimeSpan.FromDays(i)));
        }

        plot.Add.OHLC(price);

        plot.Should().SavePngWithoutThrowing();
    }

    [Test]
    public void Test_Candlestick_NoPriceChange()
    {
        ScottPlot.Plot plot = new();

        List<OHLC> price = [];

        DateTime startDate = new(1899, 12, 25); // NOTE: 1900 is the OADate rollover
        OHLC ohlc = new(100, 105, 95, 95, startDate, TimeSpan.FromDays(1));

        for (int i = 0; i < 10; i++)
        {
            price.Add(ohlc.WithClose(95 + i).ShiftedBy(TimeSpan.FromDays(i)));
        }

        plot.Add.Candlestick(price);

        plot.Should().SavePngWithoutThrowing();
    }
}
