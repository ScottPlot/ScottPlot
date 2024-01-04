namespace ScottPlot;

public interface IOHLCSource : IHasAxisLimits
{
    IList<IOHLC> GetOHLCs();
}
