namespace ScottPlot.Markers;

internal class OpenSquare : IMarker
{
    public void Render(SKCanvas canvas, SKPaintAndFont paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        PixelRect rect = new(center, size / 2);

        Drawing.DrawRectangle(canvas, rect, paint, markerStyle.LineStyle);
    }
}
