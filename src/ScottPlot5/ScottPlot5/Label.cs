using SkiaSharp;

namespace ScottPlot;

/// <summary>
/// Describes text using methods from the new font provider.
/// Holds all customization required to style and draw text.
/// </summary>
public class Label
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
    public float Rotation { get; set; } = 0;
    public float PointSize { get; set; } = 0;
    public Color PointColor { get; set; } = Colors.Magenta;

    private SKPaint MakeTextPaint()
    {
        SKFontStyleWeight weight = Bold ? SKFontStyleWeight.SemiBold : SKFontStyleWeight.Normal;
        SKFontStyleWidth width = SKFontStyleWidth.Normal;
        SKFontStyleSlant slant = Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;

        return new SKPaint()
        {
            Color = Color.ToSKColor(),
            TextAlign = Alignment.ToSKTextAlign(),
            Typeface = SKTypeface.FromFamilyName(FontName, weight, width, slant),
            TextSize = FontSize,
            IsAntialias = AntiAlias,
            FilterQuality = AntiAlias ? SKFilterQuality.High : SKFilterQuality.Low,
            //FakeBoldText = Bold,
        };
    }

    private SKPaint MakeBackgroundPaint()
    {
        return new SKPaint()
        {
            IsStroke = false,
            Color = BackgroundColor.ToSKColor(),
            IsAntialias = AntiAlias,
            FilterQuality = AntiAlias ? SKFilterQuality.High : SKFilterQuality.Low,
        };
    }

    private SKPaint MakeBorderPaint()
    {
        return new SKPaint()
        {
            IsStroke = true,
            Color = BorderColor.ToSKColor(),
            IsAntialias = AntiAlias,
            FilterQuality = AntiAlias ? SKFilterQuality.High : SKFilterQuality.Low,
        };
    }

    private SKPaint MakePointPaint()
    {
        return new SKPaint()
        {
            IsStroke = false,
            Color = PointColor.ToSKColor(),
            IsAntialias = AntiAlias,
            FilterQuality = AntiAlias ? SKFilterQuality.High : SKFilterQuality.Low,
        };
    }

    public PixelSize Measure()
    {
        return Measure(Text);
    }

    public PixelSize Measure(string text)
    {
        using SKPaint paint = MakeTextPaint();
        SKRect textBounds = new();
        paint.MeasureText(text, ref textBounds);
        return textBounds.ToPixelSize();
    }

    public PixelRect GetRectangle(Pixel pixel)
    {
        using SKPaint paint = MakeTextPaint();
        SKRect textBounds = new();
        paint.MeasureText(Text, ref textBounds);
        return textBounds.ToPixelSize().ToPixelRect(pixel, Alignment);
    }

    public void Draw(SKCanvas canvas, Pixel pixel)
    {
        using SKPaint textPaint = MakeTextPaint();
        SKRect textBounds = new();
        textPaint.MeasureText(Text, ref textBounds);
        float xOffset = textBounds.Width * Alignment.HorizontalFraction();
        float yOffset = textBounds.Height * Alignment.VerticalFraction();
        PixelRect textRect = new(0, textBounds.Width, textBounds.Height, 0);
        textRect = textRect.WithDelta(-xOffset, yOffset - textBounds.Height);

        canvas.Save();
        canvas.Translate(pixel.ToSKPoint());
        canvas.RotateDegrees(Rotation);

        if (BackgroundColor.Alpha > 0)
        {
            using SKPaint backgroundPaint = MakeBackgroundPaint();
            canvas.DrawRect(textRect.ToSKRect(), backgroundPaint);
        }

        canvas.DrawText(Text, new(0, yOffset), textPaint);

        if (BorderWidth > 0)
        {
            using SKPaint borderPaint = MakeBorderPaint();
            canvas.DrawRect(textRect.ToSKRect(), borderPaint);
        }

        if (PointSize > 0)
        {
            using SKPaint pointPaint = MakePointPaint();
            canvas.DrawCircle(new SKPoint(0, 0), PointSize, pointPaint);
        }

        canvas.Restore();
    }
}
