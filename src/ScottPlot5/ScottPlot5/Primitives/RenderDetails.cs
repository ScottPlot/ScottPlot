namespace ScottPlot;

/// <summary>
/// Details about a completed render
/// </summary>
public readonly struct RenderDetails
{
    /// <summary>
    /// Size of the plot image in pixel units
    /// </summary>
    public readonly PixelRect FigureRect;

    /// <summary>
    /// Size of the data area of the plot in pixel units
    /// </summary>
    public readonly PixelRect DataRect;

    /// <summary>
    /// Distance between the data area and the edge of the figure
    /// </summary>
    public readonly PixelPadding Padding;

    /// <summary>
    /// Total time required to render this image
    /// </summary>
    public readonly TimeSpan Elapsed;

    /// <summary>
    /// Time the render was completed
    /// </summary>
    public readonly DateTime Timestamp;

    /// <summary>
    /// Each step of the render and how long it took to execute
    /// </summary>
    public readonly (string, TimeSpan)[] TimedActions;

    /// <summary>
    /// Axis limits of the primary axes for this render
    /// </summary>
    public readonly AxisLimits AxisLimits;

    /// <summary>
    /// Axis limits of the primary axes for the previous render
    /// </summary>
    public readonly AxisLimits PreviousAxisLimits;

    /// <summary>
    /// Axis limits for every axis
    /// </summary>
    public readonly Dictionary<IAxis, CoordinateRange> AxisLimitsByAxis;

    /// <summary>
    /// Axis limits of all axes from the previous render
    /// </summary>
    public readonly Dictionary<IAxis, CoordinateRange> PreviousAxisLimitsByAxis;

    /// <summary>
    /// Indicates whether the axis view (coordinate units) of this render differs from the previous
    /// </summary>
    public readonly bool AxisLimitsChanged { get; }

    /// <summary>
    /// Indicates whether the size (pixels) of this render differs from the previous
    /// </summary>
    public readonly bool SizeChanged { get; }

    /// <summary>
    /// Arrangement of all panels
    /// </summary>
    public readonly Layout Layout;

    /// <summary>
    /// The number of total renders including this one
    /// </summary>
    public readonly int Count;

    public double PxPerUnitX => DataRect.Width / AxisLimits.HorizontalSpan;
    public double PxPerUnitY => DataRect.Height / AxisLimits.VerticalSpan;
    public double UnitsPerPxX => AxisLimits.HorizontalSpan / DataRect.Width;
    public double UnitsPerPxY => AxisLimits.VerticalSpan / DataRect.Height;

    public RenderDetails(RenderPack rp, (string, TimeSpan)[] actionTimes, RenderDetails lastRender)
    {
        // TODO: extend actionTimes report individual plottables, axes, etc.
        FigureRect = rp.FigureRect;
        DataRect = rp.DataRect;
        Padding = new PixelPadding(rp.FigureRect.Size, rp.DataRect);
        Elapsed = rp.Elapsed;
        Timestamp = DateTime.Now;
        TimedActions = actionTimes;
        AxisLimits = rp.Plot.Axes.GetLimits();
        PreviousAxisLimits = lastRender.AxisLimits;
        PreviousAxisLimitsByAxis = lastRender.AxisLimitsByAxis;
        AxisLimitsByAxis = rp.Plot.Axes.GetAxes().ToDictionary(x => x, x => x.Range.ToCoordinateRange);
        Layout = rp.Layout;
        Count = lastRender.Count + 1;
        AxisLimitsChanged = AxisLimitsChangedChecker();
        SizeChanged = !DataRect.Equals(lastRender.DataRect);
    }

    /// <summary>
    /// Search the <see cref="AxisLimitsByAxis"/> for the specified <paramref name="axis"/> and calculate the pixels per unit
    /// </summary>
    /// <param name="axis">the X-Axis to search for</param>
    /// <param name="value">the pxPerUnitX</param>
    /// <returns>
    /// <see langword="true"/> if the axis was in the collection and a result was calculated. Otherwise <see langword="false"/>
    /// <br/> When false, the <paramref name="value"/> will be set to <see cref="PxPerUnitX"/>
    /// </returns>
    public bool TryGetPixelPerUnitX(IXAxis? axis, out double value)
    {
        // To-DO : Convert to 'GetPixelPerUnitX' ? Throw if axis not found ?
        if (axis is not null && AxisLimitsByAxis.TryGetValue(axis, out var range) && range.Span != 0)
        {
            value = DataRect.Width / range.Span;
            return true;
        }
        else
        {
            value = PxPerUnitX;
            return false;
        }
    }

    /// <summary>
    /// Search the <see cref="AxisLimitsByAxis"/> for the specified <paramref name="axis"/> and calculate the pixels per unit
    /// </summary>
    /// <param name="axis">the Y-Axis to search for</param>
    /// <param name="value">the pxPerUnitX</param>
    /// <returns>
    /// <see langword="true"/> if the axis was in the collection and a result was calculated. Otherwise <see langword="false"/>
    /// <br/> When false, the <paramref name="value"/> will be set to <see cref="PxPerUnitY"/>
    /// </returns>
    public bool TryGetPixelPerUnitY(IYAxis? axis, out double value)
    {
        if (axis is not null && AxisLimitsByAxis.TryGetValue(axis, out var range) && range.Span != 0)
        {
            value = DataRect.Height / range.Span;
            return true;
        }
        else
        {
            value = PxPerUnitY;
            return false;
        }
    }

    /// <summary>
    /// Search the <see cref="AxisLimitsByAxis"/> for the specified <paramref name="axis"/> and calculate the pixels per unit
    /// </summary>
    /// <param name="axis">the X-Axis to search for</param>
    /// <param name="value">the pxPerUnitX</param>
    /// <returns>
    /// <see langword="true"/> if the axis was in the collection and a result was calculated. Otherwise <see langword="false"/>
    /// <br/> When false, the <paramref name="value"/> will be set to <see cref="PxPerUnitX"/>
    /// </returns>
    public bool TryGetUnitPerPixelX(IXAxis? axis, out double value)
    {
        if (axis is not null && AxisLimitsByAxis.TryGetValue(axis, out var range))
        {
            value = range.Span / DataRect.Width;
            return true;
        }
        else
        {
            value = PxPerUnitX;
            return false;
        }
    }

    /// <summary>
    /// Search the <see cref="AxisLimitsByAxis"/> for the specified <paramref name="axis"/> and calculate the pixels per unit
    /// </summary>
    /// <param name="axis">the Y-Axis to search for</param>
    /// <param name="value">the pxPerUnitX</param>
    /// <returns>
    /// <see langword="true"/> if the axis was in the collection and a result was calculated. Otherwise <see langword="false"/>
    /// <br/> When false, the <paramref name="value"/> will be set to <see cref="PxPerUnitY"/>
    /// </returns>
    public bool TryGetUnitPerPixelY(IYAxis? axis, out double value)
    {
        if (axis is not null && AxisLimitsByAxis.TryGetValue(axis, out var range))
        {
            value = range.Span / DataRect.Height;
            return true;
        }
        else
        {
            value = PxPerUnitY;
            return false;
        }
    }

    private bool AxisLimitsChangedChecker()
    {
        if (PreviousAxisLimitsByAxis is null)
            return true;

        foreach (IAxis axis in AxisLimitsByAxis.Keys)
        {
            if (axis is null)
                continue;

            if (!PreviousAxisLimitsByAxis.ContainsKey(axis))
                return true;

            CoordinateRange rangeNow = AxisLimitsByAxis[axis];
            CoordinateRange rangeBefore = PreviousAxisLimitsByAxis[axis];
            if (rangeNow != rangeBefore)
                return true;
        }

        return false;
    }
}
