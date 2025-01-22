﻿namespace ScottPlot.Plottables;

public class Ellipse : IPlottable, IHasLine, IHasFill, IHasLegendText
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(this, LegendText, LineStyle);

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public FillStyle FillStyle { get; set; } = new() { Color = Colors.Transparent };
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }


    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

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
    public Angle Rotation { get; set; } = Angle.FromDegrees(0);

    /// <summary>
    /// if false, it is an elliptical arc
    /// If true, it is an elliptical sector;
    /// </summary>
    public bool IsSector { get; set; } = true;

    /// <summary>
    /// Start angle of elliptical arc or elliptical sector (degrees)
    /// </summary>
    public Angle StartAngle { get; set; } = Angle.FromDegrees(0);

    /// <summary>
    /// Sweep angle of elliptical arc or elliptical sector (degrees)
    /// </summary>
    public Angle SweepAngle { get; set; } = Angle.FromDegrees(360);

    public AxisLimits GetAxisLimits()
    {
        if (Rotation.Normalized.Degrees == 0)
        {
            return new(Center.ToRect(RadiusX, RadiusY));
        }

        // https://math.stackexchange.com/a/91304

        var rad = -Rotation.Normalized.Radians;
        var cos2 = Math.Pow(Math.Cos(rad), 2);
        var sin2 = Math.Pow(Math.Sin(rad), 2);

        var a2 = Math.Pow(RadiusX, 2);
        var b2 = Math.Pow(RadiusY, 2);

        var x = Math.Sqrt(a2 * cos2 + b2 * sin2);
        var y = Math.Sqrt(a2 * sin2 + b2 * cos2);

        return new(Center.ToRect(x, y));
    }

    private static bool IsFinite(double x) => !(double.IsInfinity(x) || double.IsNaN(x));
    private bool RadiusIsNotFinite => !IsFinite(RadiusX) || !IsFinite(RadiusY);

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible || RadiusIsNotFinite)
            return;

        using var paint = new SKPaint();

        rp.Canvas.Translate(Axes.GetPixel(Center).ToSKPoint());
        rp.Canvas.RotateDegrees((float)-Rotation.Normalized.Degrees);

        float rx = Axes.GetPixelX(RadiusX) - Axes.GetPixelX(0);
        float ry = Axes.GetPixelY(RadiusY) - Axes.GetPixelY(0);

        PixelRect rect = new(-rx, rx, ry, -ry);

        if (SweepAngle.Normalized.Degrees == 0 ||
            Math.Abs(SweepAngle.Normalized.Degrees) >= 360)
        {
            Drawing.FillOval(rp.Canvas, paint, FillStyle, rect);
            Drawing.DrawOval(rp.Canvas, paint, LineStyle, rect);
        }
        else
        {
            double startAngle = StartAngle.Normalized.Degrees;
            if (IsSector)
            {
                Drawing.FillSector(rp.Canvas, paint, FillStyle, rect,
                    (float)-startAngle, (float)-SweepAngle.Degrees);
                Drawing.DrawSector(rp.Canvas, paint, LineStyle, rect,
                    (float)-startAngle, (float)-SweepAngle.Degrees);
            }
            else
            {
                Drawing.DrawArc(rp.Canvas, paint, LineStyle, rect,
                    (float)-startAngle, (float)-SweepAngle.Degrees);
            }
        }
    }
}
