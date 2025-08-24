namespace ScottPlot.Markers;

internal class OpenSquare : IMarker
{
    public void Render(SKCanvas canvas, Paint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        PixelRect rect = new(center, size / 2);

        Drawing.DrawRectangle(canvas, rect, paint, markerStyle.LineStyle);
    }
}
