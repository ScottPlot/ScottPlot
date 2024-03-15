namespace ScottPlot.Markers;

internal class Circle : IMarker
{
    public bool Fill { get; set; } = true;
    public bool Outline { get; set; } = false;

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        float radius = size / 2;

        if (Fill)
        {
            fill.ApplyToPaint(paint, new PixelRect(center, size));
            canvas.DrawCircle(center.ToSKPoint(), radius, paint);
        }

        if (Outline && outline.CanBeRendered)
        {
            outline.ApplyToPaint(paint);
            canvas.DrawCircle(center.ToSKPoint(), radius, paint);
        }
    }
}
