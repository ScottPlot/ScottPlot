namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class LineStyle
{
    public float Width { get; set; } = 0;
    public Color Color { get; set; } = Colors.Black;
    public LinePattern Pattern { get; set; } = LinePattern.Solid;

    public bool IsVisible { get; set; } = true;
    public bool AntiAlias { get; set; } = true;

    public bool Rounded
    {
        get => StrokeCap == SKStrokeCap.Round;
        set { StrokeCap = SKStrokeCap.Round; StrokeJoin = SKStrokeJoin.Round; }
    }
    public SKStrokeCap StrokeCap = SKStrokeCap.Butt;
    public SKStrokeJoin StrokeJoin = SKStrokeJoin.Miter;

    public float StrokeMiter = 4;

    [Obsolete("Use explicit logic", true)]
    public bool CanBeRendered => IsVisible && Width > 0 && Color.Alpha > 0;

    public static LineStyle None => new() { Width = 0 };

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
        paint.StrokeWidth = Width;
        paint.PathEffect = Pattern.GetPathEffect();
        paint.IsAntialias = AntiAlias;
        paint.StrokeCap = StrokeCap;
        paint.StrokeJoin = StrokeJoin;
        paint.StrokeMiter = StrokeMiter;
    }
}
