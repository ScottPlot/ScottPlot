namespace ScottPlot.DataSources;

public interface IOHLCSource : IHasAxisLimits
{
    IList<IOHLC> GetOHLCs();
}
