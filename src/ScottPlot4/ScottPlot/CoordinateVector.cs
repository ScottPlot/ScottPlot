using System;

namespace ScottPlot;

/// <summary>
/// Represents direction and magnitude in coordinate space
/// </summary>
public struct CoordinateVector
{
    public double X { get; set; }

    public double Y { get; set; }

    public double Magnitude => Math.Sqrt(X * X + Y * Y);

    public CoordinateVector(double x, double y)
    {
        X = x;
        Y = y;
    }

    public static Coordinate operator +(Coordinate coordinate, CoordinateVector vector)
    {
        return new Coordinate(coordinate.X + vector.X, coordinate.Y + vector.Y);
    }

    public static Coordinate operator -(Coordinate coordinate, CoordinateVector vector)
    {
        return new Coordinate(coordinate.X - vector.X, coordinate.Y - vector.Y);
    }
}
