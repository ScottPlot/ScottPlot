using System.Diagnostics;
using ScottPlot.Axis;
using ScottPlot.Rendering;
using ScottPlot.LayoutSystem;
using SkiaSharp;

namespace ScottPlot;

public class Plot
{
    public readonly List<IXAxis> XAxes = new();
    public readonly List<IYAxis> YAxes = new();
    public readonly List<IGrid> Grids = new();
    public readonly PlottableList Plottables = new();

    public IRenderer Renderer = new StandardRenderer();

    public ILayoutSystem Layout = new StandardLayoutSystem();

    public readonly AutoScaleMargins Margins = new();


    // TODO: allow the user to inject their own visual debugging and performance monitoring tools
    public readonly Plottables.DebugBenchmark Benchmark = new();

    // TODO: allow the user to inject their own visual debugging and performance monitoring tools
    public readonly Plottables.ZoomRectangle ZoomRectangle;

    /// <summary>
    /// Any state stored across renders can be stored here.
    /// </summary>
    internal RenderDetails LastRenderInfo = new();

    /// <summary>
    /// The primary horizontal axis (the first one in the list of <see cref="XAxes"/>)
    /// </summary>
    public IXAxis XAxis => XAxes.First();

    /// <summary>
    /// The primary vertical axis (the first one in the list of <see cref="YAxes"/>)
    /// </summary>
    public IYAxis YAxis => YAxes.First();

    public Plot()
    {
        var xAxis = new Axis.StandardAxes.BottomAxis();
        var yAxis = new Axis.StandardAxes.LeftAxis();
        XAxes.Add(xAxis);
        YAxes.Add(yAxis);

        ZoomRectangle = new(xAxis, yAxis);

        var grid = new Grids.DefaultGrid(xAxis, yAxis);
        Grids.Add(grid);
    }

    #region Axis Management

    public void SetAxisLimits(double left, double right, double bottom, double top)
    {
        // TODO: move set limits inside XAxis and YAxis
        XAxis.Left = left;
        XAxis.Right = right;

        YAxis.Bottom = bottom;
        YAxis.Top = top;
    }

    public void SetAxisLimits(CoordinateRect rect)
    {
        SetAxisLimits(rect.XMin, rect.XMax, rect.YMin, rect.YMax);
    }

    public void SetAxisLimits(AxisLimits rect)
    {
        SetAxisLimits(rect.Rect);
    }

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(XAxis.Left, XAxis.Right, YAxis.Bottom, YAxis.Top);
    }

    /// <summary>
    /// Automatically scale the axis limits to fit the data.
    /// Note: This used to be AxisAuto().
    /// Note: Margin size can be customized by editing properties of <see cref="Margins"/>
    /// </summary>
    public void AutoScale(bool tight = false)
    {
        // reset limits for all axes
        XAxes.ForEach(xAxis => xAxis.Range.Reset());
        YAxes.ForEach(yAxis => yAxis.Range.Reset());

        // expand all axes by the limits of each plot
        foreach (IPlottable plottable in Plottables)
        {
            AxisLimits limits = plottable.GetAxisLimits();
            plottable.Axes.YAxis.Range.Expand(limits.Rect.YRange);
            plottable.Axes.XAxis.Range.Expand(limits.Rect.XRange);
        }

        // apply margins
        if (!tight)
        {
            XAxes.ForEach(xAxis => xAxis.Range.Zoom(Margins.ZoomFracX));
            YAxes.ForEach(yAxis => yAxis.Range.Zoom(Margins.ZoomFracY));
        }
    }

    #endregion

    #region Mouse Interaction

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

        CoordinateRect newLimits = originalLimits.WithZoom(fracX, fracY);
        SetAxisLimits(newLimits);
    }

    /// <summary>
    /// Zoom into the coordinate corresponding to the given pixel.
    /// Fractional values >1 zoom in and <1 zoom out.
    /// </summary>
    public void MouseZoom(double fracX, double fracY, Pixel pixel)
    {
        Coordinates mouseCoordinate = GetCoordinate(pixel);
        SetAxisLimits(GetAxisLimits().WithZoom(fracX, fracY, mouseCoordinate.X, mouseCoordinate.Y));
    }

    /// <summary>
    /// Update the shape of the zoom rectangle
    /// </summary>
    /// <param name="mouseDown">Location of the mouse at the start of the drag</param>
    /// <param name="mouseNow">Location of the mouse now (after dragging)</param>
    /// <param name="vSpan">If true, shade the full region between two X positions</param>
    /// <param name="hSpan">If true, shade the full region between two Y positions</param>
    public void MouseZoomRectangle(Pixel mouseDown, Pixel mouseNow, bool vSpan, bool hSpan)
    {
        Coordinates c1 = GetCoordinate(mouseDown);
        Coordinates c2 = GetCoordinate(mouseNow);
        ZoomRectangle.SetSize(c1, c2);
        ZoomRectangle.VerticalSpan = vSpan;
        ZoomRectangle.HorizontalSpan = hSpan;
    }

    /// <summary>
    /// Indicate the click-drag zoom rectangle was dropped.
    /// Applying the zoom will set axis limits to where the rectangle was before it was dropped.
    /// </summary>
    public void MouseZoomRectangleClear(bool applyZoom) // TODO: instead of a bool, pass-in the rectangle itself
    {
        if (applyZoom)
        {
            if (ZoomRectangle.HorizontalSpan || ZoomRectangle.VerticalSpan)
            {
                double left = ZoomRectangle.VerticalSpan ? ZoomRectangle.Rect.XMin : XAxis.Left;
                double right = ZoomRectangle.VerticalSpan ? ZoomRectangle.Rect.XMax : XAxis.Right;
                double bottom = ZoomRectangle.HorizontalSpan ? ZoomRectangle.Rect.YMin : YAxis.Bottom;
                double top = ZoomRectangle.HorizontalSpan ? ZoomRectangle.Rect.YMax : YAxis.Top;
                SetAxisLimits(left, right, bottom, top);
            }
            else
            {
                SetAxisLimits(ZoomRectangle.Rect);
            }
        }

        ZoomRectangle.Clear();
    }

    /// <summary>
    /// Return the pixel for a specific coordinate using measurements from the most recent render.
    /// </summary>
    public Pixel GetPixel(Coordinates coord)
    {
        PixelRect dataRect = LastRenderInfo.DataRect;
        float x = XAxis.GetPixel(coord.X, dataRect);
        float y = YAxis.GetPixel(coord.Y, dataRect);
        return new Pixel(x, y);
    }

    /// <summary>
    /// Return the coordinate for a specific pixel using measurements from the most recent render.
    /// </summary>
    public Coordinates GetCoordinate(Pixel pixel, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        // TODO: multi-axis support
        PixelRect dataRect = LastRenderInfo.DataRect;
        double x = XAxis.GetCoordinate(pixel.X, dataRect);
        double y = YAxis.GetCoordinate(pixel.Y, dataRect);
        return new Coordinates(x, y);
    }

    #endregion

    #region Rendering and Image Creation

    public void Render(SKSurface surface)
    {
        LastRenderInfo = Renderer.Render(surface, this);
    }

    public byte[] GetImageBytes(int width, int height, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        SKImageInfo info = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        SKSurface surface = SKSurface.Create(info);
        Render(surface);
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

    #endregion
}
