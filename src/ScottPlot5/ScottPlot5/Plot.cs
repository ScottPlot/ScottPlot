using ScottPlot.Legends;
using ScottPlot.Control;
using ScottPlot.Stylers;
using ScottPlot.Rendering;

namespace ScottPlot;

public class Plot : IDisposable
{
    public List<IXAxis> XAxes { get; } = new();
    public List<IYAxis> YAxes { get; } = new();
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
    public List<ILegend> Legends { get; } = new();
    public List<IPlottable> PlottableList { get; } = new();
    public PlottableAdder Add { get; }
    public IPalette Palette { get => Add.Palette; set => Add.Palette = value; }
    public RenderManager RenderManager { get; }
    public ILayoutEngine LayoutEngine { get; set; } = new LayoutEngines.Automatic();
    public AutoScaleMargins AutoScaleMargins { get; private set; } = new(.1, .15);
    public Color FigureBackground { get; set; } = Colors.White;
    public Color DataBackground { get; set; } = Colors.White;
    public IZoomRectangle ZoomRectangle { get; set; }
    public float ScaleFactor { get; set; } = 1.0f;

    public AxisStyler AxisStyler { get; }

    public PlotStyler Style { get; }
    public bool ShowBenchmark { get; set; } = false;

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

        // add a standard legend
        ILegend legend = new StandardLegend();
        Legends.Add(legend);

        // setup classes which must be aware of the plot
        Add = new(this);
        AxisStyler = new(this);
        Style = new(this);
        RenderManager = new(this);
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

    public void SetAxisLimits(double left, double right, double bottom, double top)
    {
        XAxis.Min = left;
        XAxis.Max = right;
        YAxis.Min = bottom;
        YAxis.Max = top;
    }

    public void SetAxisLimits(double? left = null, double? right = null, double? bottom = null, double? top = null)
    {
        XAxis.Min = left ?? XAxis.Min;
        XAxis.Max = right ?? XAxis.Max;
        YAxis.Min = bottom ?? YAxis.Min;
        YAxis.Max = top ?? YAxis.Max;
    }

    public void SetAxisLimits(CoordinateRect rect)
    {
        SetAxisLimits(rect.Left, rect.Right, rect.Bottom, rect.Top);
    }

    public void SetAxisLimits(AxisLimits rect)
    {
        SetAxisLimits(rect.Rect);
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
    /// Automatically scale the axis limits to fit the data.
    /// Note: This used to be AxisAuto().
    /// Note: Margin size can be customized by editing properties of <see cref="AutoScaleMargins"/>
    /// </summary>
    public void AutoScale()
    {
        // reset limits for all axes
        XAxes.ForEach(xAxis => xAxis.Range.Reset());
        YAxes.ForEach(yAxis => yAxis.Range.Reset());

        // assign default axes to plottables without axes
        ReplaceNullAxesWithDefaults();

        // expand all axes by the limits of each plot
        foreach (IPlottable plottable in PlottableList)
        {
            AutoScale(plottable.Axes.XAxis, plottable.Axes.YAxis);
        }
    }

    /// <summary>
    /// Define the amount of whitespace to include around the data when calling <see cref="AutoScale()"/>.
    /// Values are a fraction from 0 (tightly fit the data) to 1 (lots of whitespace).
    /// </summary>
    public void Margins(double horizontal = 0.1, double vertical = .15, bool apply = true)
    {
        AutoScaleMargins = new(horizontal, vertical);

        if (apply)
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
    /// Automatically scale the given axes to fit the data in plottables which use them
    /// </summary>
    public void AutoScale(IXAxis xAxis, IYAxis yAxis)
    {
        // reset limits only for these axes
        xAxis.Range.Reset();
        yAxis.Range.Reset();

        // assign default axes to plottables without axes
        ReplaceNullAxesWithDefaults();

        // expand all axes by the limits of each plot
        foreach (IPlottable plottable in PlottableList)
        {
            AxisLimits limits = plottable.GetAxisLimits();
            plottable.Axes.YAxis.Range.Expand(limits.Rect.YRange);
            plottable.Axes.XAxis.Range.Expand(limits.Rect.XRange);
        }

        // apply margins
        XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(1 - AutoScaleMargins.Horizontal));
        YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(1 - AutoScaleMargins.Vertical));
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
        if (RenderManager.RenderCount == 0)
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
        RenderManager.Render(canvas, width, height);
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

    #endregion

    #region Helper Methods

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
    /// Return the first default legend in use.
    /// Throws an exception if no default legends exist.
    /// </summary>
    public Legends.StandardLegend GetLegend()
    {
        IEnumerable<Legends.StandardLegend> standardLegends = Legends.OfType<Legends.StandardLegend>();
        if (standardLegends.Any())
            return standardLegends.First();
        else
            throw new InvalidOperationException("The plot has no standard legends");
    }

    /// <summary>
    /// Set visibility of all legends.
    /// </summary>
    public void Legend(bool enable = true)
    {
        Legends.ForEach(x => x.IsVisible = enable);
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
