namespace ScottPlot.PolarAxes;

/// <summary>
/// Axis line on circular axis in polar coordinates
/// </summary>
public class CircularAxisLine(double value, LineStyle? lineStyle = null) :
    IHasLine
{
    /// <summary>
    /// Representative values of the axis line
    /// </summary>
    public double Value { get; set; } = value;

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
