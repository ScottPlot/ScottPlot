namespace ScottPlot.Markers;

internal class None : IMarker
{
    public void Render(SKCanvas canvas, SKPaintAndFont paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        // No rendering for none marker
    }
}
