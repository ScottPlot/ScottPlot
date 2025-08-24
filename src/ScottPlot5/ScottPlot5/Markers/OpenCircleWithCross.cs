namespace ScottPlot.Markers;

internal class OpenCircleWithCross : IMarker
{
    public void Render(SKCanvas canvas, SKPaintAndFont paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float offset = size / 2;

        var path = new SKPath();
        path.MoveTo(center.X + offset, center.Y);
        path.LineTo(center.X - offset, center.Y);
        path.MoveTo(center.X, center.Y + offset);
        path.LineTo(center.X, center.Y - offset);

        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);

        float radius = size / 2;
        Drawing.DrawCircle(canvas, center, radius, markerStyle.LineStyle, paint);
    }
}
