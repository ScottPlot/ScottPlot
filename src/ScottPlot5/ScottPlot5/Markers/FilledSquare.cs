namespace ScottPlot.Markers;

internal class FilledSquare : IMarker
{
    public void Render(SKCanvas canvas, SKPaintAndFont paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        PixelRect rect = new(center, size / 2);

        Drawing.FillRectangle(canvas, rect, paint, markerStyle.FillStyle);
        Drawing.DrawRectangle(canvas, rect, paint, markerStyle.OutlineStyle);
    }
}
