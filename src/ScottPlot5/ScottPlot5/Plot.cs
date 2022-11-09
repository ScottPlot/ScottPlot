using System.Diagnostics;
using ScottPlot.Axis;
using ScottPlot.Rendering;
using ScottPlot.LayoutSystem;
using SkiaSharp;
using ScottPlot.Plottables;

namespace ScottPlot;

public class Plot
{
    public readonly List<IXAxis> XAxes = new();
    public readonly List<IYAxis> YAxes = new();
    public readonly List<IGrid> Grids = new();
    public readonly List<IPlottable> Plottables = new();

    public readonly PlottableFactory Add;
    public IPalette Palette { get => Add.Palette; set => Add.Palette = value; }

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

        Add = new(this);
    }

    #region Axis Management

    //[Obsolete("WARNING: NOT ALL LIMITS ARE AFFECTED")]
    public void SetAxisLimits(double left, double right, double bottom, double top)
    {
        // TODO: move set limits inside XAxis and YAxis
        XAxis.Left = left;
        XAxis.Right = right;

        YAxis.Bottom = bottom;
        YAxis.Top = top;
    }

    //[Obsolete("WARNING: NOT ALL LIMITS ARE AFFECTED")]
    public void SetAxisLimits(CoordinateRect rect)
    {
        SetAxisLimits(rect.XMin, rect.XMax, rect.YMin, rect.YMax);
    }

    //[Obsolete("WARNING: NOT ALL LIMITS ARE AFFECTED")]
    public void SetAxisLimits(AxisLimits rect)
    {
        SetAxisLimits(rect.Rect);
    }

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(XAxis.Left, XAxis.Right, YAxis.Bottom, YAxis.Top);
    }

    public MultiAxisLimits GetMultiAxisLimits()
    {
        MultiAxisLimits limits = new();
        XAxes.ForEach(xAxis => limits.RememberLimits(xAxis, xAxis.Left, xAxis.Right));
        YAxes.ForEach(yAxis => limits.RememberLimits(yAxis, yAxis.Bottom, yAxis.Top));
        return limits;
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
            XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(Margins.ZoomFracX));
            YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(Margins.ZoomFracY));
        }
    }

    #endregion

    #region Mouse Interaction

    /// <summary>
    /// Apply a click-drag pan operation to the plot
    /// </summary>
    public void MousePan(MultiAxisLimits originalLimits, Pixel mouseDown, Pixel mouseNow)
    {
        float pixelDeltaX = -(mouseNow.X - mouseDown.X);
        float pixelDeltaY = mouseNow.Y - mouseDown.Y;

        // restore mousedown limits
        XAxes.ForEach(xAxis => originalLimits.RestoreLimits(xAxis));
        YAxes.ForEach(yAxis => originalLimits.RestoreLimits(yAxis));

        // pan in the direction opposite of the mouse movement
        XAxes.ForEach(xAxis => xAxis.Range.PanMouse(pixelDeltaX, LastRenderInfo.DataRect.Width));
        YAxes.ForEach(yAxis => yAxis.Range.PanMouse(pixelDeltaY, LastRenderInfo.DataRect.Height));
    }

    /// <summary>
    /// Apply a click-drag zoom operation to the plot
    /// </summary>
    public void MouseZoom(MultiAxisLimits originalLimits, Pixel mouseDown, Pixel mouseNow)
    {
        float pixelDeltaX = mouseNow.X - mouseDown.X;
        float pixelDeltaY = -(mouseNow.Y - mouseDown.Y);

        // restore mousedown limits
        XAxes.ForEach(xAxis => originalLimits.RestoreLimits(xAxis));
        YAxes.ForEach(yAxis => originalLimits.RestoreLimits(yAxis));

        // apply zoom for each axis
        XAxes.ForEach(xAxis => xAxis.Range.ZoomMouseDelta(pixelDeltaX, LastRenderInfo.DataRect.Width));
        YAxes.ForEach(yAxis => yAxis.Range.ZoomMouseDelta(pixelDeltaY, LastRenderInfo.DataRect.Height));
    }

    /// <summary>
    /// Zoom into the coordinate corresponding to the given pixel.
    /// Fractional values >1 zoom in and <1 zoom out.
    /// </summary>
    public void MouseZoom(double fracX, double fracY, Pixel pixel)
    {
        Coordinates mouseCoordinate = GetCoordinate(pixel);
        MultiAxisLimits originalLimits = GetMultiAxisLimits();

        // restore mousedown limits
        XAxes.ForEach(xAxis => originalLimits.RestoreLimits(xAxis));
        YAxes.ForEach(yAxis => originalLimits.RestoreLimits(yAxis));

        // apply zoom for each axis
        XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(fracX, xAxis.GetCoordinate(pixel.X, LastRenderInfo.DataRect)));
        YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(fracY, yAxis.GetCoordinate(pixel.Y, LastRenderInfo.DataRect)));
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
        ZoomRectangle.Update(mouseDown, mouseNow);
        ZoomRectangle.VerticalSpan = vSpan;
        ZoomRectangle.HorizontalSpan = hSpan;
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

    public byte[] GetImageBytes(int width, int height, ImageFormat format = ImageFormat.Png, int quality = 100)
    {
        if (width < 1)
            throw new ArgumentException($"{nameof(width)} must be greater than 0");

        if (height < 1)
            throw new ArgumentException($"{nameof(height)} must be greater than 0");

        SKImageInfo info = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        SKSurface surface = SKSurface.Create(info);
        if (surface is null)
            throw new NullReferenceException($"invalid SKImageInfo");
        Render(surface);
        SKImage snap = surface.Snapshot();
        SKEncodedImageFormat skFormat = format.ToSKFormat();
        SKData data = snap.Encode(skFormat, quality);
        byte[] bytes = data.ToArray();
        return bytes;
    }

    public string SaveJpeg(string path, int width, int height, int quality = 85)
    {
        byte[] bytes = GetImageBytes(width, height, ImageFormat.Jpeg, quality);
        File.WriteAllBytes(path, bytes);
        return Path.GetFullPath(path);
    }

    public string SavePng(string path, int width, int height, int quality = 85)
    {
        byte[] bytes = GetImageBytes(width, height, ImageFormat.Png, quality);
        File.WriteAllBytes(path, bytes);
        return Path.GetFullPath(path);
    }

    public string SaveBmp(string path, int width, int height, int quality = 85)
    {
        byte[] bytes = GetImageBytes(width, height, ImageFormat.Bmp, quality);
        File.WriteAllBytes(path, bytes);
        return Path.GetFullPath(path);
    }

    public string SaveWebp(string path, int width, int height, int quality = 85)
    {
        byte[] bytes = GetImageBytes(width, height, ImageFormat.Webp, quality);
        File.WriteAllBytes(path, bytes);
        return Path.GetFullPath(path);
    }

    #endregion

    #region Legend
    public IEnumerable<LegendItem> LegendItems()
    {
        foreach (var curr in Plottables)
            foreach (var item in curr.LegendItems)
                yield return item;
    }

    #endregion
}
