namespace ScottPlot.Style;

/// <summary>
/// Describes logic necessary to render a marker at a point
/// </summary>
public interface IMarkerRenderer
{
    void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline);
}
