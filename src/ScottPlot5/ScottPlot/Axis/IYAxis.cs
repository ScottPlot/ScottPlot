namespace ScottPlot.Axis;

/// <summary>
/// Vertical axis
/// </summary>
public interface IYAxis : IAxis
{
    public double Height { get; }
    public double Bottom { get; set; }
    public double Top { get; set; }
    float PixelWidth { get; }
}
