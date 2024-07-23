namespace ScottPlot.Markers;

internal class None : Marker
{
    public override void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        // No rendering for none marker
    }
}
