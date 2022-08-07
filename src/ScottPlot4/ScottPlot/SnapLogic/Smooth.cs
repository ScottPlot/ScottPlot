namespace ScottPlot.SnapLogic;

/// <summary>
/// Always returns the original value (snapping disabled)
/// </summary>
public class Smooth : ISnap
{
    public double Snap(double value)
    {
        return value;
    }
}
