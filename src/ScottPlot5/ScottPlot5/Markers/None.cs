namespace ScottPlot.Markers;

internal class None : IMarker
{
    public void Render(SKCanvas canvas, Paint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        // No rendering for none marker
    }
}
