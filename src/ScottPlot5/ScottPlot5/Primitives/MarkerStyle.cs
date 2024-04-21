namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class MarkerStyle : IHasOutline, IHasFill
{
    public bool IsVisible { get; set; } = true;

    public MarkerShape Shape { get; set; }

    /// <summary>
    /// Diameter of the marker (in pixels)
    /// </summary>
    public float Size { get; set; }


    [Obsolete("use OutlineWidth, OutlineColor, OutlinePattern, or OutlineStyle")]
    public LineStyle Outline { get => OutlineStyle; set => OutlineStyle = value; }
    public LineStyle OutlineStyle { get; set; } = new();
    public float OutlineWidth { get => OutlineStyle.Width; set => OutlineStyle.Width = value; }
    public LinePattern OutlinePattern { get => OutlineStyle.Pattern; set => OutlineStyle.Pattern = value; }
    public Color OutlineColor { get => OutlineStyle.Color; set => OutlineStyle.Color = value; }

    [Obsolete("use FillColor, FillHatchColor, FillHatch, or FillStyle")]
    public FillStyle Fill { get => FillStyle; set => FillStyle = value; }
    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }

    public MarkerStyle()
    {

    }

    public MarkerStyle(MarkerShape shape, float size) : this(shape, size, Colors.Gray)
    {

    }

    public MarkerStyle(MarkerShape shape, float size, Color color)
    {
        Shape = shape;
        OutlineColor = color;
        if (shape.IsOutlined())
        {
            FillColor = Colors.Transparent;
            OutlineWidth = 2;
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
