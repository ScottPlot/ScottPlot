namespace ScottPlot.Markers;

internal class FilledSquare : Marker
{
    public override void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        PixelRect rect = new(center, size / 2);

        SKPath path = new();
        path.AddRect(rect.ToSKRect());
        if (markerStyle.Rotate is not null)
        {
            Rotate(path, center, markerStyle.Rotate.Value);
        }

        Drawing.DrawPath(canvas, paint, path, markerStyle.FillStyle, rect);
        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);
    }
}
