namespace ScottPlot;

/// <summary>
/// Represents a rectangle on the plot.
/// May be used to represent visible axis limits.
/// </summary>
public struct CoordinateRect
{
    public readonly double XMin;
    public readonly double XMax;
    public readonly double YMin;
    public readonly double YMax;

    public double XSpan => XMax - XMin;
    public double YSpan => YMax - YMin;
    public double Area => XSpan * YSpan;

    public CoordinateRect(double xMin, double xMax, double yMin, double yMax)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
    }

    public override string ToString() => $"xMin={XMin}, xMax={XMax}, yMin={YMin}, yMax={YMax}";

    public static CoordinateRect operator +(CoordinateRect a, Coordinate b) =>
        new(xMin: a.XMin + b.X,
            xMax: a.XMax + b.X,
            yMin: a.YMin + b.Y,
            yMax: a.YMax + b.Y);
}
