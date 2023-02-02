using System.Runtime.InteropServices;

namespace ScottPlot;

/// <summary>
/// Common operations using the default rendering system.
/// </summary>
public static class Drawing
{
    public static PixelSize MeasureString(string text, SKPaint paint)
    {
        var strings = text.Split('\n');
        if (strings.Length > 1)
        {
            return strings
                .Select(s => MeasureString(s, paint))
                .Aggregate((a, b) => new PixelSize(Math.Max(a.Width, b.Width), a.Height + b.Height));
        }

        SKRect bounds = new();
        paint.MeasureText(text, ref bounds);

        float width = bounds.Width;
        float height = bounds.Height;
        return new PixelSize(width, height);
    }

    public static PixelSize MeasureLargestString(string[] strings, SKPaint paint)
    {
        float maxWidth = 0;
        float maxHeight = 0;

        for (int i = 0; i < strings.Length; i++)
        {
            PixelSize tickSize = MeasureString(strings[i], paint);
            maxWidth = Math.Max(maxWidth, tickSize.Width);
            maxHeight = Math.Max(maxHeight, tickSize.Height);
        }

        return new PixelSize(maxWidth, maxHeight);
    }

    public static void DrawLines(SKSurface surface, Pixel[] starts, Pixel[] ends, Color color, float width = 1, bool antiAlias = true)
    {
        if (starts.Length != ends.Length)
            throw new ArgumentException($"{nameof(starts)} and {nameof(ends)} must have same length");

        using SKPaint paint = new()
        {
            Color = color.ToSKColor(),
            IsStroke = true,
            IsAntialias = antiAlias,
            StrokeWidth = width,
        };

        using SKPath path = new();

        for (int i = 0; i < starts.Length; i++)
        {
            path.MoveTo(starts[i].X, starts[i].Y);
            path.LineTo(ends[i].X, ends[i].Y);
        }

        surface.Canvas.DrawPath(path, paint);
    }

    public static void Fillectangle(SKCanvas canvas, PixelRect rect, Color color)
    {
        using SKPaint paint = new()
        {
            Color = color.ToSKColor(),
            IsStroke = false,
            IsAntialias = true,
        };

        canvas.DrawRect(rect.ToSKRect(), paint);
    }

    public static void DrawRectangle(SKCanvas canvas, PixelRect rect, Color color, float thickness = 1)
    {
        using SKPaint paint = new()
        {
            Color = color.ToSKColor(),
            IsStroke = true,
            StrokeWidth = thickness,
            IsAntialias = true,
        };

        canvas.DrawRect(rect.ToSKRect(), paint);
    }

    public static void DrawDebugRectangle(SKCanvas canvas, PixelRect rect, Pixel point, Color color, float thickness = 1)
    {
        using SKPaint paint = new()
        {
            Color = color.ToSKColor(),
            IsStroke = true,
            StrokeWidth = thickness,
            IsAntialias = true,
        };

        canvas.DrawRect(rect.ToSKRect(), paint);
        canvas.DrawLine(rect.BottomLeft.ToSKPoint(), rect.TopRight.ToSKPoint(), paint);
        canvas.DrawLine(rect.TopLeft.ToSKPoint(), rect.BottomRight.ToSKPoint(), paint);

        canvas.DrawCircle(point.ToSKPoint(), 5, paint);

        paint.IsStroke = false;
        paint.Color = paint.Color.WithAlpha(20);
        canvas.DrawRect(rect.ToSKRect(), paint);
    }

    public static void DrawCircle(SKCanvas canvas, Pixel center, Color color, float radius = 5, bool fill = true)
    {
        using SKPaint paint = new()
        {
            Color = color.ToSKColor(),
            IsStroke = !fill,
            IsAntialias = true,
        };

        canvas.DrawCircle(center.ToSKPoint(), radius, paint);
    }

    public static SKBitmap BitmapFromArgbs(uint[] argbs, int width, int height)
    {
        GCHandle handle = GCHandle.Alloc(argbs, GCHandleType.Pinned);

        var imageInfo = new SKImageInfo(width, height);
        var bmp = new SKBitmap(imageInfo);
        bmp.InstallPixels(
            info: imageInfo,
            pixels: handle.AddrOfPinnedObject(),
            rowBytes: imageInfo.RowBytes,
            releaseProc: (IntPtr _, object _) => handle.Free());

        return bmp;
    }

    public static SKColorFilter GetMaskColorFilter(Color foreground, Color? background = null)
    {
        // This function and the math is explained here: https://bclehmann.github.io/2022/11/06/UnmaskingWithSKColorFilter.html

        background ??= Colors.Black;

        float redDifference = foreground.Red - background.Value.Red;
        float greenDifference = foreground.Green - background.Value.Green;
        float blueDifference = foreground.Blue - background.Value.Blue;
        float alphaDifference = foreground.Alpha - background.Value.Alpha;

        // See https://learn.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/effects/color-filters
        // for an explanation of this matrix
        // 
        // Essentially, this matrix maps all gray colours to a line from `background.Value` to `foreground`.
        // Black and white are at the extremes on this line, 
        // so they get mapped to `background.Value` and `foreground` respectively
        var mat = new float[] {
                redDifference / 255, 0, 0, 0, background.Value.Red / 255.0f,
                0, greenDifference / 255, 0, 0, background.Value.Green / 255.0f,
                0, 0, blueDifference / 255, 0, background.Value.Blue / 255.0f,
                alphaDifference / 255, 0, 0, 0, background.Value.Alpha / 255.0f,
            };

        var filter = SKColorFilter.CreateColorMatrix(mat);
        return filter;
    }
}
