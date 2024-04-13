namespace ScottPlot;

/// <summary>
/// Describes logic necessary to render a marker at a point
/// </summary>
public interface IMarker
{
    /// <summary>
    /// if the marker has a fill, this flag indicates whether it will be rendered
    /// </summary>
    bool Fill { get; set; }

    /// <summary>
    /// Line width (overriding the one in LineStyle).
    /// Set to 0 to disable line rendering.
    /// </summary>
    float LineWidth { get; set; }

    // TODO: don't accept FillStyle or LineStyle, but instead MarkerStyle which has all that stuff
    void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, FillStyle fill, LineStyle outline);
}
