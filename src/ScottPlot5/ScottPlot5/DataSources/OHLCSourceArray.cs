namespace ScottPlot.DataSources;

public class OHLCSourceArray(OHLC[] prices) : OHLCSourceBase, IOHLCSource
{
    private readonly OHLC[] Prices = prices;
    public override IReadOnlyList<OHLC> GetOHLCs() => Prices;
    public override int Count => Prices.Length;
}
