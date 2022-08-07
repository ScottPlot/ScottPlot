namespace ScottPlot.SnapLogic;

/// <summary>
/// Customizable 2D snap system from independent 1D snap objects.
/// </summary>
public class Independent2D : ISnap2D
{
    public ISnap1D SnapX { get; set; } = new NoSnap1D();
    public ISnap1D SnapY { get; set; } = new NoSnap1D();

    public Independent2D()
    {

    }

    public Independent2D(ISnap1D x, ISnap1D y)
    {
        SnapX = x;
        SnapY = y;
    }

    public Coordinate Snap(Coordinate value)
    {
        double x = SnapX.Snap(value.X);
        double y = SnapY.Snap(value.Y);
        return new Coordinate(x, y);
    }
}
