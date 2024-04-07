using ScottPlot.AxisPanels;
using ScottPlot.Control;
using ScottPlot.Grids;
using ScottPlot.Legends;
using ScottPlot.Primitives;
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

    public BackgroundStyle FigureBackground = new() { Color = Colors.White };
    public BackgroundStyle DataBackground = new() { Color = Colors.Transparent };

    public IZoomRectangle ZoomRectangle { get; set; } = new StandardZoomRectangle();
    public double ScaleFactor { get => ScaleFactorF; set => ScaleFactorF = (float)value; }
    internal float ScaleFactorF = 1.0f;

    public AxisManager Axes { get; }

    public PlotStyler Style { get; }
    public FontStyler Font { get; }

    public Legend Legend { get; set; }

    public DefaultGrid Grid => Axes.DefaultGrid;

    public IPlottable Benchmark { get; set; } = new Plottables.Benchmark();

    /// <summary>
    /// This object is locked by the Render() methods.
    /// Logic that manipulates the plot (UI inputs or editing data)
    /// can lock this object to prevent rendering artifacts.
    /// </summary>
    public object Sync { get; } = new();

    public Plot()
    {
        Axes = new(this);
        Add = new(this);
        Style = new(this);
        Font = new(this);
        RenderManager = new(this);
        Legend = new(this);
        Layout = new(this);
    }

    public void Dispose()
    {
        DataBackground?.Dispose();
        FigureBackground?.Dispose();
        PlottableList.Clear();
    }

    #region Pixel/Coordinate Conversion

    /// <summary>
    /// Return the location on the screen (pixel) for a location on the plot (coordinates) on the default axes.
    /// The figure size and layout referenced will be the one from the last render.
    /// </summary>
    public Pixel GetPixel(Coordinates coordinates) => GetPixel(coordinates, Axes.Bottom, Axes.Left);

    /// <summary>
    /// Return the location on the screen (pixel) for a location on the plot (coordinates) on the given axes.
    /// The figure size and layout referenced will be the one from the last render.
    /// </summary>
    public Pixel GetPixel(Coordinates coordinates, IXAxis xAxis, IYAxis yAxis)
    {
        float xPixel = xAxis.GetPixel(coordinates.X, RenderManager.LastRender.DataRect);
        float yPixel = yAxis.GetPixel(coordinates.Y, RenderManager.LastRender.DataRect);

        if (ScaleFactor != 1)
        {
            xPixel *= ScaleFactorF;
            yPixel *= ScaleFactorF;
        }

        return new Pixel(xPixel, yPixel);
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
    /// Return a coordinate rectangle centered at a pixel.  Uses measurements
    /// from the most recent render.
    /// <param name="x">Center point pixel's x</param>
    /// <param name="y">Center point pixel's y</param>
    /// <param name="radius">Radius in pixels</param>
    /// <returns>The coordinate rectangle</returns>
    /// </summary>
    public CoordinateRect GetCoordinateRect(float x, float y, float radius = 10, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        float leftPx = (x - radius);
        float rightPx = (x + radius);
        float topPx = (y - radius);
        float bottomPx = (y + radius);

        if (ScaleFactor != 1)
        {
            leftPx /= ScaleFactorF;
            rightPx /= ScaleFactorF;
            topPx /= ScaleFactorF;
            bottomPx /= ScaleFactorF;
        }

        PixelRect dataRect = RenderManager.LastRender.DataRect;
        double left = (xAxis ?? Axes.Bottom).GetCoordinate(leftPx, dataRect);
        double right = (xAxis ?? Axes.Bottom).GetCoordinate(rightPx, dataRect);
        double top = (yAxis ?? Axes.Left).GetCoordinate(topPx, dataRect);
        double bottom = (yAxis ?? Axes.Left).GetCoordinate(bottomPx, dataRect);
        return new CoordinateRect(left, right, bottom, top);
    }

    /// <summary>
    /// Return a coordinate rectangle centered at a pixel.  Uses measurements
    /// from the most recent render.
    /// <param name="pixel">Center point pixel</param>
    /// <param name="radius">Radius in pixels</param>
    /// <returns>The coordinate rectangle</returns>
    /// </summary>
    public CoordinateRect GetCoordinateRect(Pixel pixel, float radius = 10, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        return GetCoordinateRect(pixel.X, pixel.Y, radius, xAxis, yAxis);
    }

    /// <summary>
    /// Return a coordinate rectangle centered at a coordinate pair with the
    /// radius specified in pixels.  Uses measurements from the most recent
    /// render.
    /// <param name="coordinates">Center point in coordinate units</param>
    /// <param name="radius">Radius in pixels</param>
    /// <returns>The coordinate rectangle</returns>
    /// </summary>
    public CoordinateRect GetCoordinateRect(Coordinates coordinates, float radius = 10, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        if (ScaleFactor != 1)
        {
            radius /= ScaleFactorF;
        }

        PixelRect dataRect = RenderManager.LastRender.DataRect;
        double radiusX = (xAxis ?? Axes.Bottom).GetCoordinateDistance(radius, dataRect);
        double radiusY = (yAxis ?? Axes.Left).GetCoordinateDistance(radius, dataRect);
        return coordinates.ToRect(radiusX, radiusY);
    }

    /// <summary>
    /// Get the axis under a given pixel
    /// </summary>
    /// <param name="pixel">Point</param>
    /// <returns>The axis at <paramref name="pixel" /> (or null)</returns>
    public IAxis? GetAxis(Pixel pixel)
    {
        IPanel? panel = GetPanel(pixel, axesOnly: true);
        return panel is IAxis axis ? axis : null;
    }

    /// <summary>
    /// Get the panel under a given pixel
    /// </summary>
    /// <param name="pixel">Point</param>
    /// <returns>The panel at <paramref name="pixel" /> (or null)</returns>
    public IPanel? GetPanel(Pixel pixel, bool axesOnly)
    {
        PixelRect dataRect = RenderManager.LastRender.Layout.DataRect;

        // Reverse here so the "highest" axis is returned in the case some overlap.
        var panels = axesOnly
            ? Axes.GetPanels().Reverse().OfType<IAxis>()
            : Axes.GetPanels().Reverse();

        foreach (IPanel panel in panels)
        {
            float axisPanelSize = RenderManager.LastRender.Layout.PanelSizes[panel];
            float axisPanelOffset = RenderManager.LastRender.Layout.PanelOffsets[panel];
            PixelRect axisRect = panel.GetPanelRect(dataRect, axisPanelSize, axisPanelOffset);
            if (axisRect.Contains(pixel))
            {
                return panel;
            }
        }

        return null;
    }

    #endregion

    #region Rendering and Image Creation

    [Obsolete("Call GetImage() to create a new image, or Render() to render onto an existing canvas.", true)]
    public void Render(int width = 400, int height = 300) { }

    /// <summary>
    /// Render onto an existing canvas
    /// </summary>
    public void Render(SKCanvas canvas, int width, int height)
    {
        // TODO: obsolete this
        PixelRect rect = new(0, width, height, 0);
        Render(canvas, rect);
    }

    /// <summary>
    /// Render onto an existing canvas inside the given rectangle
    /// </summary>
    public void Render(SKCanvas canvas, PixelRect rect)
    {
        lock (Sync)
        {
            RenderManager.Render(canvas, rect);
        }
    }

    /// <summary>
    /// Render onto an existing canvas of a surface over the local clip bounds
    /// </summary>
    public void Render(SKSurface surface)
    {
        RenderManager.Render(surface.Canvas, surface.Canvas.LocalClipBounds.ToPixelRect());
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
        return new Image(surface);
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

    public SavedImageInfo SaveJpeg(string filePath, int width, int height, int quality = 85)
    {
        using Image image = GetImage(width, height);
        return image.SaveJpeg(filePath, quality).WithRenderDetails(RenderManager.LastRender);
    }

    public SavedImageInfo SavePng(string filePath, int width, int height)
    {
        using Image image = GetImage(width, height);
        return image.SavePng(filePath).WithRenderDetails(RenderManager.LastRender);
    }

    public SavedImageInfo SaveBmp(string filePath, int width, int height)
    {
        using Image image = GetImage(width, height);
        return image.SaveBmp(filePath).WithRenderDetails(RenderManager.LastRender);
    }

    public SavedImageInfo SaveWebp(string filePath, int width, int height, int quality = 85)
    {
        using Image image = GetImage(width, height);
        return image.SaveWebp(filePath, quality).WithRenderDetails(RenderManager.LastRender);
    }

    public SavedImageInfo SaveSvg(string filePath, int width, int height)
    {
        using FileStream fs = new(filePath, FileMode.Create);
        using SKCanvas canvas = SKSvgCanvas.Create(new SKRect(0, 0, width, height), fs);
        Render(canvas, width, height);
        return new SavedImageInfo(filePath, (int)fs.Length).WithRenderDetails(RenderManager.LastRender);
    }

    public SavedImageInfo Save(string filePath, int width, int height, ImageFormat format = ImageFormat.Png, int quality = 85)
    {
        if (format == ImageFormat.Svg)
        {
            return SaveSvg(filePath, width, height).WithRenderDetails(RenderManager.LastRender);
        }

        if (format.IsRasterFormat())
        {
            using Image image = GetImage(width, height);
            return image.Save(filePath, format, quality).WithRenderDetails(RenderManager.LastRender);
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
    public Image GetLegendImage() => Legend.GetImage();

    /// <summary>
    /// Returns the content of the legend as SVG (vector) image
    /// </summary>
    public string GetLegendSvgXml() => Legend.GetSvgXml();

    #endregion

    #region Helper Methods

    /// <summary>
    /// Return contents of <see cref="PlottableList"/>.
    /// </summary>
    public IEnumerable<IPlottable> GetPlottables()
    {
        return PlottableList;
    }

    /// <summary>
    /// Return all plottables in <see cref="PlottableList"/> of the given type.
    /// </summary>
    public IEnumerable<T> GetPlottables<T>() where T : IPlottable
    {
        return PlottableList.OfType<T>();
    }

    /// <summary>
    /// Remove the given plottable from the <see cref="PlottableList"/>.
    /// </summary>
    public void Remove(IPlottable plottable)
    {
        while (PlottableList.Contains(plottable))
        {
            PlottableList.Remove(plottable);
        }
    }

    /// <summary>
    /// Remove the given Panel from the <see cref="Axes"/>.
    /// </summary>
    public void Remove(IPanel panel)
    {
        Axes.Remove(panel);
    }

    /// <summary>
    /// Remove the given Axis from the <see cref="Axes"/>.
    /// </summary>
    public void Remove(IAxis axis)
    {
        Axes.Remove(axis);
    }

    /// <summary>
    /// Remove all items of a specific type from the <see cref="PlottableList"/>.
    /// </summary>
    public void Remove(Type plotType)
    {
        List<IPlottable> itemsToRemove = PlottableList.Where(x => x.GetType() == plotType).ToList();

        foreach (IPlottable item in itemsToRemove)
        {
            PlottableList.Remove(item);
        }
    }

    /// <summary>
    /// Remove a all instances of a specific type from the <see cref="PlottableList"/>.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IPlottable"/> to be removed</typeparam>
    public void Remove<T>() where T : IPlottable
    {
        PlottableList.RemoveAll(x => x is T);
    }

    /// <summary>
    /// Remove all instances of a specific type from the <see cref="PlottableList"/> 
    /// that meet the <paramref name="predicate"/> criteraia.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <typeparam name="T">Type of <see cref="IPlottable"/> to be removed</typeparam>
    public void Remove<T>(Func<T, bool> predicate) where T : IPlottable
    {
        List<T> toRemove = PlottableList.OfType<T>().Where(predicate).ToList();
        toRemove.ForEach(x => PlottableList.Remove(x));
    }

    /// <summary>
    /// Disable visibility for all grids
    /// </summary>
    public void HideGrid()
    {
        Axes.AllGrids.ForEach(x => x.IsVisible = false);
    }

    /// <summary>
    /// Enable visibility for all grids
    /// </summary>
    public void ShowGrid()
    {
        Axes.AllGrids.ForEach(x => x.IsVisible = true);
    }

    /// <summary>
    /// Helper method for setting visibility of the <see cref="Legend"/>
    /// </summary>
    public void ShowLegend()
    {
        Legend.IsVisible = true;
    }

    /// <summary>
    /// Helper method for setting visibility of the <see cref="Legend"/>
    /// and setting <see cref="Legend.Location"/> to the provided one.
    /// </summary>
    public void ShowLegend(Alignment location)
    {
        Legend.IsVisible = true;
        Legend.Location = location;
    }

    /// <summary>
    /// Helper method for displaying specific items in the legend
    /// </summary>
    public void ShowLegend(IEnumerable<LegendItem> items, Alignment location = Alignment.LowerRight)
    {
        ShowLegend(location);
        Legend.ManualItems.Clear();
        Legend.ManualItems.AddRange(items);
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

    [Obsolete("This method is deprecated. Access Plot.Grid instead.", true)]
    public static DefaultGrid GetDefaultGrid() => null!;

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
