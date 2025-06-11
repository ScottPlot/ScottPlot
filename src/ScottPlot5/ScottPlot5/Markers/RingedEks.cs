namespace ScottPlot.Markers;

internal class RingedEks : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float radius = size / 2;

        SKPath path = new();
        float eksRadius = (float)Math.Sqrt(radius * radius / 2);
        path.MoveTo(center.X + eksRadius, center.Y + eksRadius);
        path.LineTo(center.X - eksRadius, center.Y - eksRadius);
        path.MoveTo(center.X - eksRadius, center.Y + eksRadius);
        path.LineTo(center.X + eksRadius, center.Y - eksRadius);

        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);
        Drawing.DrawCircle(canvas, center, radius, markerStyle.LineStyle, paint);
    }
}
