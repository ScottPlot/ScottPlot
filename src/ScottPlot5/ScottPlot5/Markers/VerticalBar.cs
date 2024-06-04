namespace ScottPlot.Markers;

internal class VerticalBar : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float offset = size / 2;

        SKPath path = new();
        path.MoveTo(center.X, center.Y + offset);
        path.LineTo(center.X, center.Y - offset);

        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);
    }
}
