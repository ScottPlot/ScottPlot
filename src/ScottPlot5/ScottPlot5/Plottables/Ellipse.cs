using ScottPlot.Extensions;

namespace ScottPlot.Plottables;

public class Ellipse : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, LineStyle);

    public LineStyle LineStyle { get; set; } = new() { Color = Colors.Black, Width = 2 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public FillStyle FillStyle { get; set; } = new() { Color = Colors.Transparent };
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }

    /// <summary>
    /// Label to appear in the legend
    /// </summary>
    public string Label { get; set; } = string.Empty;

    public Coordinates Center = Coordinates.Origin;

    /// <summary>
    /// Horizontal radius (axis units)
    /// </summary>
    public double RadiusX
    {
        get => _radiusX;
        set => _radiusX = Math.Abs(value);
    }
    private double _radiusX = 10;

    /// <summary>
    /// Horizontal radius (axis units)
    /// </summary>
    public double RadiusY
    {
        get => _radiusY;
        set => _radiusY = Math.Abs(value);
    }
    private double _radiusY = 10;

    /// <summary>
    /// Rotation of the ellipse (degrees)
    /// </summary>
    public double Rotation
    {
        get => _rotation;
        set
        {
            if (value < 0) { value += 360; }
            _rotation = value % 360;
        }
    }
    private double _rotation = 0;

    public AxisLimits GetAxisLimits()
    {
        if (Rotation == 0)
        {
            return new(Center.ToRect(RadiusX, RadiusY));
        }

        // https://math.stackexchange.com/a/91304

        var rad = Rotation.ToRadians();
        var cos2 = Math.Pow(Math.Cos(rad), 2);
        var sin2 = Math.Pow(Math.Sin(rad), 2);

        var a2 = Math.Pow(RadiusX, 2);
        var b2 = Math.Pow(RadiusY, 2);

        var x = Math.Sqrt(a2 * cos2 + b2 * sin2);
        var y = Math.Sqrt(a2 * sin2 + b2 * cos2);

        return new(Center.ToRect(x, y));
    }

    public void Render(RenderPack rp)
    {
        if (!IsVisible || RadiusX.IsInfiniteOrNaN() || RadiusY.IsInfiniteOrNaN()) { return; }

        using var paint = new SKPaint();

        rp.Canvas.Translate(Axes.GetPixel(Center).ToSKPoint());
        rp.Canvas.RotateDegrees((float)Rotation);

        float rx = Axes.GetPixelX(RadiusX) - Axes.GetPixelX(0);
        float ry = Axes.GetPixelY(RadiusY) - Axes.GetPixelY(0);

        PixelRect rect = new(-rx, rx, ry, -ry);
        Drawing.FillOval(rp.Canvas, paint, FillStyle, rect);
        Drawing.DrawOval(rp.Canvas, paint, LineStyle, rect);

        rp.Canvas.Restore();
    }
}
