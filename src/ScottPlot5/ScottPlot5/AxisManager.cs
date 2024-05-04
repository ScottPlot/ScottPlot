using ScottPlot.AxisPanels;

namespace ScottPlot;

public class AxisManager
{
    private readonly Plot Plot;

    /// <summary>
    /// Logic that determines padding around the data area when <see cref="AutoScale()"/> is called
    /// </summary>
    public IAutoScaler AutoScaler { get; set; } = new AutoScalers.FractionalAutoScaler(.1, .15);

    /// <summary>
    /// Horizontal axes
    /// </summary>
    internal List<IXAxis> XAxes { get; } = new();

    /// <summary>
    /// Vertical axes
    /// </summary>
    internal List<IYAxis> YAxes { get; } = new();

    /// <summary>
    /// Panels take up spce on one side of the data area (like a colorbar)
    /// </summary>
    internal List<IPanel> Panels { get; } = new();

    /// <summary>
    /// A special panel
    /// </summary>
    public Panels.TitlePanel Title { get; } = new();

    /// <summary>
    /// All axes
    /// </summary>
    public IEnumerable<IAxis> GetAxes() => XAxes.Cast<IAxis>().Concat(YAxes);

    /// <summary>
    /// All axes with the given edge
    /// </summary>
    public IEnumerable<IAxis> GetAxes(Edge edge) => GetAxes().Where(x => x.Edge == edge).ToArray();

    /// <summary>
    /// Returns all axes, panels, and the title
    /// </summary>
    /// <returns></returns>
    internal IPanel[] GetPanels() => GetAxes().Concat(Panels).Concat(new[] { Title }).ToArray();

    /// <summary>
    /// The primary horizontal axis above the plot
    /// </summary>
    public IXAxis Top => XAxes.First(x => x.Edge == Edge.Top);

    /// <summary>
    /// The primary horizontal axis below the plot
    /// </summary>
    public IXAxis Bottom => XAxes.First(x => x.Edge == Edge.Bottom);

    /// <summary>
    /// The primary vertical axis to the left of the plot
    /// </summary>
    public IYAxis Left => YAxes.First(x => x.Edge == Edge.Left);

    /// <summary>
    /// The primary vertical axis to the right of the plot
    /// </summary>
    public IYAxis Right => YAxes.First(x => x.Edge == Edge.Right);

    /// <summary>
    /// All grids
    /// </summary>
    public List<IGrid> Grids { get; } = new();

    /// <summary>
    /// Rules that are applied before each render
    /// </summary>
    public List<IAxisRule> Rules { get; } = new();

    /// <summary>
    /// Contains state and logic for axes
    /// </summary>
    public AxisManager(Plot plot)
    {
        Plot = plot;

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

        // add a default grid using the primary axes
        IGrid grid = new Grids.DefaultGrid(xAxisPrimary, yAxisPrimary);
        Grids.Add(grid);
    }

    public void Clear()
    {
        Grids.Clear();
        Panels.Clear();
        YAxes.Clear();
        XAxes.Clear();
    }

    /// <summary>
    /// Remove all axes that lie on the given edge.
    /// </summary>
    public void Remove(Edge edge)
    {
        foreach (IAxis axis in GetAxes(edge).ToArray())
        {
            if (axis is IXAxis xAxis)
                Plot.Axes.XAxes.Remove(xAxis);

            if (axis is IYAxis yAxis)
                Plot.Axes.YAxes.Remove(yAxis);
        }
    }

    /// <summary>
    /// Remove the given axis from the plot
    /// </summary>
    public void Remove(IAxis axis)
    {
        XAxes.RemoveAll(ax => ax == axis);
        YAxes.RemoveAll(ax => ax == axis);
    }

    [Obsolete("This method is deprecated. Use DateTimeTicksBottom().")]
    public void DateTimeTicks(Edge edge)
    {
        Remove(edge);

        IXAxis dateAxis = edge switch
        {
            Edge.Left => throw new NotImplementedException(), // TODO: support vertical DateTime axes
            Edge.Right => throw new NotImplementedException(),
            Edge.Bottom => new DateTimeXAxis(),
            Edge.Top => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };

        Plot.Axes.XAxes.Add(dateAxis);
        Plot.Axes.Grids.ForEach(x => x.Replace(dateAxis));
    }

    /// <summary>
    /// Remove all bottom axes, create a DateTime bottom axis, add it to the plot, and return it.
    /// </summary>
    public DateTimeXAxis DateTimeTicksBottom()
    {
        Plot.Axes.Remove(Edge.Bottom);
        DateTimeXAxis dateAxis = new();
        Plot.Axes.XAxes.Add(dateAxis);
        Plot.Axes.Grids.ForEach(x => x.Replace(dateAxis));
        return dateAxis;
    }

    /// <summary>
    /// Crete a new axis, add it to the plot, and return it
    /// </summary>
    public LeftAxis AddLeftAxis()
    {
        LeftAxis axis = new();
        YAxes.Add(axis);
        return axis;
    }

    /// <summary>
    /// Crete a new axis, add it to the plot, and return it
    /// </summary>
    public RightAxis AddRightAxis()
    {
        RightAxis axis = new();
        YAxes.Add(axis);
        return axis;
    }

    /// <summary>
    /// Crete a new axis, add it to the plot, and return it
    /// </summary>
    public BottomAxis AddBottomAxis()
    {
        BottomAxis axis = new();
        XAxes.Add(axis);
        return axis;
    }

    /// <summary>
    /// Crete a new axis, add it to the plot, and return it
    /// </summary>
    public TopAxis AddTopAxis()
    {
        TopAxis axis = new();
        XAxes.Add(axis);
        return axis;
    }


    public void SetLimitsX(double left, double right, IXAxis xAxis)
    {
        xAxis.Min = left;
        xAxis.Max = right;
    }

    public void SetLimitsY(double bottom, double top, IYAxis yAxis)
    {
        yAxis.Min = bottom;
        yAxis.Max = top;
    }

    public void SetLimitsX(double left, double right)
    {
        SetLimitsX(left, right, Bottom);
    }

    public void SetLimitsY(double bottom, double top)
    {
        SetLimitsY(bottom, top, Left);
    }

    public void SetLimits(double left, double right, double bottom, double top)
    {
        SetLimitsX(left, right, Bottom);
        SetLimitsY(bottom, top, Left);
    }

    public void SetLimits(double left, double right, double bottom, double top, IXAxis xAxis, IYAxis yAxis)
    {
        SetLimitsX(left, right, xAxis);
        SetLimitsY(bottom, top, yAxis);
    }

    public void SetLimits(double? left = null, double? right = null, double? bottom = null, double? top = null)
    {
        SetLimitsX(left ?? Bottom.Min, right ?? Bottom.Max);
        SetLimitsY(bottom ?? Left.Min, top ?? Left.Max);
    }

    public void SetLimits(CoordinateRect rect)
    {
        SetLimits(rect.Left, rect.Right, rect.Bottom, rect.Top);
    }

    public void SetLimitsX(CoordinateRect limits)
    {
        SetLimitsX(limits.Left, limits.Right, Bottom);
    }

    public void SetLimitsY(CoordinateRect limits)
    {
        SetLimitsY(limits.Bottom, limits.Top, Left);
    }

    public void SetLimitsX(AxisLimits limits)
    {
        SetLimitsX(limits.Left, limits.Right);
    }

    public void SetLimitsX(AxisLimits limits, IXAxis xAxis)
    {
        SetLimitsX(limits.Left, limits.Right, xAxis);
    }

    public void SetLimitsY(AxisLimits limits)
    {
        SetLimitsY(limits.Bottom, limits.Top);
    }

    public void SetLimitsY(AxisLimits limits, IYAxis yAxis)
    {
        SetLimitsY(limits.Bottom, limits.Top, yAxis);
    }

    public void SetLimits(AxisLimits limits)
    {
        SetLimits(limits, Bottom, Left);
    }

    public void SetLimits(AxisLimits limits, IXAxis xAxis, IYAxis yAxis)
    {
        SetLimitsX(limits.Left, limits.Right, xAxis);
        SetLimitsY(limits.Bottom, limits.Top, yAxis);
    }

    public void SetLimits(CoordinateRange xRange, CoordinateRange yRange)
    {
        AxisLimits limits = new(xRange.Min, xRange.Max, yRange.Min, yRange.Max);
        SetLimits(limits);
    }

    /// <summary>
    /// Return the 2D axis limits for the default axes
    /// </summary>
    public AxisLimits GetLimits()
    {
        return new AxisLimits(
            Bottom.Min,
            Bottom.Max,
            Left.Min,
            Left.Max);
    }

    /// <summary>
    /// Return the 2D axis limits for the given X/Y axis pair
    /// </summary>
    public AxisLimits GetLimits(IXAxis xAxis, IYAxis yAxis)
    {
        return new AxisLimits(xAxis.Min, xAxis.Max, yAxis.Min, yAxis.Max);
    }

    /// <summary>
    /// Return the 2D axis limits for the given X/Y axis pair
    /// </summary>
    public AxisLimits GetLimits(IAxes axes)
    {
        return GetLimits(axes.XAxis, axes.YAxis);
    }

    /// <summary>
    /// Adds the default X and Y axes to all plottables with unset axes
    /// </summary>
    internal void ReplaceNullAxesWithDefaults()
    {
        foreach (var plottable in Plot.PlottableList)
        {
            if (plottable.Axes.XAxis is null)
                plottable.Axes.XAxis = Bottom;

            if (plottable.Axes.YAxis is null)
                plottable.Axes.YAxis = Left;
        }
    }

    /// <summary>
    /// Automatically scale all axes to fit the data in all plottables
    /// </summary>
    public void AutoScale()
    {
        ReplaceNullAxesWithDefaults();
        AutoScaler.AutoScaleAll(Plot.PlottableList);
    }

    /// <summary>
    /// Automatically expand the default axes to fit the data in all plottables.
    /// </summary>
    public void AutoScaleExpand()
    {
        AutoScaleExpand(Bottom, Left);
    }

    /// <summary>
    /// Automatically expand the provided axes to fit the data in all plottables.
    /// </summary>
    public void AutoScaleExpand(IXAxis xAxis, IYAxis yAxis)
    {
        ReplaceNullAxesWithDefaults();
        AxisLimits suggestedLimits = AutoScaler.GetAxisLimits(Plot, xAxis, yAxis);
        AxisLimits currentLimits = GetLimits();

        ExpandingAxisLimits limits = new(currentLimits);
        limits.Expand(suggestedLimits);
        SetLimits(limits.AxisLimits);
    }

    /// <summary>
    /// Automatically expand the provided axes horizontally to fit the data in all plottables.
    /// </summary>
    public void AutoScaleExpandX(IXAxis xAxis)
    {
        ReplaceNullAxesWithDefaults();
        AxisLimits suggestedLimits = AutoScaler.GetAxisLimits(Plot, xAxis, Left);
        AxisLimits currentLimits = GetLimits();

        ExpandingAxisLimits limits = new(currentLimits);
        limits.Expand(suggestedLimits);
        SetLimitsX(limits.AxisLimits);
    }

    /// <summary>
    /// Automatically expand the default horizontal axis to fit the data in all plottables.
    /// </summary>
    public void AutoScaleExpandX()
    {
        AutoScaleExpandX(Bottom);
    }

    /// <summary>
    /// Automatically expand the provided axes vertically to fit the data in all plottables.
    /// </summary>
    public void AutoScaleExpandY(IYAxis yAxis)
    {
        ReplaceNullAxesWithDefaults();
        AxisLimits suggestedLimits = AutoScaler.GetAxisLimits(Plot, Bottom, yAxis);
        AxisLimits currentLimits = GetLimits();

        ExpandingAxisLimits limits = new(currentLimits);
        limits.Expand(suggestedLimits);
        SetLimitsY(limits.AxisLimits);
    }

    /// <summary>
    /// Automatically expand the default vertical axis to fit the data in all plottables.
    /// </summary>
    public void AutoScaleExpandY()
    {
        AutoScaleExpandY(Left);
    }

    public void AutoScaleX()
    {
        AutoScaleX(Bottom);
    }

    public void AutoScaleY()
    {
        AutoScaleY(Left);
    }

    public void AutoScaleX(IXAxis xAxis)
    {
        ReplaceNullAxesWithDefaults();
        AxisLimits limits = AutoScaler.GetAxisLimits(Plot, xAxis, Left);
        SetLimitsX(limits.Left, limits.Right, xAxis);
    }

    public void AutoScaleY(IYAxis yAxis)
    {
        ReplaceNullAxesWithDefaults();
        AxisLimits limits = AutoScaler.GetAxisLimits(Plot, Bottom, yAxis);
        SetLimitsY(limits.Bottom, limits.Top, yAxis);
    }

    /// <summary>
    /// Autoscale the given axes to accommodate the data from all plottables that use them
    /// </summary>
    public void AutoScale(IXAxis xAxis, IYAxis yAxis, bool horizontal = true, bool vertical = true)
    {
        ReplaceNullAxesWithDefaults();

        AxisLimits limits = AutoScaler.GetAxisLimits(Plot, xAxis, yAxis);

        if (horizontal)
        {
            SetLimitsX(limits.Left, limits.Right, xAxis);
        }

        if (vertical)
        {
            SetLimitsY(limits.Bottom, limits.Top, yAxis);
        }
    }

    /// <summary>
    /// Autoscale axes limited to the data area defined by the supplied list. Supplied IPlottable objects that
    /// are not members of the current plot will be ignored.
    /// </summary>
    public void AutoScaleLimited(List<IPlottable> plottables)
    {
        ReplaceNullAxesWithDefaults();
        AutoScaler.AutoScaleAll(plottables.Where(x => Plot.PlottableList.Contains(x)));
    }

    public void AutoScaleLimitedX(List<IPlottable> plottables)
    {
        ReplaceNullAxesWithDefaults();
        plottables = plottables.Where(x => Plot.PlottableList.Contains(x)).ToList();

        AxisLimits initialLimits = plottables[0].GetAxisLimits();
        double minX = initialLimits.Left;
        double maxX = initialLimits.Right;

        for (int i = 1; i < plottables.Count; i++)
        {
            AxisLimits limits = plottables[i].GetAxisLimits();

            if (limits.Left < minX)
                minX = limits.Left;

            if (limits.Right > maxX)
                maxX = limits.Right;
        }

        IEnumerable<IXAxis> xAxes = plottables.Select(x => x.Axes.XAxis).Distinct();

        foreach (IXAxis axis in xAxes)
            SetLimitsX(minX, maxX, axis);
    }

    public void AutoScaleLimitedY(List<IPlottable> plottables)
    {
        ReplaceNullAxesWithDefaults();
        plottables = plottables.Where(x => Plot.PlottableList.Contains(x)).ToList();

        AxisLimits initialLimits = plottables[0].GetAxisLimits();
        double minY = initialLimits.Bottom;
        double maxY = initialLimits.Top;

        for (int i = 1; i < plottables.Count; i++)
        {
            AxisLimits limits = plottables[i].GetAxisLimits();

            if (limits.Bottom < minY)
                minY = limits.Bottom;

            if (limits.Top > maxY)
                maxY = limits.Top;
        }

        IEnumerable<IYAxis> yAxes = plottables.Select(x => x.Axes.YAxis).Distinct();

        foreach (IYAxis axis in yAxes)
            SetLimitsY(minY, maxY, axis);
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
        if (Plot.RenderManager.LastRender.Count == 0)
            throw new InvalidOperationException("at least one render is required before pixel panning is possible");

        XAxes.ForEach(ax => ax.Range.Pan(ax.GetCoordinateDistance(distance.Width, Plot.RenderManager.LastRender.DataRect)));
        YAxes.ForEach(ax => ax.Range.Pan(ax.GetCoordinateDistance(distance.Height, Plot.RenderManager.LastRender.DataRect)));
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

    /// <summary>
    /// Zoom out so the new view is the given fraction of the original view
    /// </summary>
    public void ZoomOut(double x = 1.0, double y = 1.0)
    {
        XAxes.ForEach(xAxis => xAxis.Range.ZoomOut(x));
        YAxes.ForEach(yAxis => yAxis.Range.ZoomOut(y));
    }

    /// <summary>
    /// Zoom out so the new view is the given fraction of the original view
    /// </summary>
    public void ZoomOutX(double x = 1.0)
    {
        ZoomOut(x, 1);
    }

    /// <summary>
    /// Zoom out so the new view is the given fraction of the original view
    /// </summary>
    public void ZoomOutY(double y = 1.0)
    {
        ZoomOut(1, y);
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
    public void Margins(double left = .05, double right = .05, double bottom = .07, double top = .07)
    {
        AutoScaler = new AutoScalers.FractionalAutoScaler(left, right, bottom, top);
        AutoScale();
    }
}
