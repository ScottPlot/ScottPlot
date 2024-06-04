namespace ScottPlot.AxisLimitManagers;

/// <summary>
/// Show the entire range of data, changing the axis limits only
/// when the data extends outside the current view.
/// </summary>
public class Full : IAxisLimitManager
{
    /// <summary>
    /// Defines the amount of whitespace added to the right of the data when data runs outside the current view.
    /// 1.0 for a view that tightly fits the data and is always changing.
    /// 2.0 for a view that doubles the horizontal span when new data runs outside the current view.
    /// </summary>
    public double ExpansionRatio { get; set; } = .25;

    public AxisLimits GetAxisLimits(AxisLimits viewLimits, AxisLimits dataLimits)
    {
        bool rightEdgeIsTooClose = viewLimits.Right < dataLimits.Right;
        bool rightEdgeIsTooFar = viewLimits.Right > dataLimits.Right + dataLimits.HorizontalSpan;
        bool replaceRight = rightEdgeIsTooClose || rightEdgeIsTooFar;
        double xMax = replaceRight
            ? dataLimits.Right + dataLimits.HorizontalSpan * ExpansionRatio
            : viewLimits.Right;

        bool topEdgeIsTooClose = viewLimits.Top < dataLimits.Top;
        bool topEdgeIsTooFar = viewLimits.Top < dataLimits.Top + dataLimits.VerticalSpan;
        bool replaceTop = topEdgeIsTooClose || topEdgeIsTooFar;
        double yMax = replaceTop
            ? dataLimits.Top + dataLimits.VerticalSpan * ExpansionRatio
            : viewLimits.Top;

        bool bottomEdgeIsTooClose = viewLimits.Bottom > dataLimits.Bottom;
        bool bottomEdgeIsTooFar = viewLimits.Bottom > dataLimits.Bottom - dataLimits.VerticalSpan;
        bool replaceBottom = bottomEdgeIsTooClose || bottomEdgeIsTooFar;
        double yMin = replaceBottom
            ? dataLimits.Bottom - dataLimits.VerticalSpan * ExpansionRatio
            : viewLimits.Bottom;

        return new AxisLimits(dataLimits.Left, xMax, yMin, yMax);
    }
}
