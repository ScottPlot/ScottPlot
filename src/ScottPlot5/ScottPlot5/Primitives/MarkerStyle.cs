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

    public FillStyle Fill { get; set; } = new() { Color = Colors.Gray };

    public LineStyle Outline { get; set; } = new() { Width = 1 };

    public bool CanBeRendered => IsVisible && Shape != MarkerShape.None && Size > 0 && (Shape.IsOutlined() ? Outline.CanBeRendered : Fill.Color.A > 0);
    public MarkerStyle() : this(MarkerShape.FilledCircle, 5, Colors.Gray)
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
}
