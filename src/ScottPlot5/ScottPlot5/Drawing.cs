using System.Runtime.InteropServices;

namespace ScottPlot;

// TODO: obsolete methods in this class that create paints. Pass paints in to minimize allocations at render time.

// TODO: obsolete methods that don't take Style objects

/// <summary>
/// Common operations using the default rendering system.
/// </summary>
public static class Drawing
{
    public static void DrawLine(SKCanvas canvas, SKPaint paint, PixelLine pixelLine)
    {
        DrawLine(canvas, paint, pixelLine.Pixel1, pixelLine.Pixel2);
    }

    public static void DrawLine(SKCanvas canvas, SKPaint paint, Pixel pt1, Pixel pt2)
    {
        canvas.DrawLine(pt1.ToSKPoint(), pt2.ToSKPoint(), paint);
    }

    public static void DrawLine(SKCanvas canvas, SKPaint paint, PixelLine pxLine, LineStyle lineStyle)
    {
        DrawLine(canvas, paint, pxLine.Pixel1, pxLine.Pixel2, lineStyle);
    }

    public static void DrawLines(SKCanvas canvas, SKPaint paint, IEnumerable<PixelLine> pxLines, LineStyle lineStyle)
    {
        foreach (PixelLine line in pxLines)
        {
            DrawLine(canvas, paint, line.Pixel1, line.Pixel2, lineStyle);
        }
    }

    public static void DrawLine(SKCanvas canvas, SKPaint paint, Pixel pt1, Pixel pt2, LineStyle lineStyle)
    {
        if (!lineStyle.CanBeRendered) return;

        lineStyle.ApplyToPaint(paint);
        if (lineStyle.Hairline)
            paint.StrokeWidth = 1f / canvas.TotalMatrix.ScaleX;

        canvas.DrawLine(pt1.ToSKPoint(), pt2.ToSKPoint(), paint);
    }

    public static void DrawLine(SKCanvas canvas, SKPaint paint, Pixel pt1, Pixel pt2, Color color, float width = 1, bool antiAlias = true, LinePattern pattern = default)
    {
        if (width == 0)
            return;

        paint.Color = color.ToSKColor();
        paint.IsStroke = true;
        paint.IsAntialias = antiAlias;
        paint.StrokeWidth = width;
        paint.PathEffect = pattern.GetPathEffect();

        canvas.DrawLine(pt1.ToSKPoint(), pt2.ToSKPoint(), paint);
    }

    public static void DrawPath(SKCanvas canvas, SKPaint paint, IEnumerable<Pixel> pixels, LineStyle lineStyle, bool close = false)
    {
        if (!lineStyle.CanBeRendered) return;

        using SKPath path = new();
        path.MoveTo(pixels.First().ToSKPoint());
        foreach (Pixel px in pixels.Skip(1))
        {
            path.LineTo(px.ToSKPoint());
        }

        if (close)
        {
            path.LineTo(pixels.First().ToSKPoint());
        }

        DrawPath(canvas, paint, path, lineStyle);
    }

    public static void FillPath(SKCanvas canvas, SKPaint paint, IEnumerable<Pixel> pixels, FillStyle fillStyle, PixelRect rect)
    {
        if (!fillStyle.CanBeRendered) return;

        using SKPath path = new();
        path.MoveTo(pixels.First().ToSKPoint());
        foreach (Pixel px in pixels.Skip(1))
        {
            path.LineTo(px.ToSKPoint());
        }

        fillStyle.ApplyToPaint(paint, rect);
        canvas.DrawPath(path, paint);
    }

    public static void DrawPath(SKCanvas canvas, SKPaint paint, IEnumerable<Pixel> pixels, LineStyle lineStyle, string label, LabelStyle labelStyle, bool close = false)
    {
        if (!lineStyle.CanBeRendered) return;

        using SKPath path = new();
        path.MoveTo(pixels.First().ToSKPoint());
        foreach (Pixel px in pixels.Skip(1))
        {
            path.LineTo(px.ToSKPoint());
        }

        if (close)
        {
            path.LineTo(pixels.First().ToSKPoint());
        }
        DrawPath(canvas, paint, path, lineStyle);

        if (labelStyle.IsVisible == false)
            return;

        labelStyle.ApplyToPaint(paint);

        var measuredText = paint.MeasureText(label);
        using (SKPathMeasure pathMeasure = new SKPathMeasure(path, false, 1))
            DrawTextOnPath(canvas, paint, path, label, pathMeasure.Length / 4 - measuredText / 4, 0);
    }

    public static void DrawTextOnPath(SKCanvas canvas, SKPaint paint, SKPath path, string text, float hOffset = 0, float vOffset = 0)
    {
        if (string.IsNullOrEmpty(text))
            return;
        canvas.DrawTextOnPath(text, path, hOffset, vOffset, paint);
    }

    public static void DrawPath(SKCanvas canvas, SKPaint paint, PixelPath path, LineStyle lineStyle)
    {
        DrawPath(canvas, paint, path.Pixels, lineStyle);
    }
    public static void DrawPath(SKCanvas canvas, SKPaint paint, PixelPath path, LineStyle lineStyle, string text, LabelStyle labelStyle)
    {
        DrawPath(canvas, paint, path.Pixels, lineStyle, text, labelStyle);
    }

    public static void DrawPath(SKCanvas canvas, SKPaint paint, PixelPath path, FillStyle fillStyle)
    {
        DrawPath(canvas, paint, path.Pixels, fillStyle);
    }

    public static void DrawPath(SKCanvas canvas, SKPaint paint, IEnumerable<Pixel> pixels, FillStyle fillStyle)
    {
        if (fillStyle.IsVisible == false || fillStyle.Color == Colors.Transparent)
            return;

        float xMin = pixels.First().X;
        float xMax = pixels.First().X;
        float yMin = pixels.First().Y;
        float yMax = pixels.First().Y;

        using SKPath path = new();
        path.MoveTo(pixels.First().ToSKPoint());
        foreach (Pixel px in pixels.Skip(1))
        {
            path.LineTo(px.ToSKPoint());
            xMin = Math.Min(xMin, px.X);
            xMax = Math.Max(xMax, px.X);
            yMin = Math.Min(yMin, px.Y);
            yMax = Math.Max(yMax, px.Y);
        }

        PixelRect rect = new(xMin, xMax, yMax, yMin);
        DrawPath(canvas, paint, path, fillStyle, rect);
    }

    public static void DrawPath(SKCanvas canvas, SKPaint paint, SKPath path, LineStyle lineStyle)
    {
        if (!lineStyle.CanBeRendered) return;

        lineStyle.ApplyToPaint(paint);
        if (lineStyle.Hairline)
            paint.StrokeWidth = 1f / canvas.TotalMatrix.ScaleX;

        canvas.DrawPath(path, paint);
    }

    public static void DrawPath(SKCanvas canvas, SKPaint paint, SKPath path, FillStyle fillStyle, PixelRect rect)
    {
        if (fillStyle.IsVisible == false || fillStyle.Color == Colors.Transparent)
            return;

        fillStyle.ApplyToPaint(paint, rect);
        canvas.DrawPath(path, paint);
    }

    private static readonly IPathStrategy StraightLineStrategy = new PathStrategies.Straight();

    public static void DrawLines(SKCanvas canvas, SKPaint paint, IEnumerable<Pixel> pixels, LineStyle lineStyle)
    {
        if (!lineStyle.CanBeRendered) return;
        if (pixels.Take(2).Count() < 2) return;

        lineStyle.ApplyToPaint(paint);
        if (lineStyle.Hairline)
            paint.StrokeWidth = 1f / canvas.TotalMatrix.ScaleX;

        using SKPath path = StraightLineStrategy.GetPath(pixels);
        canvas.DrawPath(path, paint);
    }

    public static void DrawLines(SKCanvas canvas, SKPaint paint, SKPath path, LineStyle lineStyle)
    {
        if (!lineStyle.CanBeRendered) return;

        lineStyle.ApplyToPaint(paint);
        if (lineStyle.Hairline)
            paint.StrokeWidth = 1f / canvas.TotalMatrix.ScaleX;

        canvas.DrawPath(path, paint);
    }

    public static void FillRectangle(SKCanvas canvas, PixelRect rect, SKPaint paint, FillStyle fillStyle)
    {
        fillStyle.ApplyToPaint(paint, rect);
        canvas.DrawRect(rect.ToSKRect(), paint);
    }

    public static void FillRectangle(SKCanvas canvas, PixelRect rect, Color color)
    {
        if (color == Colors.Transparent)
            return;

        using SKPaint paint = new()
        {
            Color = color.ToSKColor(),
            IsStroke = false,
            IsAntialias = true,
        };

        canvas.DrawRect(rect.ToSKRect(), paint);
    }

    public static void DrawRectangle(SKCanvas canvas, PixelRect rect, SKPaint paint, LineStyle lineStyle)
    {
        if (!lineStyle.CanBeRendered) return;

        lineStyle.ApplyToPaint(paint);
        if (lineStyle.Hairline)
            paint.StrokeWidth = 1f / canvas.TotalMatrix.ScaleX;

        canvas.DrawRect(rect.ToSKRect(), paint);
    }

    public static void DrawRectangle(SKCanvas canvas, PixelRect rect, SKPaint paint, FillStyle fillStyle)
    {
        if (!fillStyle.IsVisible) return;
        if (fillStyle.Color == Colors.Transparent) return;

        fillStyle.ApplyToPaint(paint, rect);
        canvas.DrawRect(rect.ToSKRect(), paint);
    }

    public static void DrawRectangle(SKCanvas canvas, PixelRect rect, SKPaint paint)
    {
        canvas.DrawRect(rect.ToSKRect(), paint);
    }

    public static void DrawRectangle(SKCanvas canvas, PixelRect rect, Color color, float lineWidth = 1)
    {
        if (color == Colors.Transparent || lineWidth == 0)
            return;

        using SKPaint paint = new()
        {
            Color = color.ToSKColor(),
            IsStroke = true,
            StrokeWidth = lineWidth,
            IsAntialias = true,
        };

        DrawRectangle(canvas, rect, paint);
    }

    public static void DrawDebugRectangle(SKCanvas canvas, PixelRect rect, Pixel? point = null, Color? color = null, float lineWidth = 3)
    {
        point ??= Pixel.NaN;
        color ??= Colors.Magenta;

        using SKPaint paint = new()
        {
            Color = color.Value.ToSKColor(),
            IsStroke = true,
            StrokeWidth = lineWidth,
            IsAntialias = true,
        };

        canvas.DrawRect(rect.ToSKRect(), paint);
        canvas.DrawLine(rect.BottomLeft.ToSKPoint(), rect.TopRight.ToSKPoint(), paint);
        canvas.DrawLine(rect.TopLeft.ToSKPoint(), rect.BottomRight.ToSKPoint(), paint);

        canvas.DrawCircle(point.Value.ToSKPoint(), 5, paint);

        paint.IsStroke = false;
        paint.Color = paint.Color.WithAlpha(20);
        canvas.DrawRect(rect.ToSKRect(), paint);
    }

    public static void DrawDebugPoint(SKCanvas canvas, Pixel point, Color? color = null, float size = 3)
    {
        color ??= Colors.Magenta;

        using SKPaint paint = new()
        {
            Color = color.Value.ToSKColor(),
            IsAntialias = true,
        };

        canvas.DrawCircle(point.ToSKPoint(), size, paint);
    }

    public static void DrawCircle(SKCanvas canvas, Pixel center, float radius, LineStyle lineStyle, SKPaint paint)
    {
        if (!lineStyle.CanBeRendered) return;

        lineStyle.ApplyToPaint(paint);
        if (lineStyle.Hairline)
            paint.StrokeWidth = 1f / canvas.TotalMatrix.ScaleX;

        canvas.DrawCircle(center.ToSKPoint(), radius, paint);
    }

    public static void DrawCircle(SKCanvas canvas, Pixel center, float radius, FillStyle fillStyle, SKPaint paint)
    {
        if (!fillStyle.IsVisible) return;
        if (fillStyle.Color == Colors.Transparent) return;

        PixelRect rect = new(center, radius);
        fillStyle.ApplyToPaint(paint, rect);
        canvas.DrawCircle(center.ToSKPoint(), radius, paint);
    }

    public static void DrawOval(SKCanvas canvas, SKPaint paint, LineStyle lineStyle, PixelRect rect)
    {
        if (!lineStyle.CanBeRendered) return;

        lineStyle.ApplyToPaint(paint);
        if (lineStyle.Hairline)
            paint.StrokeWidth = 1f / canvas.TotalMatrix.ScaleX;

        canvas.DrawOval(rect.ToSKRect(), paint);
    }

    public static void FillOval(SKCanvas canvas, SKPaint paint, FillStyle fillStyle, PixelRect rect)
    {
        if (!fillStyle.IsVisible) return;
        if (fillStyle.Color == Colors.Transparent) return;

        fillStyle.ApplyToPaint(paint, rect);
        canvas.DrawOval(rect.ToSKRect(), paint);
    }

    public static void DrawMarker(SKCanvas canvas, SKPaint paint, Pixel pixel, MarkerStyle style)
    {
        if (!style.IsVisible)
            return;

        IMarker marker = style.CustomRenderer ?? style.Shape.GetMarker();

        marker.Render(canvas, paint, pixel, style.Size, style);
    }

    public static void DrawMarkers(SKCanvas canvas, SKPaint paint, IEnumerable<Pixel> pixels, MarkerStyle style)
    {
        if (!style.IsVisible)
            return;

        IMarker marker = style.CustomRenderer ?? style.Shape.GetMarker();

        foreach (Pixel pixel in pixels)
        {
            marker.Render(canvas, paint, pixel, style.Size, style);
        }
    }

    public static void DrawMarkers(SKCanvas canvas, SKPaint paint, IReadOnlyList<Pixel> pixels, MarkerStyle style, IColormap colormap)
    {
        if (!style.IsVisible)
            return;

        IMarker marker = style.CustomRenderer ?? style.Shape.GetMarker();

        for (int i = 0; i < pixels.Count; i++)
        {
            Pixel pixel = pixels[i];
            style.MarkerColor = colormap.GetColor(i, pixels.Count);
            marker.Render(canvas, paint, pixel, style.Size, style);
        }
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
        // Essentially, this matrix maps all gray colors to a line from `background.Value` to `foreground`.
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

    public static SKSurface CreateSurface(int width, int height)
    {
        SKImageInfo imageInfo = new(
            width: width,
            height: height,
            colorType: SKColorType.Rgba8888,
            alphaType: SKAlphaType.Premul);

        return SKSurface.Create(imageInfo);
    }

    public static void SavePng(SKSurface surface, string filename)
    {
        new Image(surface).SavePng(filename);
    }

    public static void DrawImage(SKCanvas canvas, Image image, PixelRect target, SKPaint paint, bool antiAlias = true)
    {
        image.Render(canvas, target, paint, antiAlias);
    }
}
