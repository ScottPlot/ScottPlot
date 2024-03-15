namespace ScottPlot.Markers;

internal class Circle : IMarker
{
    public bool Fill { get; set; } = true;
    public float LineWidth { get; set; } = 0;

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        float radius = size / 2;

        if (Fill)
        {
            fill.ApplyToPaint(paint, new PixelRect(center, size));
            canvas.DrawCircle(center.ToSKPoint(), radius, paint);
        }

        if (LineWidth > 0)
        {
            outline.ApplyToPaint(paint);
            paint.StrokeWidth = LineWidth;
            canvas.DrawCircle(center.ToSKPoint(), radius, paint);
        }
    }
}
