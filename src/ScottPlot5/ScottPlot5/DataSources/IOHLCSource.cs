namespace ScottPlot.DataSources;

public interface IOHLCSource : IHasAxisLimits
{
    IReadOnlyList<OHLC> GetOHLCs();
}
