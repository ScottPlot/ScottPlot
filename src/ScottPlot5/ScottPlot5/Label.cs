using SkiaSharp;

namespace ScottPlot;

/// <summary>
/// Describes text using methods from the new font provider.
/// Holds all customization required to style and draw text.
/// </summary>
public class Label // TODO: rename later
{
    public string Text { get; set; } = string.Empty;
    public Font Font { get; set; } = Font.Default;
    public string FontName { get => Font.Name; set => Font = Font.WithName(value); }
    public float FontSize { get => Font.Size; set => Font = Font.WithSize(value); }
    public FontWeight FontWeight { get => Font.GetNearestWeight(); set => Font = Font.WithWeight(value); }
    public bool Bold { get => Font.Weight > (float)FontWeight.Normal; set => Font = Font.WithBold(value); }
    public bool Italic { get => Font.Italic; set => Font = Font.WithItalic(value); }
    public Color Color { get; set; } = Colors.Black;
    public Color BackgroundColor { get; set; } = Colors.Transparent;
    public Color BorderColor { get; set; } = Colors.Black;
    public float BorderWidth { get; set; } = 0;
    public bool Outline { get; set; } = false;
    public float OutlineWidth { get; set; } = 0;
    public bool AntiAlias { get; set; } = true;
    public Alignment Alignment { get; set; } = Alignment.UpperLeft;
    public float Rotation = 0;

    public SKPaint MakePaint()
    {
        return new SKPaint()
        {
            Color = Color.ToSKColor(),
            TextAlign = Alignment.ToSKTextAlign(),
            Typeface = SKTypeface.FromFamilyName(FontName),
            TextSize = FontSize,
            IsAntialias = AntiAlias,
        };
    }

    public PixelSize Measure()
    {
        return Measure(Text);
    }

    public PixelSize Measure(string text)
    {
        using SKPaint paint = MakePaint();
        SKRect textBounds = new();
        paint.MeasureText(text, ref textBounds);
        return textBounds.ToPixelSize();
    }

    public PixelRect GetRectangle(Pixel pixel)
    {
        using SKPaint paint = MakePaint();
        SKRect textBounds = new();
        paint.MeasureText(Text, ref textBounds);
        return textBounds.ToPixelSize().ToPixelRect(pixel, Alignment);
    }

    public PixelRect Draw(SKCanvas canvas, Pixel pixel)
    {
        using SKPaint paint = MakePaint();
        SKRect textBounds = new();
        paint.MeasureText(Text, ref textBounds);
        float yOffset = textBounds.Height * Alignment.VerticalFraction();
        SKPoint pt = new(pixel.X, pixel.Y + yOffset);
        canvas.DrawText(Text, pt, paint);
        return textBounds.ToPixelSize().ToPixelRect(pixel, Alignment);
    }
}
