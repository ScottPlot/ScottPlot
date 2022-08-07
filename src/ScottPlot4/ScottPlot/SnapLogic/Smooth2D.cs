namespace ScottPlot.SnapLogic;

/// <summary>
/// Always returns the original value (snapping disabled)
/// </summary>
public class Smooth2D : ISnap2D
{
    public Coordinate Snap(Coordinate value)
    {
        return value;
    }
}
