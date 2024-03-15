namespace ScottPlot.Markers;

internal class Diamond : IMarker
{
    public bool Fill { get; set; } = true;
    public float LineWidth { get; set; } = 1;

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        float offset = size / 2;

        // 4 corners
        SKPoint[] pointsList =
        {
            new(center.X + offset, center.Y),
            new(center.X, center.Y + offset),
            new(center.X - offset, center.Y),
            new(center.X, center.Y - offset),
        };

        var path = new SKPath();
        path.AddPoly(pointsList);

        if (Fill)
        {
            fill.ApplyToPaint(paint, new PixelRect(center, size));
            canvas.DrawPath(path, paint);
        }

        if (LineWidth > 0)
        {
            outline.ApplyToPaint(paint);
            paint.StrokeWidth = LineWidth;
            canvas.DrawPath(path, paint);
        }
    }
}
