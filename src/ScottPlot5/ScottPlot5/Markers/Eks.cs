namespace ScottPlot.Markers;

internal class Eks : Marker
{
    public override void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float radius = size / 2;

        SKPath path = new();
        path.MoveTo(center.X + radius, center.Y + radius);
        path.LineTo(center.X - radius, center.Y - radius);
        path.MoveTo(center.X - radius, center.Y + radius);
        path.LineTo(center.X + radius, center.Y - radius);

        if (markerStyle.Rotate is not null)
        {
            Rotate(path, center, markerStyle.Rotate.Value);
        }

        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);
    }
}

