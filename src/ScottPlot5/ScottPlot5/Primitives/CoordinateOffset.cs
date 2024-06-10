namespace ScottPlot;

public readonly struct CoordinateOffset(double x, double y)
{
    public double X { get; } = x;
    public double Y { get; } = y;
}

