namespace ScottPlot.Markers;

internal class TriUp : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        // Length of each side of inscribing triangle = size
        float radius = (float)(size / 1.732); // size / sqrt(3)
        float xOffset = (float)(radius * 0.866); // r * sqrt(3)/2
        float yOffset = radius / 2;

        SKPath path = new();

        // Top line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X, center.Y + radius);

        // Left bottom line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X - xOffset, center.Y - yOffset);

        // Right bottom line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X + xOffset, center.Y - yOffset);

        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);
    }
}
