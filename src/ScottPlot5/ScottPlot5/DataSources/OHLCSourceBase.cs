namespace ScottPlot.DataSources;

public abstract class OHLCSourceBase : IOHLCSource
{
    public abstract IReadOnlyList<OHLC> GetOHLCs();
    public abstract int Count { get; }

    public AxisLimits GetLimits()
    {
        return GetOHLCs().Any()
            ? new AxisLimits(GetLimitsX(), GetLimitsY())
            : AxisLimits.NoLimits;
    }

    public CoordinateRange GetLimitsX()
    {
        var dates = GetOHLCs().Select(x => x.DateTime);
        return new CoordinateRange(NumericConversion.ToNumber(dates.Min()), NumericConversion.ToNumber(dates.Max()));
    }

    public CoordinateRange GetLimitsY()
    {
        var priceRanges = GetOHLCs().Select(x => new Range(x.Low, x.High));
        double min = priceRanges.Select(x => x.Min).Min();
        double max = priceRanges.Select(x => x.Max).Max();
        return new CoordinateRange(min, max);
    }

    public CoordinateRange GetPriceRange(int index1, int index2)
    {
        var ohlcs = GetOHLCs();
        var ohlcsInView = ohlcs.Skip(index1).Take(index2 - index1);
        if (!ohlcsInView.Any())
            return CoordinateRange.NotSet;

        double yMin = ohlcsInView.First().Low;
        double yMax = ohlcsInView.First().High;
        for (int i = index1; i <= index2; i++)
        {
            yMin = Math.Min(yMin, ohlcs[i].Low);
            yMax = Math.Max(yMax, ohlcs[i].High);
        }

        return new(yMin, yMax);
    }
}
