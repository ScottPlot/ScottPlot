namespace ScottPlot;

/// <summary>
/// Represents the X/Y location of a point on the screen (in pixel units, not plot units)
/// </summary>
public struct Pixel
{
    public readonly float X;
    public readonly float Y;

    public Pixel(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Coordinate ToCoordinate(PlotView view) => view.GetCoordinate(X, Y);

    public override string ToString() => $"[X={X}, Y={Y}]";
}