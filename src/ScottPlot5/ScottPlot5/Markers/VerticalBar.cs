namespace ScottPlot.Markers;

internal class VerticalBar : IMarker
{
    public bool Fill { get; set; } = false;
    public bool Outline { get; set; } = true;

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        if (!Outline)
            return;

        float offset = size / 2;

        var path = new SKPath();
        path.MoveTo(center.X, center.Y + offset);
        path.LineTo(center.X, center.Y - offset);

        outline.ApplyToPaint(paint);
        canvas.DrawPath(path, paint);
    }
}
