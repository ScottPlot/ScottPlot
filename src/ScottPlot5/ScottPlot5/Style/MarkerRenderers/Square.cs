namespace ScottPlot.Style.MarkerRenderers;

internal class Square : IMarkerRenderer
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        PixelRect rect = PixelRect.Centered(center, radius: size / 2);

        fill.ApplyToPaint(paint);
        canvas.DrawRect(rect.ToSKRect(), paint);

        if (outline.Width > 0)
        {
            outline.ApplyToPaint(paint);
            canvas.DrawRect(rect.ToSKRect(), paint);
        }
    }
}
