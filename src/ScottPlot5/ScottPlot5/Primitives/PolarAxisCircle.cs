namespace ScottPlot;

/// <summary>
/// A polar axis tick describes the radius of a circle centered at the origin 
/// and includes the styling information required to render it
/// </summary>
public class PolarAxisCircle(double radius) : IHasLine
{
    public double Radius { get; set; } = radius;
    public Coordinates Origin { get; set; } = new();
    public Angle StartAngle { get; set; } = Angle.FromDegrees(0);
    public Angle SweepAngle { get; set; } = Angle.FromDegrees(360);

    public LabelStyle LabelStyle { get; set; } = new()
    {
        Alignment = Alignment.LowerLeft,
        OffsetX = 3,
    };

    public string LabelText { get; set; } = string.Empty;

    public Angle LabelAngle { get; set; } = Angle.FromDegrees(0);

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
