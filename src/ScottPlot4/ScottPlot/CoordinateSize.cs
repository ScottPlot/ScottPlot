namespace ScottPlot;

/// <summary>
/// Describes a size in coordinate space
/// </summary>
public struct CoordinateSize
{
    public double Width;
    public double Height;

    public CoordinateSize(double width, double height)
    {
        Width = width;
        Height = height;
    }
}
