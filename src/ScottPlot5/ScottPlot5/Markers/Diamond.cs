namespace ScottPlot.Markers;

internal class Diamond : IMarker
{
    public bool Fill { get; set; } = true;
    public bool Outline { get; set; } = false;

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

        if (Outline && outline.CanBeRendered)
        {
            outline.ApplyToPaint(paint);
            canvas.DrawPath(path, paint);
        }
    }
}
