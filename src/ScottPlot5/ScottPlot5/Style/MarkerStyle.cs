namespace ScottPlot.Style;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class MarkerStyle
{
    public bool IsVisible { get; set; }

    public MarkerStyle(MarkerShape shape = MarkerShape.FilledCircle, float size = 5) : this(shape, size, Colors.Gray)
    { }

    public MarkerStyle(MarkerShape shape, float size, Color color)
    {
        MarkerRenderer = shape.GetRenderer();
        IsVisible = shape != MarkerShape.None;

        Outline.Color = color;
        if (shape.IsOutlined())
        {
            Fill.Color = Colors.Transparent;
            Outline.Width = 2;
        }
        else
        {
            Fill.Color = color;
        }

        Size = size;
    }

    /// <summary>
    /// Diameter of the marker (in pixels)
    /// </summary>
    public float Size { get; set; }

    public IMarkerRenderer MarkerRenderer { get; set; }

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
