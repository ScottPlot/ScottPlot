using System.Diagnostics;
using ScottPlot.Axes;
using ScottPlot.AxisViews;
using SkiaSharp;

namespace ScottPlot;

public class Plot
{
    // TODO: store these in lists to support multi-axis plots
    internal readonly IXAxis XAxis;
    internal readonly IYAxis YAxis;
    internal readonly IAxisView LeftAxisView;
    internal readonly IAxisView BottomAxisView;

    readonly List<IPlottable> Plottables = new();

    public IGrid Grid = new Grids.DefaultGrid();

    // TODO: allow the user to inject their own visual debugging and performance monitoring tools
    public readonly Plottables.DebugBenchmark Benchmark = new();

    // TODO: allow the user to inject their own visual debugging and performance monitoring tools
    public readonly Plottables.ZoomRectangle ZoomRectangle;

    /// <summary>
    /// Any state stored across renders can be stored here.
    /// </summary>
    internal RenderInformation LastRenderInfo = new(false);

    public Plot()
    {
        XAxis = new LinearXAxis();
        YAxis = new LinearYAxis();

        LeftAxisView = new LeftAxisView(YAxis);
        BottomAxisView = new BottomAxisView(XAxis);

        ZoomRectangle = new(XAxis, YAxis);
    }

    public void Add(IPlottable plottable)
    {
        Plottables.Add(plottable);
    }

    public void Clear()
    {
        Plottables.Clear();
    }

    public IPlottable[] GetPlottables()
    {
        return Plottables.ToArray();
    }

    public void SetAxisLimits(double left, double right, double bottom, double top)
    {
        XAxis.Left = left;
        XAxis.Right = right;
        XAxis.HasBeenSet = true;

        YAxis.Bottom = bottom;
        YAxis.Top = top;
        YAxis.HasBeenSet = true;
    }

    public void SetAxisLimits(CoordinateRect rect)
    {
        SetAxisLimits(rect.XMin, rect.XMax, rect.YMin, rect.YMax);
    }

    public void SetAxisLimits(AxisLimits rect)
    {
        SetAxisLimits(rect.Rect);
    }

    /// <summary>
    /// Automatically scale the axis limits to fit the data.
    /// Note: This used to be AxisAuto().
    /// </summary>
    /// <param name="xMargin">pad each side of the data area with this fraction of empty space</param>
    /// <param name="yMargin">pad each side of the data area with this fraction of empty space</param>
    public void AutoScale(double xMargin = .05, double yMargin = .1)
    {
        if (xMargin < 0 || xMargin > 1)
            throw new ArgumentException($"{nameof(xMargin)} must be within the range [0, 1] ");

        if (yMargin < 0 || yMargin > 1)
            throw new ArgumentException($"{nameof(yMargin)} must be within the range [0, 1] ");

        AxisLimits limits = AxisLimits.NoLimits;

        foreach (var plottable in Plottables)
        {
            limits.Expand(plottable.GetAxisLimits());
        }

        if (double.IsNaN(limits.Rect.XMin))
            limits.Rect.XMin = -10;
        if (double.IsNaN(limits.Rect.XMax))
            limits.Rect.XMax = +10;
        if (double.IsNaN(limits.Rect.YMin))
            limits.Rect.YMin = -10;
        if (double.IsNaN(limits.Rect.YMax))
            limits.Rect.YMax = +10;

        SetAxisLimits(limits.WithZoom(1 - xMargin, 1 - yMargin));
    }

    /// <summary>
    /// Apply a click-drag pan operation to the plot
    /// </summary>
    public void MousePan(AxisLimits originalLimits, Pixel mouseDown, Pixel mouseNow)
    {
        double pxPerUnitx = LastRenderInfo.DataRect.Width / XAxis.Width;
        double pxPerUnity = LastRenderInfo.DataRect.Height / YAxis.Height;

        float pixelDeltaX = mouseNow.X - mouseDown.X;
        float pixelDeltaY = mouseNow.Y - mouseDown.Y;

        double deltaX = pixelDeltaX / pxPerUnitx;
        double deltaY = pixelDeltaY / pxPerUnity;

        // pan in the direction opposite of the mouse movement
        SetAxisLimits(originalLimits.WithPan(-deltaX, deltaY));
    }

    /// <summary>
    /// Apply a click-drag zoom operation to the plot
    /// </summary>
    public void MouseZoom(AxisLimits originalLimits, Pixel mouseDown, Pixel mouseNow)
    {
        float pixelDeltaX = mouseNow.X - mouseDown.X;
        float pixelDeltaY = mouseNow.Y - mouseDown.Y;

        double deltaFracX = pixelDeltaX / (Math.Abs(pixelDeltaX) + LastRenderInfo.DataRect.Width);
        double fracX = Math.Pow(10, deltaFracX);

        double deltaFracY = -pixelDeltaY / (Math.Abs(pixelDeltaY) + LastRenderInfo.DataRect.Height);
        double fracY = Math.Pow(10, deltaFracY);

        SetAxisLimits(originalLimits.WithZoom(fracX, fracY));
    }

    /// <summary>
    /// Zoom into the coordinate corresponding to the given pixel.
    /// Fractional values >1 zoom in and <1 zoom out.
    /// </summary>
    public void MouseZoom(double fracX, double fracY, Pixel pixel)
    {
        Coordinate mouseCoordinate = GetCoordinate(pixel);
        SetAxisLimits(GetAxisLimits().WithZoom(fracX, fracY, mouseCoordinate.X, mouseCoordinate.Y));
    }

    public void MouseZoomRectangle(Pixel mouseDown, Pixel mouseNow)
    {
        Coordinate downCoordinate = GetCoordinate(mouseDown);
        Coordinate nowCoordinate = GetCoordinate(mouseNow);
        CoordinateRect rect = new(downCoordinate, nowCoordinate);
        ZoomRectangle.SetSize(rect);
    }

    /// <summary>
    /// Indicate the click-drag zoom rectangle was dropped.
    /// Applying the zoom will set axis limits to where the rectangle was before it was dropped.
    /// </summary>
    public void MouseZoomRectangleClear(bool applyZoom)
    {
        if (applyZoom)
        {
            SetAxisLimits(ZoomRectangle.Rect);
        }

        ZoomRectangle.Clear();
    }

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(XAxis.Left, XAxis.Right, YAxis.Bottom, YAxis.Top);
    }

    /// <summary>
    /// Return the pixel for a specific coordinate using measurements from the most recent render.
    /// </summary>
    public Pixel GetPixel(Coordinate coord)
    {
        PixelRect dataRect = LastRenderInfo.DataRect;
        float x = XAxis.GetPixel(coord.X, dataRect);
        float y = YAxis.GetPixel(coord.Y, dataRect);
        return new Pixel(x, y);
    }

    /// <summary>
    /// Return the coordinate for a specific pixel using measurements from the most recent render.
    /// </summary>
    public Coordinate GetCoordinate(Pixel pixel)
    {
        PixelRect dataRect = LastRenderInfo.DataRect;
        double x = XAxis.GetCoordinate(pixel.X, dataRect);
        double y = YAxis.GetCoordinate(pixel.Y, dataRect);
        return new Coordinate(x, y);
    }

    public PixelRect GetDataAreaRect(PixelRect figureRect)
    {
        LeftAxisView.RegenerateTicks(figureRect);
        float padLeft = LeftAxisView.Measure();

        BottomAxisView.RegenerateTicks(figureRect);
        float padBottom = BottomAxisView.Measure();

        float padRight = 20;

        float padTop = 20;

        PixelPadding DataAreaPadding = new(padLeft, padRight, padBottom, padTop);
        return figureRect.Contract(DataAreaPadding);
    }

    public void Render(SKSurface surface)
    {
        LastRenderInfo = ScottPlot.Render.OnSurface(surface, this);
    }

    public byte[] GetImageBytes(int width, int height, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        SKImageInfo info = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        SKSurface surface = SKSurface.Create(info);
        SKImage snap = surface.Snapshot();
        SKData data = snap.Encode(format, quality);
        byte[] bytes = data.ToArray();
        return bytes;
    }

    public void SaveImage(int width, int height, string path, int quality = 100)
    {
        SKEncodedImageFormat format;

        if (path.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            format = SKEncodedImageFormat.Png;
        else if (path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
            format = SKEncodedImageFormat.Jpeg;
        else if (path.EndsWith(".jepg", StringComparison.OrdinalIgnoreCase))
            format = SKEncodedImageFormat.Jpeg;
        else if (path.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
            format = SKEncodedImageFormat.Bmp;
        else
            throw new ArgumentException("unsupported image format");

        byte[] bytes = GetImageBytes(width, height, format, quality);
        File.WriteAllBytes(path, bytes);
    }

    #region AddPlottable Helper Methods

    public Plottables.Scatter AddScatter(double[] xs, double[] ys, Color color)
    {
        Plottables.Scatter scatter = new(xs, ys) { Color = color };

        Add(scatter);

        return scatter;
    }

    #endregion
}
