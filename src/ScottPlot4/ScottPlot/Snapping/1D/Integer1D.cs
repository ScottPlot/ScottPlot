using System;

namespace ScottPlot.SnapLogic;

/// <summary>
/// Snaps to the nearest integer position
/// </summary>
public class Integer1D : ISnap1D
{
    public double Snap(double value)
    {
        return Math.Round(value);
    }
}
