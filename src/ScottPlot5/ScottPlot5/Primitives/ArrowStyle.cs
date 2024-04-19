namespace ScottPlot;

public class ArrowStyle
{
    public LineStyle LineStyle { get; set; } = new();
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public ArrowAnchor Anchor { get; set; } = ArrowAnchor.Center;

    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Total width of the arrowhead in pixels
    /// </summary>
    public float ArrowheadWidth { get; set; } = 5;

    /// <summary>
    /// Length of the arrowhead in pixels
    /// </summary>
    public float ArrowheadLength { get; set; } = 5;

    /// <summary>
    /// The arrow will always be rendered to be at least this long (in pixels).
    /// If too small, it will gro according to the anchor.
    /// </summary>
    public float MinimumLength { get; set; } = 0;

    /// <summary>
    /// Back the arrow away from its tip along its axis by this many pixels
    /// </summary>
    public float Offset { get; set; } = 0;

    public void Render(SKCanvas canvas, PixelLine line, SKPaint paint)
    {
        if (!IsVisible)
            return;

        using SKPath path = new();
        path.MoveTo(line.Pixel1.ToSKPoint());
        path.LineTo(line.Pixel2.ToSKPoint());

        // TODO: account for rotation
        path.MoveTo(line.Pixel2.WithOffset(ArrowheadLength, ArrowheadWidth).ToSKPoint());
        path.LineTo(line.Pixel2.ToSKPoint());
        path.LineTo(line.Pixel2.WithOffset(ArrowheadLength, -ArrowheadWidth).ToSKPoint());

        LineStyle.Render(canvas, path, paint);
    }
}
