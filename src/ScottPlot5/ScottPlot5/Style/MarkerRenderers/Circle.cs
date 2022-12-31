namespace ScottPlot.Style.MarkerRenderers;

internal class Circle : IMarkerRenderer
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        float radius = size / 2;

        fill.ApplyToPaint(paint);
        canvas.DrawCircle(center.ToSKPoint(), radius, paint);

        if (outline.Width > 0)
        {
            outline.ApplyToPaint(paint);
            canvas.DrawCircle(center.ToSKPoint(), radius, paint);
        }
    }
}
