namespace ScottPlot.Markers;

internal class None : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        // No rendering for none marker
    }
}