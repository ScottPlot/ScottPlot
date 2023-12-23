using ScottPlot.Control;
using ScottPlot.Legends;
using ScottPlot.Rendering;
using ScottPlot.Stylers;

namespace ScottPlot;

public class Plot : IDisposable
{
    public List<IXAxis> XAxes { get; } = new(); // TODO: axes should be inside the Panels list
    public List<IYAxis> YAxes { get; } = new(); // TODO: axes should be inside the Panels list
    public List<IAxis> Axes => Enumerable.Concat<IAxis>(XAxes, YAxes).ToList();

    public IXAxis TopAxis => XAxes.First(x => x.Edge == Edge.Top);
    public IXAxis BottomAxis => XAxes.First(x => x.Edge == Edge.Bottom);
    public IYAxis LeftAxis => YAxes.First(x => x.Edge == Edge.Left);
    public IYAxis RightAxis => YAxes.First(x => x.Edge == Edge.Right);

    /// <summary>
    /// Panels are rectangular regions on the 4 edges outside the data area.
    /// Axes, colorbars, title, etc.
    /// </summary>
    public List<IPanel> Panels { get; } = new();

    /// <summary>
    /// This panel displays a label above the plot.
    /// </summary>
    public Panels.TitlePanel TitlePanel { get; } = new();

    public List<IGrid> Grids { get; } = new();
    public Legend Legend { get; set; }
    public List<IPlottable> PlottableList { get; } = new();
    public PlottableAdder Add { get; }
    public IPalette Palette { get => Add.Palette; set => Add.Palette = value; }
    public RenderManager RenderManager { get; }
    public RenderDetails LastRender => RenderManager.LastRender;
    public ILayoutEngine LayoutEngine { get; set; } = new LayoutEngines.Automatic();
    public IAutoScaler AutoScaler { get; set; } = new AutoScalers.FractionalAutoScaler(.1, .15);
    public Color FigureBackground { get; set; } = Colors.White;
    public Color DataBackground { get; set; } = Colors.White;
    public IZoomRectangle ZoomRectangle { get; set; }
    public float ScaleFactor { get; set; } = 1.0f;

    public AxisStyler AxisStyler { get; }

    public PlotStyler Style { get; }

    public IPlottable Benchmark { get; set; } = new Plottables.Benchmark();

    /// <summary>
    /// This property provides access to the primary horizontal axis below the plot.
    /// WARNING: Accessing this property will throw if the first bottom axis is not a standard axis.
    /// </summary>
    public IXAxis XAxis
    {
        get
        {
            var lowerAxes = XAxes.Where(x => x.Edge == Edge.Bottom);

            if (!lowerAxes.Any())
                throw new InvalidOperationException("Plot does not contain any bottom axes");

            return lowerAxes.First();
        }
    }


    /// <summary>
    /// This property provides access to the primary vertical axis to the left of the plot.
    /// WARNING: Accessing this property will throw if the first bottom axis is not a standard axis.
    /// </summary>
    public IYAxis YAxis
    {
        get
        {
            var leftAxes = YAxes.Where(x => x.Edge == Edge.Left);

            if (!leftAxes.Any())
                throw new InvalidOperationException("Plot does not contain any left axes");

            return leftAxes.First();
        }
    }

    public Plot()
    {
        // setup the default primary X and Y axes
        IXAxis xAxisPrimary = new AxisPanels.BottomAxis();
        IYAxis yAxisPrimary = new AxisPanels.LeftAxis();
        XAxes.Add(xAxisPrimary);
        YAxes.Add(yAxisPrimary);

        // add labeless secondary axes to get right side ticks and padding
        IXAxis xAxisSecondary = new AxisPanels.TopAxis();
        IYAxis yAxisSecondary = new AxisPanels.RightAxis();
        XAxes.Add(xAxisSecondary);
        YAxes.Add(yAxisSecondary);

        // setup the zoom rectangle
        ZoomRectangle = new StandardZoomRectangle();

        // add a default grid using the primary axes
        IGrid grid = new Grids.DefaultGrid(xAxisPrimary, yAxisPrimary);
        Grids.Add(grid);

        // setup classes which must be aware of the plot
        Add = new(this);
        AxisStyler = new(this);
        Style = new(this);
        RenderManager = new(this);
        Legend = new(this);
    }

    public void Dispose()
    {
        PlottableList.Clear();
        Grids.Clear();
        Panels.Clear();
        YAxes.Clear();
        XAxes.Clear();
    }

    #region Axis Management

    internal AxisPanels.AxisBase[] GetStandardAxes()
    {
        // TODO: throw if custom axes in use
        return new AxisPanels.AxisBase[]
        {
            (AxisPanels.AxisBase)XAxes[0],
            (AxisPanels.AxisBase)XAxes[1],
            (AxisPanels.AxisBase)YAxes[0],
            (AxisPanels.AxisBase)YAxes[1],
        };
    }

    internal IAxis[] GetAllAxes() => XAxes.Select(x => (IAxis)x).Concat(YAxes).ToArray();

    internal IPanel[] GetAllPanels() => XAxes.Select(x => (IPanel)x)
        .Concat(YAxes)
        .Concat(Panels)
        .Concat(new[] { TitlePanel })
        .ToArray();

    public void SetAxisLimitsX(double left, double right, IXAxis xAxis)
    {
        xAxis.Min = left;
        xAxis.Max = right;
    }

    public void SetAxisLimitsY(double bottom, double top, IYAxis yAxis)
    {
        yAxis.Min = bottom;
        yAxis.Max = top;
    }

    public void SetAxisLimitsX(double left, double right)
    {
        SetAxisLimitsX(left, right, BottomAxis);
    }

    public void SetAxisLimitsY(double bottom, double top)
    {
        SetAxisLimitsY(bottom, top, LeftAxis);
    }

    public void SetAxisLimits(double left, double right, double bottom, double top)
    {
        SetAxisLimitsX(left, right, BottomAxis);
        SetAxisLimitsY(bottom, top, LeftAxis);
    }

    public void SetAxisLimits(double left, double right, double bottom, double top, IXAxis xAxis, IYAxis yAxis)
    {
        SetAxisLimitsX(left, right, xAxis);
        SetAxisLimitsY(bottom, top, yAxis);
    }

    public void SetAxisLimits(double? left = null, double? right = null, double? bottom = null, double? top = null)
    {
        SetAxisLimitsX(left ?? XAxis.Min, right ?? XAxis.Max);
        SetAxisLimitsY(bottom ?? YAxis.Min, top ?? YAxis.Max);
    }

    public void SetAxisLimits(CoordinateRect rect)
    {
        SetAxisLimits(rect.Left, rect.Right, rect.Bottom, rect.Top);
    }

    public void SetAxisLimitsX(CoordinateRect limits)
    {
        SetAxisLimitsX(limits.Left, limits.Right, BottomAxis);
    }

    public void SetAxisLimitsY(CoordinateRect limits)
    {
        SetAxisLimitsY(limits.Bottom, limits.Top, LeftAxis);
    }

    public void SetAxisLimitsX(AxisLimits limits)
    {
        SetAxisLimitsX(limits.Left, limits.Right);
    }

    public void SetAxisLimitsX(AxisLimits limits, IXAxis xAxis)
    {
        SetAxisLimitsX(limits.Left, limits.Right, xAxis);
    }

    public void SetAxisLimitsY(AxisLimits limits)
    {
        SetAxisLimitsY(limits.Bottom, limits.Top);
    }

    public void SetAxisLimitsY(AxisLimits limits, IYAxis yAxis)
    {
        SetAxisLimitsY(limits.Bottom, limits.Top, yAxis);
    }

    public void SetAxisLimits(AxisLimits limits)
    {
        SetAxisLimits(limits, XAxis, YAxis);
    }

    public void SetAxisLimits(AxisLimits limits, IXAxis xAxis, IYAxis yAxis)
    {
        SetAxisLimitsX(limits.Left, limits.Right, xAxis);
        SetAxisLimitsY(limits.Bottom, limits.Top, yAxis);
    }

    public void SetAxisLimits(CoordinateRange xRange, CoordinateRange yRange)
    {
        AxisLimits limits = new(xRange.Min, xRange.Max, yRange.Min, yRange.Max);
        SetAxisLimits(limits);
    }

    /// <summary>
    /// Return the 2D axis limits for the default axes
    /// </summary>
    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(XAxis.Min, XAxis.Max, YAxis.Min, YAxis.Max);
    }

    /// <summary>
    /// Return the 2D axis limits for the given X/Y axis pair
    /// </summary>
    public AxisLimits GetAxisLimits(IXAxis xAxis, IYAxis yAxis)
    {
        return new AxisLimits(xAxis.Min, xAxis.Max, yAxis.Min, yAxis.Max);
    }

    /// <summary>
    /// Reset plot data margins to their default value.
    /// </summary>
    public void Margins()
    {
        AutoScaler = new AutoScalers.FractionalAutoScaler();
        AutoScale();
    }

    /// <summary>
    /// Define the amount of whitespace to place around the data area when calling <see cref="AutoScale()"/>.
    /// Values are a fraction from 0 (tightly fit the data) to 1 (lots of whitespace).
    /// </summary>
    public void Margins(double horizontal = 0.1, double vertical = .15)
    {
        AutoScaler = new AutoScalers.FractionalAutoScaler(horizontal, vertical);
        AutoScale();
    }

    /// <summary>
    /// Define the amount of whitespace to place around the data area when calling <see cref="AutoScale()"/>.
    /// Values are a fraction from 0 (tightly fit the data) to 1 (lots of whitespace).
    /// </summary>
    public void Margins(double left, double right, double bottom, double top)
    {
        AutoScaler = new AutoScalers.FractionalAutoScaler(left, right, bottom, top);
        AutoScale();
    }

    /// <summary>
    /// Adds the default X and Y axes to all plottables with unset axes
    /// </summary>
    internal void ReplaceNullAxesWithDefaults()
    {
        foreach (var plottable in PlottableList)
        {
            if (plottable.Axes.XAxis is null)
                plottable.Axes.XAxis = XAxis;

            if (plottable.Axes.YAxis is null)
                plottable.Axes.YAxis = YAxis;
        }
    }

    /// <summary>
    /// Automatically scale all axes to fit the data in all plottables
    /// </summary>
    public void AutoScale()
    {
        ReplaceNullAxesWithDefaults();
        AutoScaler.AutoScaleAll(PlottableList);
    }

    /// <summary>
    /// Autoscale the given axes to accommodate the data from all plottables that use them
    /// </summary>
    public void AutoScale(IXAxis xAxis, IYAxis yAxis, bool horizontal = true, bool vertical = true)
    {
        ReplaceNullAxesWithDefaults();

        AxisLimits limits = AutoScaler.GetAxisLimits(this, xAxis, yAxis);

        if (horizontal)
        {
            SetAxisLimitsX(limits.Left, limits.Right, xAxis);
        }

        if (vertical)
        {
            SetAxisLimitsY(limits.Bottom, limits.Top, yAxis);
        }
    }

    /// <summary>
    /// Adjust limits all axes to pan by the given distance in coordinate space
    /// </summary>
    public void Pan(CoordinateSize distance)
    {
        XAxes.ForEach(x => x.Range.Pan(distance.Width));
        YAxes.ForEach(x => x.Range.Pan(distance.Height));
    }

    /// <summary>
    /// Adjust limits all axes to pan by the given distance in pixel space
    /// </summary>
    public void Pan(PixelSize distance)
    {
        if (RenderManager.LastRender.Count == 0)
            throw new InvalidOperationException("at least one render is required before pixel panning is possible");

        XAxes.ForEach(ax => ax.Range.Pan(ax.GetCoordinateDistance(distance.Width, RenderManager.LastRender.DataRect)));
        YAxes.ForEach(ax => ax.Range.Pan(ax.GetCoordinateDistance(distance.Height, RenderManager.LastRender.DataRect)));
    }

    /// <summary>
    /// Modify limits of all axes to apply the given zoom.
    /// Fractional values >1 zoom in and <1 zoom out.
    /// </summary>
    public void Zoom(double fracX = 1.0, double fracY = 1.0)
    {
        XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(fracX));
        YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(fracY));
    }

    #endregion

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
        XAxes.ForEach(xAxis => xAxis.Range.PanMouse(scaledDeltaX, RenderManager.LastRender.DataRect.Width));
        YAxes.ForEach(yAxis => yAxis.Range.PanMouse(scaledDeltaY, RenderManager.LastRender.DataRect.Height));
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
        XAxes.ForEach(xAxis => xAxis.Range.ZoomMouseDelta(pixelDeltaX, RenderManager.LastRender.DataRect.Width));
        YAxes.ForEach(yAxis => yAxis.Range.ZoomMouseDelta(pixelDeltaY, RenderManager.LastRender.DataRect.Height));
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
        XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(fracX, xAxis.GetCoordinate(scaledPixel.X, RenderManager.LastRender.DataRect)));
        YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(fracY, yAxis.GetCoordinate(scaledPixel.Y, RenderManager.LastRender.DataRect)));
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
        float x = XAxis.GetPixel(scaledCoordinates.X, dataRect);
        float y = YAxis.GetPixel(scaledCoordinates.Y, dataRect);
        return new Pixel(x, y);
    }

    /// <summary>
    /// Return the coordinate for a specific pixel using measurements from the most recent render.
    /// </summary>
    public Coordinates GetCoordinates(Pixel pixel, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        Pixel scaledPx = new(pixel.X / ScaleFactor, pixel.Y / ScaleFactor);
        PixelRect dataRect = RenderManager.LastRender.DataRect;
        double x = (xAxis ?? XAxis).GetCoordinate(scaledPx.X, dataRect);
        double y = (yAxis ?? YAxis).GetCoordinate(scaledPx.Y, dataRect);
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
        double left = XAxis.GetCoordinate(x - radius, dataRect);
        double right = XAxis.GetCoordinate(x + radius, dataRect);
        double top = YAxis.GetCoordinate(y - radius, dataRect);
        double bottom = YAxis.GetCoordinate(y + radius, dataRect);
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
        double radiusX = XAxis.GetCoordinateDistance(radius, dataRect);
        double radiusY = YAxis.GetCoordinateDistance(radius, dataRect);
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
    public SvgImage GetLegendSvg() => Legend.GetSvgImage(this);

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

    public void DisableGrid()
    {
        Grids.ForEach(x => x.IsVisible = false);
    }

    public void EnableGrid()
    {
        Grids.ForEach(x => x.IsVisible = true);
    }

    /// <summary>
    /// Helper method for setting visibility of the <see cref="Legend"/>
    /// </summary>
    public void ShowLegend(Alignment location = Alignment.LowerRight)
    {
        Legend.IsVisible = true;
        Legend.Alignment = location;
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
        TitlePanel.Label.Text = text;
        TitlePanel.IsVisible = !string.IsNullOrWhiteSpace(text);
        if (size.HasValue)
            TitlePanel.Label.Font.Size = size.Value;
    }

    /// <summary>
    /// Shortcut to set text of the <see cref="BottomAxis"/> Label
    /// Assign properties of <see cref="BottomAxis"/> Label to customize size, color, font, etc.
    /// </summary>
    public void XLabel(string label, float? size = null)
    {
        BottomAxis.Label.Text = label;
        if (size.HasValue)
            BottomAxis.Label.Font.Size = size.Value;
    }

    /// <summary>
    /// Shortcut to set text of the <see cref="BottomAxis"/> Label
    /// Assign properties of <see cref="BottomAxis"/> Label to customize size, color, font, etc.
    /// </summary>
    public void YLabel(string label, float? size = null)
    {
        LeftAxis.Label.Text = label;
        if (size.HasValue)
            LeftAxis.Label.Font.Size = size.Value;
    }

    /// <summary>
    /// Return the first default grid in use.
    /// Throws an exception if no default grids exist.
    /// </summary>
    public Grids.DefaultGrid GetDefaultGrid()
    {
        IEnumerable<Grids.DefaultGrid> defaultGrids = Grids.OfType<Grids.DefaultGrid>();
        if (defaultGrids.Any())
            return defaultGrids.First();
        else
            throw new InvalidOperationException("The plot has no default grids");
    }

    /// <summary>
    /// Set axis limits of this plots to match those of a given plot
    /// </summary>
    public void MatchAxisLimits(Plot other, bool x = true, bool y = true)
    {
        AxisLimits theseLimits = GetAxisLimits();
        AxisLimits otherLimits = other.GetAxisLimits();
        CoordinateRange xRange = x ? otherLimits.XRange : theseLimits.XRange;
        CoordinateRange yRange = y ? otherLimits.YRange : theseLimits.YRange;
        SetAxisLimits(xRange, yRange);
    }

    /// <summary>
    /// Apply a fixed layout using the given rectangle to define the data area
    /// </summary>
    public void FixedLayout(PixelRect dataRect)
    {
        LayoutEngine = new LayoutEngines.FixedDataArea(dataRect);
    }

    /// <summary>
    /// Apply a fixed layout using the given padding to define space between the
    /// data area and the edge of the figure
    /// </summary>
    public void FixedLayout(PixelPadding padding)
    {
        LayoutEngine = new LayoutEngines.FixedPadding(padding);
    }

    /// <summary>
    /// Use the automatic layout system (default)
    /// </summary>
    public void AutomaticLayout()
    {
        LayoutEngine = new LayoutEngines.Automatic();
    }

    #endregion

    #region Developer Tools

    public void Developer_ShowAxisDetails(bool enable = true)
    {
        foreach (IPanel panel in GetAllAxes())
        {
            panel.ShowDebugInformation = enable;
        }
    }

    #endregion
}
