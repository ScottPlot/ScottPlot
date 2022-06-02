using System;

namespace ScottPlot;

/// <summary>
/// Describes the location and size of a rectangle in coordinate space
/// </summary>
public class CoordinateRect
{
    public readonly double XMin;
    public readonly double XMax;
    public readonly double YMin;
    public readonly double YMax;

    public double Width => XMax - XMin;
    public double Height => YMax - YMin;
    public double Area => Math.Abs(Width * Height);
    public bool HasArea => Area > 0;

    public CoordinateRect(double x1, double x2, double y1, double y2)
    {
        if (double.IsNaN(x1) || double.IsInfinity(x1))
            throw new ArgumentOutOfRangeException(nameof(x1), "must be a real number");

        if (double.IsNaN(x2) || double.IsInfinity(x2))
            throw new ArgumentOutOfRangeException(nameof(x2), "must be a real number");

        if (double.IsNaN(y1) || double.IsInfinity(y1))
            throw new ArgumentOutOfRangeException(nameof(y1), "must be a real number");

        if (double.IsNaN(y2) || double.IsInfinity(y2))
            throw new ArgumentOutOfRangeException(nameof(y2), "must be a real number");

        XMin = Math.Min(x1, x2);
        XMax = Math.Max(x1, x2);
        YMin = Math.Min(y1, y2);
        YMax = Math.Max(y1, y2);
    }

    public bool Contains(Coordinate coord)
    {
        return Contains(coord.X, coord.Y);
    }

    public bool Contains(double x, double y)
    {
        return x >= XMin && x <= XMax && y >= YMin && y <= YMax;
    }

    public override string ToString()
    {
        return $"CoordinateRect: x=[{XMin}, {XMax}] y=[{YMin}, {YMax}]";
    }

    public static CoordinateRect BoundingBox(Coordinate[] coordinates)
    {
        if (coordinates is null)
            throw new ArgumentNullException(nameof(coordinates));

        if (coordinates.Length == 0)
            throw new ArgumentException(nameof(coordinates), "must not be empty");

        double x1 = coordinates[0].X;
        double x2 = coordinates[0].X;
        double y1 = coordinates[0].Y;
        double y2 = coordinates[0].Y;

        foreach (Coordinate c in coordinates)
        {
            x1 = Math.Min(x1, c.X);
            x2 = Math.Max(x2, c.X);
            y1 = Math.Min(y1, c.Y);
            y2 = Math.Max(y2, c.Y);
        }

        return new(x1, x2, y1, y2);
    }
}
