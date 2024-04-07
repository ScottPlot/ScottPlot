using SkiaSharp;

namespace ScottPlot.Extensions;

public static class SkiaSharpExtensions
{
    public static PixelSize ToPixelSize(this SKRect rect)
    {
        return new PixelSize(rect.Width, rect.Height);
    }

    public static Pixel ToPixel(this SKPoint point)
    {
        return new Pixel(point.X, point.Y);
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

    public static void ApplyToPaint(this LineStyle style, SKPaint paint)
    {
        paint.Shader = null;
        paint.IsStroke = true;
        paint.Color = style.Color.ToSKColor();
        paint.StrokeWidth = style.Width;
        paint.PathEffect = style.Pattern.GetPathEffect();
        paint.IsAntialias = style.AntiAlias;
    }

    public static void ApplyToPaint(this FillStyle fs, SKPaint paint, PixelRect rect)
    {
        paint.Color = fs.Color.ToSKColor();
        paint.IsStroke = false;
        paint.IsAntialias = fs.AntiAlias;

        if (fs.Hatch is not null)
        {
            paint.Shader = fs.Hatch.GetShader(fs.Color, fs.HatchColor, rect);
        }
        else
        {
            paint.Shader = null;
        }
    }

    public static void ApplyToPaint(this FontStyle fontStyle, SKPaint paint)
    {
        paint.Shader = null;
        paint.IsStroke = false;
        paint.Typeface = fontStyle.Typeface;
        paint.TextSize = fontStyle.Size;
        paint.Color = fontStyle.Color.ToSKColor();
        paint.IsAntialias = fontStyle.AntiAlias;
        paint.FakeBoldText = fontStyle.Bold;
    }

    /// <summary>
    /// Create a 1 by 256 bitmap displaying all values of a heatmap
    /// </summary>
    public static SKBitmap GetBitmap(this IColormap colormap, bool vertical)
    {
        uint[] argbs = Enumerable.Range(0, 256)
           .Select(i => colormap.GetColor((vertical ? 255 - i : i) / 255f).ARGB)
           .ToArray();

        int bmpWidth = vertical ? 1 : 256;
        int bmpHeight = !vertical ? 1 : 256;

        SKBitmap bmp = Drawing.BitmapFromArgbs(argbs, bmpWidth, bmpHeight);

        return bmp;
    }
}
