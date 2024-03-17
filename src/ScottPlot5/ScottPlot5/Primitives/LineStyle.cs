namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class LineStyle
{
    public float Width { get; set; } = 1.0f;
    public Color Color { get; set; } = Colors.Black;
    public LinePattern Pattern { get; set; } = LinePattern.Solid;
    public bool IsVisible { get; set; } = true; // TODO: deprecate in favor of 0 line width
    public static LineStyle None => new() { IsVisible = false, Width = 0 };
    public bool AntiAlias { get; set; } = true;
    public bool CanBeRendered => IsVisible && Width > 0 && Color.Alpha > 0;

    public static LineStyle DefaultMajorStyle => new()
    {
        Width = 1,
        Color = Colors.Black.WithOpacity(.1)
    };

    public static LineStyle DefaultMinorStyle => new()
    {
        Width = 0,
        Color = Colors.Black.WithOpacity(.05)
    };

    public void Render(SKCanvas canvas, SKPaint paint, PixelLine line)
    {
        if (CanBeRendered == false) return;

        this.ApplyToPaint(paint);
        canvas.DrawLine(line.X1, line.Y1, line.X2, line.Y2, paint);
    }
}
