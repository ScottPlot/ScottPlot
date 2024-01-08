namespace ScottPlot;

public interface IOHLCSource
{
    IList<OHLC> GetOHLCs();
    public CoordinateRange GetLimitsX(); // TODO: struct
    public CoordinateRange GetLimitsY(); // TODO: struct
    AxisLimits GetLimits();
}
