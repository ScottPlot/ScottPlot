using ScottPlot.Style;
using SkiaSharp;

namespace ScottPlot;

/// <summary>
/// Describes text using methods from the new font provider.
/// Holds all customization required to style and draw text.
/// </summary>
public class Label
{
    public string Text { get; set; } = string.Empty;
    public FontStyle Font { get; set; } = new();
    public Color BackgroundColor { get; set; } = Colors.Transparent;
    public LineStyle Border { get; set; } = new() { Width = 0 };
    public LineStyle Outline { get; set; } = new() { Width = 0 };// TODO: respect this

    public bool AntiAlias { get; set; } = true;
    public Alignment Alignment { get; set; } = Alignment.UpperLeft;
    public float Rotation { get; set; } = 0;
    public float PointSize { get; set; } = 0;
    public Color PointColor { get; set; } = Colors.Magenta;

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
            Color = Border.Color.ToSKColor(),
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
        using SKPaint paint = Font.MakePaint();
        SKRect textBounds = new();
        paint.MeasureText(text, ref textBounds);
        return textBounds.ToPixelSize();
    }

    public PixelRect GetRectangle(Pixel pixel)
    {
        using SKPaint paint = Font.MakePaint();
        SKRect textBounds = new();
        paint.MeasureText(Text, ref textBounds);
        return textBounds.ToPixelSize().ToPixelRect(pixel, Alignment);
    }

    public void Draw(SKCanvas canvas, Pixel pixel)
    {
        using SKPaint textPaint = Font.MakePaint();
        textPaint.TextAlign = Alignment.ToSKTextAlign();
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

        if (Border.Width > 0)
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
