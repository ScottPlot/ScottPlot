namespace ScottPlot;

public class ArrowStyle : IHasLine, IHasFill
{
    public LineStyle LineStyle { get; set; } = new();
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }

    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }

    public ArrowAnchor Anchor { get; set; } = ArrowAnchor.Center;

    public bool IsVisible { get; set; } = true;

    public float ArrowWidth { get; set; } = 7;
    public float ArrowheadAxisLength { get; set; } = 15;
    public float ArrowheadLength { get; set; } = 20;
    public float ArrowheadWidth { get; set; } = 20;

    /// <summary>
    /// The arrow will always be rendered to be at least this long (in pixels).
    /// If too small, its base will move away from the tip.
    /// </summary>
    public float MinimumLength { get; set; } = 0;

    /// <summary>
    /// The arrow will always be rendered to its length never exceeds this value (in pixels).
    /// If too large, its base will move toward the tip.
    /// </summary>
    public float MaximumLength { get; set; } = float.MaxValue;

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
