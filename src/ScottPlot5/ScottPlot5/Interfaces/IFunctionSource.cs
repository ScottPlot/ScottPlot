namespace ScottPlot;

public interface IFunctionSource
{
    CoordinateRange RangeX { get; }
    CoordinateRange GetRangeY(CoordinateRange rangeX);
    double Get(double x);
}
