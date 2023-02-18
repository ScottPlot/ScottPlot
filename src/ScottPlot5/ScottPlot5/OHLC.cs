namespace ScottPlot;

public readonly struct OHLC
{
    public double Open { get; }
    public double High { get; }
    public double Low { get; }
    public double Close { get; }
    public DateTime DateTime { get; }
    public TimeSpan TimeSpan { get; }

    public OHLC(double open, double high, double low, double close, DateTime start, TimeSpan span)
    {
        Open = open;
        High = high;
        Low = low;
        Close = close;
        DateTime = start;
        TimeSpan = span;
    }

    public CoordinateRange GetPriceRange()
    {
        double min = Open;
        min = Math.Min(min, High);
        min = Math.Min(min, Low);
        min = Math.Min(min, Close);

        double max = Open;
        max = Math.Max(max, High);
        max = Math.Max(max, Low);
        max = Math.Max(max, Close);

        return new CoordinateRange(min, max);
    }
}
