namespace ScottPlot.Markers;

internal class Square : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        PixelRect rect = new(center: center, radius: size / 2);

        fill.ApplyToPaint(paint);
        canvas.DrawRect(rect.ToSKRect(), paint);

        if (outline.Width > 0)
        {
            outline.ApplyToPaint(paint);
            canvas.DrawRect(rect.ToSKRect(), paint);
        }
    }
}
