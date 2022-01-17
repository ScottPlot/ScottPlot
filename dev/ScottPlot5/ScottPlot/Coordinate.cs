namespace ScottPlot;

/// <summary>
/// Represents the X/Y location of a point on the plot (in plot units, not pixels)
/// </summary>
public struct Coordinate
{
    public readonly double X;
    public readonly double Y;

    public Coordinate(double x, double y)
    {
        X = x;
        Y = y;
    }

    public Pixel ToPixel(PlotView view) => view.GetPixel(X, Y);

    public override string ToString() => $"(X={X}, Y={Y})";
}