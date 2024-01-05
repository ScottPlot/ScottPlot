namespace ScottPlot;

public interface IOHLCSource : IHasAxisLimits
{
    IList<OHLC> GetOHLCs();
}
