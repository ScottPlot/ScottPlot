namespace ScottPlot.Markers;

internal class TriangleDown : IMarker
{
    public bool Fill { get; set; } = true;
    public float LineWidth { get; set; } = 1;

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        // Length of each side of triangle = size
        float radius = (float)(size / 1.732); // size / sqrt(3)
        float xOffset = (float)(radius * 0.866); // r * sqrt(3)/2
        float yOffset = radius / 2;

        // Bottom, right, and left vertices
        SKPoint[] pointsList =
        {
            new (center.X, center.Y + radius),
            new (center.X + xOffset, center.Y - yOffset),
            new (center.X - xOffset, center.Y - yOffset),
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
