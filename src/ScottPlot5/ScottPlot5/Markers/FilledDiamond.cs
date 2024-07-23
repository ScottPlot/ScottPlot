namespace ScottPlot.Markers;

internal class FilledDiamond : Marker
{
    public override void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
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
        if (markerStyle.Rotate is not null)
        {
            Rotate(path, center, markerStyle.Rotate.Value);
        }

        Drawing.DrawPath(canvas, paint, path, markerStyle.FillStyle, rect);
        Drawing.DrawPath(canvas, paint, path, markerStyle.OutlineStyle);
    }
}
