namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class MarkerStyle
{
    public bool IsVisible { get; set; } = true;

    public MarkerShape Shape { get; set; }

    /// <summary>
    /// Diameter of the marker (in pixels)
    /// </summary>
    public float Size { get; set; }

    public FillStyle Fill { get; set; } = new();

    public LineStyle Outline { get; set; } = new();

    public MarkerStyle()
    {

    }

    public MarkerStyle(MarkerShape shape, float size) : this(shape, size, Colors.Gray)
    {

    }

    public MarkerStyle(MarkerShape shape, float size, Color color)
    {
        Shape = shape;
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

    public static MarkerStyle Default => new(MarkerShape.FilledCircle, 5);

    public static MarkerStyle None => new(MarkerShape.None, 0);

    public void Render(SKCanvas canvas, Pixel px, SKPaint paint)
    {
        if (!IsVisible)
            return;

        Drawing.DrawMarker(canvas, paint, px, this);
    }
}
