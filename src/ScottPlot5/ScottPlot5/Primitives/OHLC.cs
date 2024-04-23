namespace ScottPlot;

public struct OHLC
{
    public double Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Close { get; set; }
    public DateTime DateTime { get; set; }
    public TimeSpan TimeSpan { get; set; }

    public OHLC(double open, double high, double low, double close)
    {
        Open = open;
        High = high;
        Low = low;
        Close = close;
        DateTime = DateTime.MinValue;
        TimeSpan = TimeSpan.FromDays(1);
    }

    public OHLC(double open, double high, double low, double close, DateTime start, TimeSpan span)
    {
        Open = open;
        High = high;
        Low = low;
        Close = close;
        DateTime = start;
        TimeSpan = span;
    }

    public readonly OHLC Clone()
    {
        return new OHLC(Open, High, Low, Close, DateTime, TimeSpan);
    }

    public readonly OHLC WithOpen(double price)
    {
        return new OHLC(price, High, Low, Close, DateTime, TimeSpan);
    }

    public readonly OHLC WithHigh(double price)
    {
        return new OHLC(Open, price, Low, Close, DateTime, TimeSpan);
    }

    public readonly OHLC WithLow(double price)
    {
        return new OHLC(Open, High, price, Close, DateTime, TimeSpan);
    }

    public readonly OHLC WithClose(double price)
    {
        return new OHLC(Open, High, Low, price, DateTime, TimeSpan);
    }

    public readonly OHLC WithDate(DateTime dateTime)
    {
        return new OHLC(Open, High, Low, Close, dateTime, TimeSpan);
    }

    public readonly OHLC WithTimeSpan(TimeSpan timeSpan)
    {
        return new OHLC(Open, High, Low, Close, DateTime, timeSpan);
    }

    public readonly OHLC ShiftedBy(TimeSpan timeSpan)
    {
        return new OHLC(Open, High, Low, Close, DateTime + timeSpan, TimeSpan);
    }

    public readonly OHLC ShiftedBy(double delta)
    {
        return new OHLC(Open + delta, High + delta, Low + delta, Close + delta, DateTime, TimeSpan);
    }
}

public static class OhlcExtensions
{
    public static CoordinateRange GetPriceRange(this OHLC ohlc)
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
