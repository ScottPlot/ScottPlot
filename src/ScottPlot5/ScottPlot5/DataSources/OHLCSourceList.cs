namespace ScottPlot.DataSources;

public class OHLCSourceList(List<OHLC> prices) : OHLCSourceBase, IOHLCSource
{
    private readonly List<OHLC> Prices = prices;
    public override IReadOnlyList<OHLC> GetOHLCs() => Prices;
    public override int Count => Prices.Count;
}
