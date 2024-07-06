namespace ScottPlot;

/// <summary>
/// Line on radial axis
/// </summary>
public class Spoke(double angle, double length, LineStyle? lineStyle = null) :
    IHasLine
{
    /// <summary>
    /// Line angle (degrees)
    /// </summary>
    public double Angle { get; set; } = angle;

    /// <summary>
    /// Line length
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
}
