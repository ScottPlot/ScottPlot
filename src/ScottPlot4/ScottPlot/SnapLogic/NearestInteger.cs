using System;

namespace ScottPlot.SnapLogic;

/// <summary>
/// Snaps to the nearest integer position
/// </summary>
public class NearestInteger : ISnap
{
    public double Snap(double value)
    {
        return Math.Round(value);
    }
}
