namespace ScottPlot.Markers;

internal class None : IMarker
{
    public bool Fill { get; set; } = false;
    public float LineWidth { get; set; } = 0;

    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline)
    {
        // No rendering for none marker
    }
}
