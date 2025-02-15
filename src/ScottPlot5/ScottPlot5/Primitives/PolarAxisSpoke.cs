namespace ScottPlot;

/// <summary>
/// A straight line extending outward from the origin
/// </summary>
public class PolarAxisSpoke(Angle angle, double length) : IHasLine
{
    public Angle Angle { get; set; } = angle;

    public double Length { get; set; } = length;

    public LabelStyle LabelStyle { get; } = new();
    public string? LabelText { get; set; } = null;
    public double LabelPaddingFraction { get; set; } = 0.2;
    public double LabelLength => Length * (1 + LabelPaddingFraction);

    public LineStyle LineStyle { get; set; } = new()
    {
        Width = 1,
        Color = Colors.Black.WithAlpha(.5),
    };

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
