using System;

namespace ScottPlot.SnapLogic;

/// <summary>
/// Snaps to the nearest position in a user-provided array
/// </summary>
public class Nearest1D : ISnap1D
{
    private readonly double[] SnapPositions;

    public Nearest1D(double[] positions)
    {
        SnapPositions = positions;
    }

    public double Snap(double value)
    {
        var closestDistance = double.MaxValue;
        var closestPosition = double.MaxValue;

        foreach (var position in SnapPositions)
        {
            var distance = Math.Abs(value - position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = position;
            }
        }

        return closestPosition;
    }
}
