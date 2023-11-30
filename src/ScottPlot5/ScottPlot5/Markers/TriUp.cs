using ScottPlot.AxisPanels;

namespace ScottPlot.Markers;

internal class TriUp : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        // Length of each side of inscribing triangle = size
        float radius = (float)(size / 1.732); // size / sqrt(3)
        float xOffset = (float)(radius * 0.866); // r * sqrt(3)/2
        float yOffset = radius / 2;

        fill.ApplyToPaint(paint);

        // Top, right, and left vertices
        SKPoint[] pointsList = new SKPoint[]
        {
            new SKPoint(center.X, center.Y - radius),
            new SKPoint(center.X + xOffset, center.Y + yOffset),
            new SKPoint(center.X - xOffset, center.Y + yOffset),
        };

        var path = new SKPath();
        // Top line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X, center.Y + radius);
        // Left bottom line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X - xOffset, center.Y - yOffset);
        // Right bottom line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X + xOffset, center.Y - yOffset);

        outline.ApplyToPaint(paint);
        canvas.DrawPath(path, paint);
    }
}
