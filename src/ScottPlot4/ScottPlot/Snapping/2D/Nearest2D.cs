using System;

namespace ScottPlot.SnapLogic;

/// <summary>
/// Snaps to the nearest position in a user-provided array
/// </summary>
public class Nearest2D : ISnap2D
{
    private readonly Coordinate[] Coordinates;

    public Nearest2D(Coordinate[] coordinates)
    {
        Coordinates = coordinates;
    }

    public Nearest2D(double[] xs, double[] ys)
    {
        if (xs.Length != ys.Length)
            throw new ArgumentException($"{nameof(xs)} must have the same length as {nameof(ys)}");

        Coordinates = new Coordinate[xs.Length];
        for (int i = 0; i < xs.Length; i++)
        {
            Coordinates[i] = new Coordinate(xs[i], ys[i]);
        }
    }

    /// <summary>
    /// Returns the position of the item in the array closest to the given position
    /// </summary>
    public Coordinate Snap(Coordinate value)
    {
        int closestIndex = SnapIndex(value);
        return Coordinates[closestIndex];
    }

    /// <summary>
    /// Returns the index of the item in the array closest to the given position
    /// </summary>
    public int SnapIndex(Coordinate value)
    {
        double closestDistance = double.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < Coordinates.Length; i++)
        {
            double dX = Math.Abs(Coordinates[i].X - value.X);
            double dY = Math.Abs(Coordinates[i].Y - value.Y);
            double distance = dX * dX + dY * dY;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}
