namespace ScottPlot.SnapLogic;

/// <summary>
/// Always returns the original value (snapping disabled)
/// </summary>
public class NoSnap1D : ISnap1D
{
    public double Snap(double value)
    {
        return value;
    }
}
