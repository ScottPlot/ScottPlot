namespace ScottPlot;

public interface IOHLCSource
{
    IReadOnlyList<OHLC> GetOHLCs();
    public CoordinateRange GetLimitsX();
    public CoordinateRange GetLimitsY();
    AxisLimits GetLimits();
    public CoordinateRange GetPriceRange(int index1, int index2);
}
