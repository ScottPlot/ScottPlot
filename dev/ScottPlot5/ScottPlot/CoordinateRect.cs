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

    public double Width => XMax - XMin;
    public double Height => YMax - YMin;
    public double Area => Width * Height;
    public double XCenter => (XMax + XMin) / 2;
    public double YCenter => (YMax + YMin) / 2;

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
