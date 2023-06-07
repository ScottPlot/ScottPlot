namespace ScottPlot.Plottable.AxisManagers;

/// <summary>
/// Show the entire range of data, changing the axis limits only
/// when the data extends otuside the current view.
/// </summary>
public class Full : IAxisManager
{
    /// <summary>
    /// Defines the amount of whitespace added to the right of the data when data runs outside the current view.
    /// 1.0 for a view that tightly fits the data and is always changing.
    /// 2.0 for a view that doubles the horizontal span when new data runs outside the current view.
    /// </summary>
    public double ExpansionRatio { get; set; } = 1.25;

    public AxisLimits GetAxisLimits(AxisLimits viewLimits, AxisLimits dataLimits)
    {
        bool xOverflow = viewLimits.XMin > dataLimits.XMin || viewLimits.XMax < dataLimits.XMax;
        double xMin = xOverflow ? dataLimits.XMin : viewLimits.XMin;
        double xMax = xOverflow ? dataLimits.XMax * ExpansionRatio : viewLimits.XMax;

        double ySpanHalf = (dataLimits.YSpan / 2) * ExpansionRatio;
        bool yOverflow = viewLimits.YMin > dataLimits.YMin || viewLimits.YMax < dataLimits.YMax;
        double yMin = yOverflow ? dataLimits.YCenter - ySpanHalf : viewLimits.YMin;
        double yMax = yOverflow ? dataLimits.YCenter + ySpanHalf : viewLimits.YMax;

        return new AxisLimits(xMin, xMax, yMin, yMax);
    }
}
