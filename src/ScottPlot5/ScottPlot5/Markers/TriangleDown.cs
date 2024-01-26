namespace ScottPlot.Markers;

internal class TriangleDown : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        // Length of each side of triangle = size
        float radius = (float)(size / 1.732); // size / sqrt(3)
        float xOffset = (float)(radius * 0.866); // r * sqrt(3)/2
        float yOffset = radius / 2;

        fill.ApplyToPaint(paint, new PixelRect(center, size));

        // Bottom, right, and left vertices
        SKPoint[] pointsList = new SKPoint[]
        {
            new SKPoint(center.X, center.Y + radius),
            new SKPoint(center.X + xOffset, center.Y - yOffset),
            new SKPoint(center.X - xOffset, center.Y - yOffset),
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
