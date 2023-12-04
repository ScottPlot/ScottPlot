namespace ScottPlot;

/// <summary>
/// Represents a straight line in coordinate space
/// </summary>
public readonly struct CoordinateLine
{
    public readonly double X1;
    public readonly double X2;
    public readonly double Y1;
    public readonly double Y2;

    public CoordinateLine(double x1, double y1, double x2, double y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    public CoordinateLine(Coordinates pt1, Coordinates pt2)
    {
        X1 = pt1.X;
        Y1 = pt1.Y;
        X2 = pt2.X;
        Y2 = pt2.Y;
    }
}
