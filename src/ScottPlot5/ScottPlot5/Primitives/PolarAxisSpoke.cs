namespace ScottPlot;

/// <summary>
/// A straight line extending outward from the origin
/// </summary>
public class PolarAxisSpoke(double degrees, double length) : IHasLine
{
    public double Degrees { get; set; } = degrees;

    public double Length { get; set; } = length;

    LabelStyle LabelStyle = new();

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

    public PolarCoordinates GetPolarCoordinates()
    {
        return new PolarCoordinates(Length, Degrees);
    }

    public void Render(RenderPack rp, IAxes axes, SKPaint paint, double labelDistance)
    {
        PolarCoordinates polar = GetPolarCoordinates();
        Pixel pixel = axes.GetPixel(polar) - axes.GetPixel(Coordinates.Origin);
        Drawing.DrawLine(rp.Canvas, paint, new Pixel(0, 0), pixel, LineStyle);

        LabelStyle.Text = $"{Degrees}";
        PolarCoordinates labPolar = new(polar.Radial * labelDistance, polar.Angle);
        Pixel labelPixel = axes.GetPixel(labPolar) - axes.GetPixel(Coordinates.Origin);
        PixelRect labelRect = LabelStyle.Measure().Rect(Alignment.MiddleCenter);
        Pixel labelOffset = labelRect.Center - labelRect.TopLeft;
        labelPixel -= labelOffset;
        LabelStyle.Render(rp.Canvas, labelPixel, paint);
    }
}
