using System.Drawing;

namespace ScottPlot;

public class LabelExperimental
{
    public bool IsVisible { get; set; } = true;
    public string Text { get; set; } = string.Empty;

    public Alignment Alignment { get; set; } = 0;

    public float Rotation { get; set; } = 0;

    public Color ForeColor { get; set; } = Colors.Black;

    public Color BackColor { get; set; } = Colors.Gray;

    public Color BorderColor { get; set; } = Colors.Black;
    public float BorderWidth { get; set; } = 1;

    private SKTypeface? CachedTypeface = null;
    private SKTypeface Typeface => CachedTypeface ??= FontStyle.CreateTypeface(FontName, Bold, Italic);
    public string FontName { get; set; } = Fonts.Default;
    public float FontSize { get; set; } = 12;
    public bool Bold = false;
    public bool Italic = false;
    public bool AntiAlias = true;
    public float Padding = 0;

    public float PointSize = 0;
    public bool PointFilled = false;
    public Color PointColor = Colors.Magenta;

    public float OffsetX = 0; // TODO: automatic padding support for arbitrary rotations
    public float OffsetY = 0; // TODO: automatic padding support for arbitrary rotations

    private void ApplyPointPaint(SKPaint paint)
    {
        paint.IsStroke = !PointFilled;
        paint.StrokeWidth = 1;
        paint.Color = PointColor.ToSKColor();
        paint.IsAntialias = AntiAlias;
    }

    private void ApplyBorderPaint(SKPaint paint)
    {
        paint.IsStroke = true;
        paint.StrokeWidth = BorderWidth;
        paint.Color = BorderColor.ToSKColor();
        paint.IsAntialias = AntiAlias;
    }

    private void ApplyBackgroundPaint(SKPaint paint)
    {
        paint.IsStroke = false;
        paint.Color = BackColor.ToSKColor();
        paint.IsAntialias = AntiAlias;
    }

    private void ApplyTextPaint(SKPaint paint)
    {
        paint.TextAlign = SKTextAlign.Left;
        paint.IsStroke = false;
        paint.Typeface = Typeface;
        paint.TextSize = FontSize;
        paint.Color = ForeColor.ToSKColor();
        paint.IsAntialias = AntiAlias;
    }

    public void Render(SKCanvas canvas, Pixel pixel)
    {
        Render(canvas, pixel.X, pixel.Y);
    }

    public void Render(SKCanvas canvas, float x, float y)
    {
        using SKPaint paint = new();
        Render(canvas, x, y, paint);
    }

    public void Render(SKCanvas canvas, float x, float y, SKPaint paint)
    {
        ApplyTextPaint(paint);
        SKRect textBounds = new();
        paint.MeasureText(Text, ref textBounds);

        float xOffset = textBounds.Width * Alignment.HorizontalFraction();
        float yOffset = textBounds.Height * Alignment.VerticalFraction();
        PixelRect textRect = new(0, textBounds.Width, textBounds.Height, 0);
        textRect = textRect.WithDelta(-xOffset, yOffset - textBounds.Height);
        PixelRect backgroundRect = textRect.Expand(Padding);

        canvas.Save();
        canvas.Translate(x + OffsetX, y + OffsetY); // compensate for padding
        canvas.RotateDegrees(Rotation);
        ApplyBackgroundPaint(paint);
        canvas.DrawRect(backgroundRect.ToSKRect(), paint);
        ApplyTextPaint(paint);
        canvas.DrawText(Text, textRect.Left, textRect.Bottom, paint);
        ApplyBorderPaint(paint);
        canvas.DrawRect(backgroundRect.ToSKRect(), paint);
        canvas.Restore();

        canvas.Save();
        canvas.Translate(x, y); // do not compensate for padding
        canvas.RotateDegrees(Rotation);
        ApplyPointPaint(paint);
        canvas.DrawCircle(0, 0, PointSize, paint);
        canvas.DrawLine(-PointSize, 0, PointSize, 0, paint);
        canvas.DrawLine(0, -PointSize, 0, PointSize, paint);
        canvas.Restore();
    }
}
