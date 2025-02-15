namespace ScottPlot;

public struct OHLC
{
    public double Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Close { get; set; }

    // TODO: deprecate this eventually. OHLC should be price information only.
    // Perhaps create a Candle primitive that contains time information.
    public DateTime DateTime { get; set; }
    public TimeSpan TimeSpan { get; set; }

    public OHLC(double open, double high, double low, double close)
        : this(open, high, low, close, DateTime.MinValue, TimeSpan.FromDays(1))
    {
    }

    public OHLC(double open, double high, double low, double close, DateTime start, TimeSpan span)
    {
        if (low > high)
        {
            throw new ArgumentException($"{nameof(high)} must be equal to or greater than {nameof(low)}");
        }

        if (open < low || close < low)
        {
            throw new ArgumentException($"{nameof(low)} must be equal to or less than {nameof(open)} and {nameof(close)}");
        }

        if (high < open || high < close)
        {
            throw new ArgumentException($"{nameof(high)} must be equal to or greater than {nameof(open)} and {nameof(close)}");
        }

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
