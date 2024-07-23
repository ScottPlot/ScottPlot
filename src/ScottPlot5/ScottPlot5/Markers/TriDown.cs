namespace ScottPlot.Markers;

internal class TriDown : Marker
{
    public override void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        // Length of each side of inscribing triangle = size
        float radius = (float)(size / 1.732); // size / sqrt(3)
        float xOffset = (float)(radius * 0.866); // r * sqrt(3)/2
        float yOffset = radius / 2;

        SKPath path = new();

        // Bottom line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X, center.Y - radius);

        // Left top line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X - xOffset, center.Y + yOffset);

        // Right top line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X + xOffset, center.Y + yOffset);

        if (markerStyle.Rotate is not null)
        {
            Rotate(path, center, markerStyle.Rotate.Value);
        }

        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);
    }
}
