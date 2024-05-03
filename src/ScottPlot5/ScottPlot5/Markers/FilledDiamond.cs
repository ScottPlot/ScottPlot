namespace ScottPlot.Markers;

internal class FilledDiamond : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float radius = size / 2;

        PixelRect rect = new(center, radius);

        SKPoint[] pointsList =
        {
            new(center.X + radius, center.Y),
            new(center.X, center.Y + radius),
            new(center.X - radius, center.Y),
            new(center.X, center.Y - radius),
        };

        SKPath path = new();
        path.AddPoly(pointsList);

        Drawing.DrawPath(canvas, paint, path, markerStyle.FillStyle, rect);
        Drawing.DrawPath(canvas, paint, path, markerStyle.OutlineStyle);
    }
}
