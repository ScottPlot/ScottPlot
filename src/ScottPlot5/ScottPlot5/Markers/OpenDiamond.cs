namespace ScottPlot.Markers;

internal class OpenDiamond : Marker
{
    public override void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float radius = size / 2;

        SKPoint[] pointsList =
        {
            new(center.X + radius, center.Y),
            new(center.X, center.Y + radius),
            new(center.X - radius, center.Y),
            new(center.X, center.Y - radius),
        };

        SKPath path = new();
        path.AddPoly(pointsList);
        if (markerStyle.Rotate is not null)
        {
            Rotate(path, center, markerStyle.Rotate.Value);
        }

        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);
    }
}
