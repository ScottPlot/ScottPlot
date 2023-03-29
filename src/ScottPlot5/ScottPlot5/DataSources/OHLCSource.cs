namespace ScottPlot.DataSources;

public class OHLCSource : IOHLCSource
{
    private readonly IList<IOHLC> Prices;

    public OHLCSource(IList<IOHLC> prices)
    {
        Prices = prices;
    }

    public IList<IOHLC> GetOHLCs()
    {
        return Prices;
    }

    public AxisLimits GetLimits()
    {
        return new AxisLimits(GetLimitsX(), GetLimitsY());
    }

    public CoordinateRange GetLimitsX()
    {
        var dates = Prices.Select(x => x.DateTime);
        return new CoordinateRange(dates.Min().ToNumber(), dates.Max().ToNumber());
    }

    public CoordinateRange GetLimitsY()
    {
        var priceRanges = Prices.Select(x => x.GetPriceRange());
        double min = priceRanges.Select(x => x.Min).Min();
        double max = priceRanges.Select(x => x.Max).Max();
        return new CoordinateRange(min, max);
    }
}
