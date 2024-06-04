namespace ScottPlot.Markers;

internal class HorizontalBar : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float radius = size / 2;

        SKPath path = new();
        path.MoveTo(center.X + radius, center.Y);
        path.LineTo(center.X - radius, center.Y);

        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);
    }
}
