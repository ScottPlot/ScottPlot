namespace ScottPlot.DataSources;

public class OHLCSource : IOHLCSource
{
    private readonly List<OHLC> Prices;

    public OHLCSource(List<OHLC> prices)
    {
        Prices = prices;
    }

    public List<OHLC> GetOHLCs()
    {
        return Prices;
    }

    public AxisLimits GetLimits()
    {
        return Prices.Any() ? new AxisLimits(GetLimitsX(), GetLimitsY()) : AxisLimits.NoLimits;
    }

    public CoordinateRange GetLimitsX()
    {
        var dates = Prices.Select(x => x.DateTime);
        return new CoordinateRange(NumericConversion.ToNumber(dates.Min()), NumericConversion.ToNumber(dates.Max()));
    }

    public CoordinateRange GetLimitsY()
    {
        var priceRanges = Prices.Select(x => x.GetPriceRange());
        double min = priceRanges.Select(x => x.Min).Min();
        double max = priceRanges.Select(x => x.Max).Max();
        return new CoordinateRange(min, max);
    }
}
