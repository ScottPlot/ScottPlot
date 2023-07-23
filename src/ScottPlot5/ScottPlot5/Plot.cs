using ScottPlot.Axis;
using ScottPlot.Layouts;
using ScottPlot.Axis.StandardAxes;
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
    public List<IPlottable> Plottables { get; } = new();
    public AddPlottable Add { get; }
    public IPalette Palette { get => Add.Palette; set => Add.Palette = value; }
    public RenderManager RenderManager { get; }
    public ILayoutMaker Layout { get; set; } = new Layouts.StandardLayoutMaker();
    public AutoScaleMargins Margins { get; } = new();
    public Color FigureBackground { get; set; } = Colors.White;
    public Color DataBackground { get; set; } = Colors.White;
    public IZoomRectangle ZoomRectangle { get; set; }
    public float ScaleFactor = 1.0f;

    public AxisStyler Axes { get; }

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
        IXAxis xAxisPrimary = new BottomAxis();
        IYAxis yAxisPrimary = new LeftAxis();
        XAxes.Add(xAxisPrimary);
        YAxes.Add(yAxisPrimary);

        // add labeless secondary axes to get right side ticks and padding
        IXAxis xAxisSecondary = new TopAxis();
        IYAxis yAxisSecondary = new RightAxis();
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
        Axes = new(this);
        Style = new(this);
        RenderManager = new(this);
    }

    public void Dispose()
    {
        Plottables.Clear();
        Grids.Clear();
        Panels.Clear();
        YAxes.Clear();
        XAxes.Clear();
    }

    #region Axis Management

    internal AxisBase[] GetStandardAxes()
    {
        // TODO: throw if custom axes in use
        return new AxisBase[]
        {
            (AxisBase)XAxes[0],
            (AxisBase)XAxes[1],
            (AxisBase)YAxes[0],
            (AxisBase)YAxes[1],
        };
    }

    internal IAxis[] GetAllAxes() => XAxes.Select(x => (IAxis)x).Concat(YAxes).ToArray();

    internal IPanel[] GetAllPanels() => XAxes.Select(x => (IPanel)x)
        .Concat(YAxes)
        .Concat(Panels)
        .Concat(new[] { TitlePanel })
        .ToArray();

    //[Obsolete("WARNING: NOT ALL LIMITS ARE AFFECTED")]
    public void SetAxisLimits(double left, double right, double bottom, double top)
    {
        XAxis.Min = left;
        XAxis.Max = right;
        YAxis.Min = bottom;
        YAxis.Max = top;
    }

    //[Obsolete("WARNING: NOT ALL LIMITS ARE AFFECTED")]
    public void SetAxisLimits(double? left = null, double? right = null, double? bottom = null, double? top = null)
    {
        XAxis.Min = left ?? XAxis.Min;
        XAxis.Max = right ?? XAxis.Max;
        YAxis.Min = bottom ?? YAxis.Min;
        YAxis.Max = top ?? YAxis.Max;
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
        return new AxisLimits(XAxis.Min, XAxis.Max, YAxis.Min, YAxis.Max);
    }

    public MultiAxisLimits GetMultiAxisLimits()
    {
        MultiAxisLimits limits = new();
        XAxes.ForEach(xAxis => limits.RememberLimits(xAxis, xAxis.Min, xAxis.Max));
        YAxes.ForEach(yAxis => limits.RememberLimits(yAxis, yAxis.Min, yAxis.Max));
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

        // assign default axes to plottables without axes
        ReplaceNullAxesWithDefaults();

        // expand all axes by the limits of each plot
        foreach (IPlottable plottable in Plottables)
        {
            AutoScale(plottable.Axes.XAxis, plottable.Axes.YAxis, tight);
        }
    }

    /// <summary>
    /// Adds the default X and Y axes to all plottables with unset axes
    /// </summary>
    internal void ReplaceNullAxesWithDefaults()
    {
        foreach (var plottable in Plottables)
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
    public void AutoScale(IXAxis xAxis, IYAxis yAxis, bool tight = false)
    {
        // reset limits only for these axes
        xAxis.Range.Reset();
        yAxis.Range.Reset();

        // assign default axes to plottables without axes
        ReplaceNullAxesWithDefaults();

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
        XAxes.ForEach(xAxis => xAxis.Range.PanMouse(pixelDeltaX, RenderManager.LastRenderInfo.DataRect.Width));
        YAxes.ForEach(yAxis => yAxis.Range.PanMouse(pixelDeltaY, RenderManager.LastRenderInfo.DataRect.Height));
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
        XAxes.ForEach(xAxis => xAxis.Range.ZoomMouseDelta(pixelDeltaX, RenderManager.LastRenderInfo.DataRect.Width));
        YAxes.ForEach(yAxis => yAxis.Range.ZoomMouseDelta(pixelDeltaY, RenderManager.LastRenderInfo.DataRect.Height));
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
        XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(fracX, xAxis.GetCoordinate(pixel.X, RenderManager.LastRenderInfo.DataRect)));
        YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(fracY, yAxis.GetCoordinate(pixel.Y, RenderManager.LastRenderInfo.DataRect)));
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
        PixelRect dataRect = RenderManager.LastRenderInfo.DataRect;
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
        PixelRect dataRect = RenderManager.LastRenderInfo.DataRect;
        double x = XAxis.GetCoordinate(pixel.X, dataRect);
        double y = YAxis.GetCoordinate(pixel.Y, dataRect);
        return new Coordinates(x, y);
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

    /// <summary>
    /// Render onto an existing surface using the local clip to determine dimensions
    /// </summary>
    public void Render(SKSurface surface)
    {
        RenderManager.Render(surface);
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

        Render(surface);
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
    /// Clears the <see cref="Plottables"/> list
    /// </summary>
    public void Clear() => Plottables.Clear();

    /// <summary>
    /// Shortcut to set text of the <see cref="TitlePanel"/> Label.
    /// Assign properties of <see cref="TitlePanel"/> Label to customize size, color, font, etc.
    /// </summary>
    public void Title(string text)
    {
        TitlePanel.Label.Text = text;
        TitlePanel.IsVisible = !string.IsNullOrWhiteSpace(text);
    }

    /// <summary>
    /// Shortcut to set text of the <see cref="BottomAxis"/> Label
    /// Assign properties of <see cref="BottomAxis"/> Label to customize size, color, font, etc.
    /// </summary>
    public void XLabel(string label)
    {
        BottomAxis.Label.Text = label;
    }

    /// <summary>
    /// Shortcut to set text of the <see cref="BottomAxis"/> Label
    /// Assign properties of <see cref="BottomAxis"/> Label to customize size, color, font, etc.
    /// </summary>
    public void YLabel(string label)
    {
        LeftAxis.Label.Text = label;
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
