namespace ScottPlot.PolarAxes;

/// <summary>
/// Axis line on radial axis in polar coordinates
/// </summary>
public class Spoke(double angle, double length, LineStyle? lineStyle = null) :
    IHasLine
{
    /// <summary>
    /// Angle(degrees) in polar coordinate system
    /// </summary>
    public double Angle { get; set; } = angle;

    /// <summary>
    /// Length from the origin
    /// </summary>
    public double Length { get; set; } = length;

    public LineStyle LineStyle { get; set; } = lineStyle ?? new();

    public float LineWidth
    {
        get => LineStyle.Width;
        set => LineStyle.Width = value;
    }

    public LinePattern LinePattern
    {
        get => LineStyle.Pattern;
        set => LineStyle.Pattern = value;
    }

    public Color LineColor
    {
        get => LineStyle.Color;
        set => LineStyle.Color = value;
    }

    public PolarCoordinates GetPolarCoordinates()
    {
        return new PolarCoordinates(Length, Angle);
    }
}
