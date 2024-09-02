namespace ScottPlot;

/// <summary>
/// A straight line extending outward from the origin
/// </summary>
public class PolarAxisSpoke(Angle angle, double length) : IHasLine
{
    public Angle Angle { get; set; } = angle;

    public double Length { get; set; } = length;

    public readonly LabelStyle LabelStyle = new();
    public string? LabelText = null;

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

    public void Render(RenderPack rp, IAxes axes, SKPaint paint, double labelDistance, double rotation)
    {
        PolarCoordinates tipPoint = new(Length, Angle);
        Pixel tipPixel = axes.GetPixel(tipPoint.CartesianCoordinates) - axes.GetPixel(Coordinates.Origin);
        Drawing.DrawLine(rp.Canvas, paint, new Pixel(0, 0), tipPixel, LineStyle);

        LabelStyle.Text = LabelText ?? string.Empty;
        LabelStyle.Rotation = -(float)rotation;
        LabelStyle.Alignment = Alignment.MiddleCenter;

        PolarCoordinates labelPoint = new(tipPoint.Radius * labelDistance, tipPoint.Angle);
        Pixel labelPixel = axes.GetPixel(labelPoint.CartesianCoordinates) - axes.GetPixel(Coordinates.Origin);
        LabelStyle.Render(rp.Canvas, labelPixel.WithOffset(0, 0), paint);
    }
}
