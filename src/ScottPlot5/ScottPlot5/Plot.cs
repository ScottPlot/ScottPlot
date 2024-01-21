using ScottPlot.AxisPanels;
using ScottPlot.Control;
using ScottPlot.Legends;
using ScottPlot.Rendering;
using ScottPlot.Stylers;

namespace ScottPlot;

public class Plot : IDisposable
{
    public List<IPlottable> PlottableList { get; } = [];
    public PlottableAdder Add { get; }
    public RenderManager RenderManager { get; }
    public RenderDetails LastRender => RenderManager.LastRender;
    public LayoutManager Layout { get; private set; }
    public Color FigureBackground { get; set; } = Colors.White;
    public Color DataBackground { get; set; } = Colors.White;
    public IZoomRectangle ZoomRectangle { get; set; } = new StandardZoomRectangle();
    public float ScaleFactor { get; set; } = 1.0f;

    public AxisManager Axes { get; }

    public PlotStyler Style { get; }

    public Legend Legend { get; set; }

    public IPlottable Benchmark { get; set; } = new Plottables.Benchmark();

    public Plot()
    {
        Axes = new(this);
        Add = new(this);
        Style = new(this);
        RenderManager = new(this);
        Legend = new(this);
        Layout = new(this);
    }

    public void Dispose()
    {
        PlottableList.Clear();
        Axes.Clear();
    }

    #region Mouse Interaction

    /// <summary>
    /// Apply a click-drag pan operation to the plot
    /// </summary>
    public void MousePan(MultiAxisLimitManager originalLimits, Pixel mouseDown, Pixel mouseNow)
    {
        float pixelDeltaX = -(mouseNow.X - mouseDown.X);
        float pixelDeltaY = mouseNow.Y - mouseDown.Y;

        float scaledDeltaX = pixelDeltaX / ScaleFactor;
        float scaledDeltaY = pixelDeltaY / ScaleFactor;

        // restore mousedown limits
        originalLimits.Apply(this);

        // pan in the direction opposite of the mouse movement
        Axes.XAxes.ForEach(xAxis => xAxis.Range.PanMouse(scaledDeltaX, RenderManager.LastRender.DataRect.Width));
        Axes.YAxes.ForEach(yAxis => yAxis.Range.PanMouse(scaledDeltaY, RenderManager.LastRender.DataRect.Height));
    }

    /// <summary>
    /// Apply a click-drag zoom operation to the plot
    /// </summary>
    public void MouseZoom(MultiAxisLimitManager originalLimits, Pixel mouseDown, Pixel mouseNow)
    {
        float pixelDeltaX = mouseNow.X - mouseDown.X;
        float pixelDeltaY = -(mouseNow.Y - mouseDown.Y);

        // restore mousedown limits
        originalLimits.Apply(this);

        // apply zoom for each axis
        Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomMouseDelta(pixelDeltaX, RenderManager.LastRender.DataRect.Width));
        Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomMouseDelta(pixelDeltaY, RenderManager.LastRender.DataRect.Height));
    }

    /// <summary>
    /// Zoom into the coordinate corresponding to the given pixel.
    /// Fractional values >1 zoom in and <1 zoom out.
    /// </summary>
    public void MouseZoom(double fracX, double fracY, Pixel pixel)
    {
        Coordinates mouseCoordinate = GetCoordinates(pixel);

        MultiAxisLimitManager originalLimits = new(this);

        // restore mousedown limits
        originalLimits.Apply(this);

        // apply zoom for each axis
        Pixel scaledPixel = new(pixel.X / ScaleFactor, pixel.Y / ScaleFactor);
        Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(fracX, xAxis.GetCoordinate(scaledPixel.X, RenderManager.LastRender.DataRect)));
        Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(fracY, yAxis.GetCoordinate(scaledPixel.Y, RenderManager.LastRender.DataRect)));
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
        Pixel scaledMouseDown = new(mouseDown.X / ScaleFactor, mouseDown.Y / ScaleFactor);
        Pixel scaledMouseNow = new(mouseNow.X / ScaleFactor, mouseNow.Y / ScaleFactor);
        ZoomRectangle.Update(scaledMouseDown, scaledMouseNow);
        ZoomRectangle.VerticalSpan = vSpan;
        ZoomRectangle.HorizontalSpan = hSpan;
    }

    /// <summary>
    /// Return the pixel for a specific coordinate using measurements from the most recent render.
    /// </summary>
    public Pixel GetPixel(Coordinates coordinates)
    {
        Coordinates scaledCoordinates = new(coordinates.X * ScaleFactor, coordinates.Y * ScaleFactor);
        PixelRect dataRect = RenderManager.LastRender.DataRect;
        float x = Axes.Bottom.GetPixel(scaledCoordinates.X, dataRect);
        float y = Axes.Left.GetPixel(scaledCoordinates.Y, dataRect);
        return new Pixel(x, y);
    }

    /// <summary>
    /// Return the coordinate for a specific pixel using measurements from the most recent render.
    /// </summary>
    public Coordinates GetCoordinates(Pixel pixel, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        Pixel scaledPx = new(pixel.X / ScaleFactor, pixel.Y / ScaleFactor);
        PixelRect dataRect = RenderManager.LastRender.DataRect;
        double x = (xAxis ?? Axes.Bottom).GetCoordinate(scaledPx.X, dataRect);
        double y = (yAxis ?? Axes.Left).GetCoordinate(scaledPx.Y, dataRect);
        return new Coordinates(x, y);
    }

    /// <summary>
    /// Return the coordinate for a specific pixel using measurements from the most recent render.
    /// </summary>
    public Coordinates GetCoordinates(float x, float y, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        Pixel px = new(x, y);
        return GetCoordinates(px, xAxis, yAxis);
    }

    /// <summary>
    /// Return a coordinate rectangle centered at a pixel
    /// </summary>
    public CoordinateRect GetCoordinateRect(float x, float y, float radius = 10)
    {
        PixelRect dataRect = RenderManager.LastRender.DataRect;
        double left = Axes.Bottom.GetCoordinate(x - radius, dataRect);
        double right = Axes.Bottom.GetCoordinate(x + radius, dataRect);
        double top = Axes.Left.GetCoordinate(y - radius, dataRect);
        double bottom = Axes.Left.GetCoordinate(y + radius, dataRect);
        return new CoordinateRect(left, right, bottom, top);
    }

    /// <summary>
    /// Return a coordinate rectangle centered at a pixel
    /// </summary>
    public CoordinateRect GetCoordinateRect(Pixel pixel, float radius = 10)
    {
        return GetCoordinateRect(pixel.X, pixel.Y, radius);
    }

    /// <summary>
    /// Return a coordinate rectangle centered at a pixel
    /// </summary>
    public CoordinateRect GetCoordinateRect(Coordinates coordinates, float radius = 10)
    {
        PixelRect dataRect = RenderManager.LastRender.DataRect;
        double radiusX = Axes.Bottom.GetCoordinateDistance(radius, dataRect);
        double radiusY = Axes.Left.GetCoordinateDistance(radius, dataRect);
        return coordinates.ToRect(radiusX, radiusY);
    }

    #endregion

    #region Rendering and Image Creation

    /// <summary>
    /// Force the plot to render once by calling <see cref="GetImage(int, int)"/> but don't return what was rendered.
    /// </summary>
    public void Render(int width = 400, int height = 300)
    {
        if (width < 1)
            throw new ArgumentException($"{nameof(width)} must be greater than 0");

        if (height < 1)
            throw new ArgumentException($"{nameof(height)} must be greater than 0");

        GetImage(width, height);
    }

    /// <summary>
    /// Render onto an existing canvas
    /// </summary>
    public void Render(SKCanvas canvas, int width, int height)
    {
        // TODO: obsolete this
        PixelRect rect = new(0, width, height, 0);
        RenderManager.Render(canvas, rect);
    }

    /// <summary>
    /// Render onto an existing canvas inside the given rectangle
    /// </summary>
    public void Render(SKCanvas canvas, PixelRect rect)
    {
        RenderManager.Render(canvas, rect);
    }

    public Image GetImage(int width, int height)
    {
        if (width < 1)
            throw new ArgumentException($"{nameof(width)} must be greater than 0");

        if (height < 1)
            throw new ArgumentException($"{nameof(height)} must be greater than 0");

        SKImageInfo info = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        using SKSurface surface = SKSurface.Create(info);
        if (surface is null)
            throw new NullReferenceException($"invalid SKImageInfo");

        Render(surface.Canvas, width, height);
        return new(surface.Snapshot());
    }

    /// <summary>
    /// Render the plot and return an HTML img element containing a Base64-encoded PNG
    /// </summary>
    public string GetImageHtml(int width, int height)
    {
        Image img = GetImage(width, height);
        byte[] bytes = img.GetImageBytes();
        return ImageOperations.GetImageHtml(bytes);
    }

    public void SaveJpeg(string filePath, int width, int height, int quality = 85)
    {
        using Image image = GetImage(width, height);
        image.SaveJpeg(filePath, quality);
    }

    public void SavePng(string filePath, int width, int height)
    {
        using Image image = GetImage(width, height);
        image.SavePng(filePath);
    }

    public void SaveBmp(string filePath, int width, int height)
    {
        using Image image = GetImage(width, height);
        image.SaveBmp(filePath);
    }

    public void SaveWebp(string filePath, int width, int height, int quality = 85)
    {
        using Image image = GetImage(width, height);
        image.SaveWebp(filePath, quality);
    }

    public void SaveSvg(string filePath, int width, int height)
    {
        using FileStream fs = new(filePath, FileMode.Create);
        using SKCanvas canvas = SKSvgCanvas.Create(new SKRect(0, 0, width, height), fs);
        Render(canvas, width, height);
    }

    public void Save(string filePath, int width, int height, ImageFormat format = ImageFormat.Png, int quality = 85)
    {
        if (format == ImageFormat.Svg)
        {
            SaveSvg(filePath, width, height);
            return;
        }

        if (format.IsRasterFormat())
        {
            using Image image = GetImage(width, height);
            image.Save(filePath, format, quality);
            return;
        }

        throw new NotImplementedException(format.ToString());
    }

    public byte[] GetImageBytes(int width, int height, ImageFormat format = ImageFormat.Bmp)
    {
        using Image image = GetImage(width, height);
        byte[] bytes = image.GetImageBytes(format);
        return bytes;
    }

    /// <summary>
    /// Returns the content of the legend as a raster image
    /// </summary>
    public Image GetLegendImage() => Legend.GetImage(this);

    /// <summary>
    /// Returns the content of the legend as SVG (vector) image
    /// </summary>
    public string GetLegendSvgXml() => Legend.GetSvgXml(this);

    #endregion

    #region Helper Methods

    /// <summary>
    /// Remove a specific object from the plot.
    /// This removes the given object from <see cref="PlottableList"/>.
    /// </summary>
    public void Remove(IPlottable plottable)
    {
        PlottableList.Remove(plottable);
    }

    /// <summary>
    /// Disable visibility for all grids
    /// </summary>
    public void HideGrid()
    {
        Axes.Grids.ForEach(x => x.IsVisible = false);
    }

    /// <summary>
    /// Enable visibility for all grids
    /// </summary>
    public void ShowGrid()
    {
        Axes.Grids.ForEach(x => x.IsVisible = true);
    }

    /// <summary>
    /// Helper method for setting visibility of the <see cref="Legend"/>
    /// </summary>
    public void ShowLegend(Alignment location = Alignment.LowerRight)
    {
        Legend.IsVisible = true;
        Legend.Location = location;
    }

    /// <summary>
    /// Helper method for setting visibility of the <see cref="Legend"/>
    /// </summary>
    public void HideLegend()
    {
        Legend.IsVisible = false;
    }

    /// <summary>
    /// Clears the <see cref="PlottableList"/> list
    /// </summary>
    public void Clear() => PlottableList.Clear();

    /// <summary>
    /// Shortcut to set text of the <see cref="TitlePanel"/> Label.
    /// Assign properties of <see cref="TitlePanel"/> Label to customize size, color, font, etc.
    /// </summary>
    public void Title(string text, float? size = null)
    {
        Axes.Title.Label.Text = text;
        Axes.Title.IsVisible = !string.IsNullOrWhiteSpace(text);
        if (size.HasValue)
            Axes.Title.Label.FontSize = size.Value;
    }

    /// <summary>
    /// Shortcut to set text of the <see cref="BottomAxis"/> Label
    /// Assign properties of <see cref="BottomAxis"/> Label to customize size, color, font, etc.
    /// </summary>
    public void XLabel(string label, float? size = null)
    {
        Axes.Bottom.Label.Text = label;
        if (size.HasValue)
            Axes.Bottom.Label.FontSize = size.Value;
    }

    /// <summary>
    /// Shortcut to set text of the <see cref="BottomAxis"/> Label
    /// Assign properties of <see cref="BottomAxis"/> Label to customize size, color, font, etc.
    /// </summary>
    public void YLabel(string label, float? size = null)
    {
        Axes.Left.Label.Text = label;
        if (size.HasValue)
            Axes.Left.Label.FontSize = size.Value;
    }

    /// <summary>
    /// Return the first default grid in use.
    /// Throws an exception if no default grids exist.
    /// </summary>
    public Grids.DefaultGrid GetDefaultGrid()
    {
        IEnumerable<Grids.DefaultGrid> defaultGrids = Axes.Grids.OfType<Grids.DefaultGrid>();
        if (defaultGrids.Any())
            return defaultGrids.First();
        else
            throw new InvalidOperationException("The plot has no default grids");
    }

    #endregion

    #region Developer Tools

    public void Developer_ShowAxisDetails(bool enable = true)
    {
        foreach (IPanel panel in Axes.GetAxes())
        {
            panel.ShowDebugInformation = enable;
        }
    }

    #endregion
}
