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

    /// <summary>
    /// Returns the position of the item in the array closest to the given position
    /// </summary>
    public double Snap(double value)
    {
        int index = SnapIndex(value);
        return SnapPositions[index];
    }

    /// <summary>
    /// Returns the index of the item in the array closest to the given position
    /// </summary>
    public int SnapIndex(double value)
    {
        var closestDistance = double.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < SnapPositions.Length; i++)
        {
            double distance = Math.Abs(value - SnapPositions[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}
