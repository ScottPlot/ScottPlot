namespace ScottPlot.Style;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class MarkerStyle
{
    public bool IsVisible => Shape != MarkerShape.None && Size > 0;

    /// <summary>
    /// Standard marker shape.
    /// Ignored when using a custom marker renderer.
    /// </summary>
    public MarkerShape Shape
    {
        get => _Shape;
        set
        {
            _Shape = value;
            MarkerRenderer = value.GetRenderer();
        }
    }

    private MarkerShape _Shape = MarkerShape.Circle;

    /// <summary>
    /// Diameter of the marker (in pixels)
    /// </summary>
    public float Size { get; set; } = 5;

    public IMarkerRenderer MarkerRenderer = new MarkerRenderers.Circle();

    public FillStyle Fill { get; set; } = new() { Color = Colors.Gray };

    public LineStyle Outline { get; set; } = new() { Width = 0 };

    public void Render(SKCanvas canvas, Pixel pixel)
    {
        Render(canvas, new[] { pixel });
    }

    public void Render(SKCanvas canvas, IEnumerable<Pixel> pixels)
    {
        if (!IsVisible)
            return;

        using SKPaint paint = new() { IsAntialias = true };

        foreach (Pixel pixel in pixels)
        {
            MarkerRenderer.Render(canvas, paint, pixel, Size, Fill, Outline);
        }
    }
}
