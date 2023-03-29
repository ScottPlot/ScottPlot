namespace ScottPlot;

public struct OHLC : IOHLC
{
    public double Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Close { get; set; }
    public DateTime DateTime { get; set; }
    public TimeSpan TimeSpan { get; set; }

    public OHLC(double open, double high, double low, double close, DateTime start, TimeSpan span)
    {
        Open = open;
        High = high;
        Low = low;
        Close = close;
        DateTime = start;
        TimeSpan = span;
    }
}

public static class OhlcExtensions
{
    public static CoordinateRange GetPriceRange(this IOHLC ohlc)
    {
        double min = ohlc.Open;
        min = Math.Min(min, ohlc.High);
        min = Math.Min(min, ohlc.Low);
        min = Math.Min(min, ohlc.Close);

        double max = ohlc.Open;
        max = Math.Max(max, ohlc.High);
        max = Math.Max(max, ohlc.Low);
        max = Math.Max(max, ohlc.Close);

        return new CoordinateRange(min, max);
    }
}
