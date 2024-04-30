namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class MarkerStyle : IHasLine, IHasFill, IHasOutline
{
    public bool IsVisible { get; set; } = true;

    public MarkerShape Shape { get; set; }

    /// <summary>
    /// Diameter of the marker (in pixels)
    /// </summary>
    public float Size { get; set; }

    public Color MarkerColor
    {
        get => LineColor; set
        {
            FillColor = value;
            LineColor = value;
        }
    }

    // Line style is for shapes whose lines make up the shape (e.g., HashTag)
    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    // The fill style is for filled shapes (e.g., filled circle)
    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }
    public IMarker? CustomRenderer { get; set; } = null;

    [Obsolete("This property is obsolete. An outline will be drawn if OutlineWidth width is greater than 0.", true)]
    public bool FillOutline { get; set; }

    // An outline may be drawn around markers with filled areas (e.g., around a circle)
    [Obsolete("use OutlineWidth, OutlineColor, OutlineStyle, etc", true)]
    public LineStyle Outline { get => OutlineStyle; set => OutlineStyle = value; }
    public LineStyle OutlineStyle { get; set; } = new() { Color = Colors.Black, Width = 0 };
    public float OutlineWidth { get => OutlineStyle.Width; set => OutlineStyle.Width = value; }
    public LinePattern OutlinePattern { get => OutlineStyle.Pattern; set => OutlineStyle.Pattern = value; }
    public Color OutlineColor { get => OutlineStyle.Color; set => OutlineStyle.Color = value; }

    [Obsolete("use FillColor, FillHatchColor, FillHatch, or FillStyle", true)]
    public FillStyle Fill { get => FillStyle; set => FillStyle = value; }

    public MarkerStyle()
    {

    }

    public MarkerStyle(MarkerShape shape, float size) : this(shape, size, Colors.Gray)
    {

    }

    public MarkerStyle(MarkerShape shape, float size, Color color)
    {
        Shape = shape;
        LineColor = color;
        if (shape.IsOutlined())
        {
            FillColor = Colors.Transparent;
            LineWidth = 2;
        }
        else
        {
            FillColor = color;
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
