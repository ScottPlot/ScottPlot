namespace ScottPlot.DataSources;

public abstract class OHLCSourceBase : IOHLCSource
{
    public abstract IReadOnlyList<OHLC> GetOHLCs();

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
}
