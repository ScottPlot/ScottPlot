namespace ScottPlot.Markers;

internal class HorizontalBar : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        float offset = size / 2;

        var path = new SKPath();
        path.MoveTo(center.X + offset, center.Y);
        path.LineTo(center.X - offset, center.Y);

        outline.ApplyToPaint(paint);
        canvas.DrawPath(path, paint);
    }
}
