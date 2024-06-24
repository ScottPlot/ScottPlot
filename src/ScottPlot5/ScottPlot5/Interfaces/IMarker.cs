namespace ScottPlot;

/// <summary>
/// Describes logic necessary to render a marker at a point
/// </summary>
public interface IMarker
{
    void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle);
}
