namespace ScottPlot.Markers;

internal class FilledSquare : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        PixelRect rect = new(center, size / 2);

        Drawing.DrawRectangle(canvas, rect, paint, markerStyle.FillStyle);

        if (markerStyle.FillOutline)
            Drawing.DrawRectangle(canvas, rect, paint, markerStyle.LineStyle);
    }
}
