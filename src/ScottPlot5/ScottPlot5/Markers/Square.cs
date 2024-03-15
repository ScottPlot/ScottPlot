namespace ScottPlot.Markers;

internal class Square : IMarker
{
    public bool Fill { get; set; } = true;
    public bool Outline { get; set; } = false;

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        PixelRect rect = new(center: center, radius: size / 2);

        if (Fill)
        {
            fill.ApplyToPaint(paint, new PixelRect(center, size));
            canvas.DrawRect(rect.ToSKRect(), paint);
        }

        if (Outline & outline.CanBeRendered)
        {
            outline.ApplyToPaint(paint);
            canvas.DrawRect(rect.ToSKRect(), paint);
        }
    }
}
