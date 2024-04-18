namespace ScottPlot;

public class ArrowStyle
{
    public LineStyle LineStyle { get; set; } = new();

    public ArrowAnchor Anchor { get; set; } = ArrowAnchor.Center;
    public bool IsVisible { get; set; } = true;

    public void Render(SKCanvas canvas, PixelLine line, SKPaint paint)
    {
        if (!IsVisible)
            return;

        using SKPath path = new();
        path.MoveTo(line.Pixel1.ToSKPoint());
        path.LineTo(line.Pixel2.ToSKPoint());

        // TODO: account for rotation
        path.MoveTo(line.Pixel2.WithOffset(5, 5).ToSKPoint());
        path.LineTo(line.Pixel2.ToSKPoint());
        path.LineTo(line.Pixel2.WithOffset(5, -5).ToSKPoint());

        LineStyle.Render(canvas, path, paint);
    }
}
