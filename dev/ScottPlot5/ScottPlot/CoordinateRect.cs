using System;

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

    private static bool IsFinite(double x) => !(double.IsInfinity(x) || double.IsNaN(x));

    public bool HasFiniteWidth => IsFinite(XMin) && IsFinite(XMax);
    public bool HasFiniteHeight => IsFinite(YMin) && IsFinite(YMax);

    public CoordinateRect(double xMin, double xMax, double yMin, double yMax)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
    }

    public static CoordinateRect AllNan() => new(double.NaN, double.NaN, double.NaN, double.NaN);

    public CoordinateRect Expand(CoordinateRect rect)
    {
        double xMinNew = rect.XMin;
        if (!double.IsNaN(XMin) && !double.IsNaN(rect.XMin))
            xMinNew = Math.Min(rect.XMin, XMin);

        double xMaxNew = rect.XMax;
        if (!double.IsNaN(XMax) && !double.IsNaN(rect.XMax))
            xMaxNew = Math.Max(rect.XMax, XMax);

        double yMinNew = rect.YMin;
        if (!double.IsNaN(YMin) && !double.IsNaN(rect.YMin))
            yMinNew = Math.Min(rect.YMin, YMin);

        double yMaxNew = rect.YMax;
        if (!double.IsNaN(YMax) && !double.IsNaN(rect.YMax))
            yMaxNew = Math.Max(rect.YMax, YMax);

        return new CoordinateRect(xMinNew, xMaxNew, yMinNew, yMaxNew);
    }

    public CoordinateRect WithX(double xMin, double xMax) => new(xMin, xMax, YMin, YMax);

    public CoordinateRect WithY(double yMin, double yMax) => new(XMin, XMax, yMin, yMax);


    public override string ToString() => $"xMin={XMin}, xMax={XMax}, yMin={YMin}, yMax={YMax}";

    public static CoordinateRect operator +(CoordinateRect a, Coordinate b) =>
        new(xMin: a.XMin + b.X,
            xMax: a.XMax + b.X,
            yMin: a.YMin + b.Y,
            yMax: a.YMax + b.Y);
}
