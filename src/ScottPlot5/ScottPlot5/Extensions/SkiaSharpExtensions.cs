using ScottPlot.Style;
using SkiaSharp;

namespace ScottPlot.Extensions;

public static class SkiaSharpExtensions
{
    public static PixelSize ToPixelSize(this SKRect rect)
    {
        return new PixelSize(rect.Width, rect.Height);
    }

    public static SKTextAlign ToSKTextAlign(this Alignment alignment)
    {
        return alignment switch
        {
            Alignment.UpperLeft => SKTextAlign.Left,
            Alignment.UpperCenter => SKTextAlign.Center,
            Alignment.UpperRight => SKTextAlign.Right,
            Alignment.MiddleLeft => SKTextAlign.Left,
            Alignment.MiddleCenter => SKTextAlign.Center,
            Alignment.MiddleRight => SKTextAlign.Right,
            Alignment.LowerLeft => SKTextAlign.Left,
            Alignment.LowerCenter => SKTextAlign.Center,
            Alignment.LowerRight => SKTextAlign.Right,
            _ => throw new NotImplementedException(),
        };
    }

    public static SKEncodedImageFormat ToSKFormat(this ImageFormat fmt)
    {
        return fmt switch
        {
            ImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
            ImageFormat.Png => SKEncodedImageFormat.Png,
            ImageFormat.Bmp => SKEncodedImageFormat.Bmp,
            ImageFormat.Webp => SKEncodedImageFormat.Webp,
            _ => throw new NotImplementedException($"unsupported format: {fmt}")
        };
    }

    public static Pixel CenterPixel(this SKBitmap bmp)
    {
        return new Pixel(bmp.Width / 2, bmp.Height / 2);
    }

    public static PixelSize GetPixelSize(this SKSurface surface)
    {
        return new PixelSize(
            width: surface.Canvas.LocalClipBounds.Width,
            height: surface.Canvas.LocalClipBounds.Height);
    }

    public static PixelRect GetPixelRect(this SKSurface surface)
    {
        return new PixelRect(
            left: 0,
            right: surface.Canvas.LocalClipBounds.Width,
            bottom: surface.Canvas.LocalClipBounds.Height,
            top: 0);
    }

    public static PixelRect ToPixelRect(this SKRect rect)
    {
        return new PixelRect(rect.Left, rect.Right, rect.Bottom, rect.Top);
    }

    public static void SetFill(this SKPaint paint, Style.Fill fill, byte alpha = 255)
    {
        paint.Color = fill.Color.WithAlpha(alpha).ToSKColor();
        paint.Shader = fill.GetShader();
        paint.Style = SKPaintStyle.Fill;
    }

    public static SKPaint MakePaint(this LineStyle style, bool antiAlias = true)
    {
        return new SKPaint()
        {
            IsAntialias = antiAlias,
            Style = SKPaintStyle.Stroke,
            Color = style.Color.ToSKColor(),
            StrokeWidth = style.Width,
        };
    }

    public static void ApplyToPaint(this LineStyle lineStyle, SKPaint paint)
    {
        paint.StrokeWidth = lineStyle.Width;
        paint.Color = lineStyle.Color.ToSKColor();
    }

    public static SKFont MakeFont(this FontStyle fontStyle)
    {
        SKFontStyleWeight weight = fontStyle.Bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal;
        SKFontStyleSlant slant = fontStyle.Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;
        SKFontStyleWidth width = SKFontStyleWidth.Normal;
        SKFontStyle skfs = new(weight, width, slant);
        SKTypeface typeface = SKTypeface.FromFamilyName(fontStyle.Name, skfs);
        return new SKFont(typeface, fontStyle.Size);
    }

    public static SKPaint MakePaint(this FontStyle fontStyle, bool antiAlias = true)
    {
        SKFontStyleWeight weight = fontStyle.Bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal;
        SKFontStyleSlant slant = fontStyle.Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;
        SKFontStyleWidth width = SKFontStyleWidth.Normal;
        SKFontStyle skfs = new(weight, width, slant);
        SKTypeface typeface = SKTypeface.FromFamilyName(fontStyle.Name, skfs);
        SKFont skFont = new(typeface, fontStyle.Size);
        return new SKPaint(skFont) { IsAntialias = antiAlias };
    }
}
