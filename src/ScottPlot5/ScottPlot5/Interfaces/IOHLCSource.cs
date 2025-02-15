namespace ScottPlot;

public interface IOHLCSource
{
    int Count { get; }
    IReadOnlyList<OHLC> GetOHLCs();
    CoordinateRange GetLimitsX();
    CoordinateRange GetLimitsY();
    AxisLimits GetLimits();
    CoordinateRange GetPriceRange(int index1, int index2);
}
