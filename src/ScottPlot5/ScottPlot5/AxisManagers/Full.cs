namespace ScottPlot.AxisManagers;

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
        bool xOverflow = viewLimits.Left > dataLimits.Left || viewLimits.Right < dataLimits.Right;
        double xMin = xOverflow ? dataLimits.Left : viewLimits.Left;
        double xMax = xOverflow ? dataLimits.Right * ExpansionRatio : viewLimits.Right;

        double ySpanHalf = (dataLimits.VerticalSpan / 2) * ExpansionRatio;
        bool yOverflow = viewLimits.Bottom > dataLimits.Bottom || viewLimits.Top < dataLimits.Top;
        double yMin = yOverflow ? dataLimits.VerticalCenter - ySpanHalf : viewLimits.Bottom;
        double yMax = yOverflow ? dataLimits.VerticalCenter + ySpanHalf : viewLimits.Top;

        return new AxisLimits(xMin, xMax, yMin, yMax);
    }
}
