namespace ScottPlot.Markers;

internal class Diamond : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        float offset = size / 2;

        fill.ApplyToPaint(paint, size, size);

        // 4 corners
        SKPoint[] pointsList = new SKPoint[]
        {
            new SKPoint(center.X + offset, center.Y),
            new SKPoint(center.X, center.Y + offset),
            new SKPoint(center.X - offset, center.Y),
            new SKPoint(center.X, center.Y - offset),

        };

        var path = new SKPath();
        path.AddPoly(pointsList);

        canvas.DrawPath(path, paint);

        if (outline.Width > 0)
        {
            outline.ApplyToPaint(paint);
            canvas.DrawPath(path, paint);
        }
    }
}
