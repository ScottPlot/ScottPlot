namespace ScottPlot.Axis;

/// <summary>
/// Horizontal axis
/// </summary>
public interface IXAxis : IAxis
{
    public double Width { get; }
    public double Left { get; set; }
    public double Right { get; set; }
    float PixelHeight { get; }
}
