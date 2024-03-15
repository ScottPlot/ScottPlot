namespace ScottPlot.Markers;

internal class Square : IMarker
{
    public bool Fill { get; set; } = true;
    public float LineWidth { get; set; } = 0;

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        PixelRect rect = new(center: center, radius: size / 2);

        if (Fill)
        {
            fill.ApplyToPaint(paint, new PixelRect(center, size));
            canvas.DrawRect(rect.ToSKRect(), paint);
        }

        if (LineWidth > 0)
        {
            outline.ApplyToPaint(paint);
            paint.StrokeWidth = LineWidth;
            canvas.DrawRect(rect.ToSKRect(), paint);
        }
    }
}
