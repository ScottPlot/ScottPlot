namespace ScottPlot.Markers;

internal class HorizontalBar : IMarker
{
    public bool Fill { get; set; } = false;
    public float LineWidth { get; set; } = 1;

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        if (LineWidth == 0)
            return;

        float offset = size / 2;

        var path = new SKPath();
        path.MoveTo(center.X + offset, center.Y);
        path.LineTo(center.X - offset, center.Y);

        outline.ApplyToPaint(paint);
        paint.StrokeWidth = LineWidth;
        canvas.DrawPath(path, paint);
    }
}
