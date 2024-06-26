﻿namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class LineStyle
{
    /// <summary>
    /// Width of the line (in pixels)
    /// </summary>
    public float Width { get; set; } = 0;

    /// <summary>
    /// If enabled, <see cref="Width"/> is ignored and lines are rendered as a single pixel (regardless of scale factor)
    /// </summary>
    public bool Hairline { get; set; } = false;

    /// <summary>
    /// Returns true if <see cref="Width"/> is >0 or <see cref="Hairline"/> is true
    /// </summary>
    public bool RenderedLineHasWidth => Width > 0 || Hairline;

    public Color Color { get; set; } = Colors.Black;

    public LinePattern Pattern { get; set; } = LinePattern.Solid;

    public bool IsVisible { get; set; } = true;
    public bool AntiAlias { get; set; } = true;

    public bool Rounded
    {
        get => StrokeCap == SKStrokeCap.Round;
        set { StrokeCap = SKStrokeCap.Round; StrokeJoin = SKStrokeJoin.Round; }
    }
    public SKStrokeCap StrokeCap { get; set; } = SKStrokeCap.Butt;
    public SKStrokeJoin StrokeJoin { get; set; } = SKStrokeJoin.Miter;
    public float StrokeMiter { get; set; } = 4;

    public static LineStyle None => new() { Width = 0 };

    public void Render(SKCanvas canvas, Pixel[] starts, Pixel[] ends, SKPaint paint)
    {
        if (starts.Length != ends.Length)
            throw new ArgumentException($"{nameof(starts)} and {nameof(ends)} must have equal length");

        using SKPath path = new();

        for (int i = 0; i < starts.Length; i++)
        {
            path.MoveTo(starts[i].X, starts[i].Y);
            path.LineTo(ends[i].X, ends[i].Y);
        }

        Drawing.DrawPath(canvas, paint, path, this);
    }

    public void Render(SKCanvas canvas, PixelLine[] lines, SKPaint paint)
    {
        using SKPath path = new();

        for (int i = 0; i < lines.Length; i++)
        {
            path.MoveTo(lines[i].X1, lines[i].Y1);
            path.LineTo(lines[i].X2, lines[i].Y2);
        }

        Drawing.DrawPath(canvas, paint, path, this);
    }

    public void Render(SKCanvas canvas, PixelLine line, SKPaint paint)
    {
        if (!IsVisible)
            return;

        Drawing.DrawLine(canvas, paint, line, this);
    }

    public void Render(SKCanvas canvas, PixelRect rect, SKPaint paint)
    {
        if (!IsVisible)
            return;

        Pixel[] pixels =
        [
            rect.BottomLeft,
            rect.TopLeft,
            rect.TopRight,
            rect.BottomRight,
            rect.BottomLeft,
        ];

        Drawing.DrawLines(canvas, paint, pixels, this);
    }

    public void Render(SKCanvas canvas, SKPath path, SKPaint paint)
    {
        if (!IsVisible)
            return;

        Drawing.DrawPath(canvas, paint, path, this);
    }

    [Obsolete("use the overload where the paint is passed last")]
    public void Render(SKCanvas canvas, SKPaint paint, PixelLine line)
    {
        if (!IsVisible)
            return;

        Drawing.DrawLine(canvas, paint, line, this);
    }

    public void ApplyToPaint(SKPaint paint)
    {
        paint.Shader = null;
        paint.IsStroke = true;
        paint.Color = Color.ToSKColor();
        paint.StrokeWidth = Hairline ? 1 : Width;
        paint.PathEffect = Pattern.GetPathEffect();
        paint.IsAntialias = AntiAlias;
        paint.StrokeCap = StrokeCap;
        paint.StrokeJoin = StrokeJoin;
        paint.StrokeMiter = StrokeMiter;
    }
}
