namespace ScottPlot.Markers;

internal class Cross : Marker
{
    public override void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float offset = size / 2;

        var path = new SKPath();
        path.MoveTo(center.X + offset, center.Y);
        path.LineTo(center.X - offset, center.Y);
        path.MoveTo(center.X, center.Y + offset);
        path.LineTo(center.X, center.Y - offset);

        if (markerStyle.Rotate is not null)
        {
            Rotate(path, center, markerStyle.Rotate.Value);
        }

        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);
    }
}
