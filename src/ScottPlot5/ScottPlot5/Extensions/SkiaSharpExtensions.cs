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

    public static void SetStroke(this SKPaint paint, Style.Stroke stroke)
    {
        paint.StrokeWidth = (float)stroke.Width;
        paint.Color = stroke.Color.ToSKColor();
        paint.Style = SKPaintStyle.Stroke;

        paint.Shader = null;
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
}
