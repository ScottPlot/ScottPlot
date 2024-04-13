namespace ScottPlot.Markers;

internal class TriDown : IMarker
{
    public bool Fill { get; set; } = false;
    public float LineWidth { get; set; } = 1;

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        if (LineWidth == 0)
            return;

        // Length of each side of inscribing triangle = size
        float radius = (float)(size / 1.732); // size / sqrt(3)
        float xOffset = (float)(radius * 0.866); // r * sqrt(3)/2
        float yOffset = radius / 2;

        var path = new SKPath();

        // Bottom line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X, center.Y - radius);
        // Left top line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X - xOffset, center.Y + yOffset);
        // Right top line
        path.MoveTo(center.X, center.Y);
        path.LineTo(center.X + xOffset, center.Y + yOffset);

        outline.ApplyToPaint(paint);
        paint.StrokeWidth = LineWidth;
        canvas.DrawPath(path, paint);
    }
}
