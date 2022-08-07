using System;

namespace ScottPlot.SnapLogic;

/// <summary>
/// Snaps to the nearest position in a user-provided array
/// </summary>
public class NearestPosition2D : ISnap2D
{
    private readonly double[] Xs;
    private readonly double[] Ys;

    public NearestPosition2D(double[] xs, double[] ys)
    {
        if (xs.Length != ys.Length)
            throw new ArgumentException($"{nameof(Xs)} must have the same length as {nameof(Ys)}");

        Xs = xs;
        Ys = ys;
    }

    public Coordinate Snap(Coordinate value)
    {
        double closestDistance = double.MaxValue;
        int closestIndex = 0;
        for (int i = 0; i < Xs.Length; i++)
        {
            double dX = Math.Abs(Xs[i] - value.X);
            double dY = Math.Abs(Ys[i] - value.Y);
            double distance = dX * dX + dY * dY;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        return new Coordinate(Xs[closestIndex], Ys[closestIndex]);
    }
}
