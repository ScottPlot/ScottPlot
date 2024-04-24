namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class MarkerStyle : IHasLine, IHasFill
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

    // the line is for shapes like open circles
    public LineStyle LineStyle { get; set; } = new();
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }
    public IMarker? CustomRenderer { get; set; } = null;

    #region obsolete

    [Obsolete("use LineWidth, LineColor, etc.")]
    public LineStyle Outline { get => LineStyle; set => LineStyle = value; }

    [Obsolete("use LineWidth, LineColor, etc.")]
    public LineStyle OutlineStyle { get => LineStyle; set => LineStyle = value; }

    [Obsolete("use LineWidth, LineColor, etc.")]
    public float OutlineWidth { get => OutlineStyle.Width; set => OutlineStyle.Width = value; }

    [Obsolete("use LineWidth, LineColor, etc.")]
    public LinePattern OutlinePattern { get => OutlineStyle.Pattern; set => OutlineStyle.Pattern = value; }

    [Obsolete("use LineWidth, LineColor, etc.")]
    public Color OutlineColor { get => OutlineStyle.Color; set => OutlineStyle.Color = value; }



    [Obsolete("use FillColor, FillHatchColor, FillHatch, or FillStyle")]
    public FillStyle Fill { get => FillStyle; set => FillStyle = value; }

    #endregion

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
