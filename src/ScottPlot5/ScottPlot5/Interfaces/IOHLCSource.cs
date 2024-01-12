namespace ScottPlot;

public interface IOHLCSource
{
    List<OHLC> GetOHLCs();
    public CoordinateRange GetLimitsX();
    public CoordinateRange GetLimitsY();
    AxisLimits GetLimits();
}
