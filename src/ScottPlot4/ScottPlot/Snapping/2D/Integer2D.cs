namespace ScottPlot.SnapLogic;

/// <summary>
/// Returns the given position snapped to the nearest integer positions
/// </summary>
public class Integer2D : ISnap2D
{
    public Coordinate Snap(Coordinate value)
    {
        double x = System.Math.Round(value.X);
        double y = System.Math.Round(value.Y);
        return new Coordinate(x, y);
    }
}
