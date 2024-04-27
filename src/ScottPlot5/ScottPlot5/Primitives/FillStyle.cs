namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class FillStyle
{
    public Color Color { get; set; } = Colors.Transparent;
    public Color HatchColor { get; set; } = Colors.Gray;
    public IHatch? Hatch { get; set; } = null;
    public bool HasValue => Color != Colors.Transparent || Hatch is not null && HatchColor != Colors.Transparent;
    public bool AntiAlias { get; set; } = true;
    public bool IsVisible { get; set; } = true;

    public void Render(SKCanvas canvas, PixelRect rect, SKPaint paint)
    {
        if (!IsVisible)
            return;

        Drawing.FillRectangle(canvas, rect, paint, this);
    }

    public void ApplyToPaint(SKPaint paint, PixelRect rect)
    {
        paint.Color = Color.ToSKColor();
        paint.IsStroke = false;
        paint.IsAntialias = AntiAlias;

        if (Hatch is not null)
        {
            paint.Shader = Hatch.GetShader(Color, HatchColor, rect);
        }
        else
        {
            paint.Shader = null;
        }
    }
}
