namespace ScottPlot;

public interface IFunctionSource
{
    CoordinateRange RangeX { get; set; }
    CoordinateRange GetRangeY(CoordinateRange rangeX);
    double Get(double x);
}
