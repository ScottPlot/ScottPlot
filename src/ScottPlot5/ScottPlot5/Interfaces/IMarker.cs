namespace ScottPlot;

/// <summary>
/// Describes logic necessary to render a marker at a point
/// </summary>
public interface IMarker
{
    bool Fill { get; set; }
    bool Outline { get; set; }
    void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline);
}
