namespace ScottPlot.Plottable.DataLoggerViews;

/// <summary>
/// Show the entire range of data, changing the axis limits only
/// when the data extends otuside the current view.
/// </summary>
public class FullLoggerView : IDataLoggerView
{
    /// <summary>
    /// Fractional amount to increase the view are if data runs outside the current axis limits
    /// </summary>
    public double ExpansionRatio = 1.25;

    /// <summary>
    /// Set axis limits if the data extends outside the current view
    /// </summary>
    public void SetAxisLimits(Plot plt, AxisLimits dataLimits)
    {
        AxisLimits viewLimits = plt.GetAxisLimits();

        bool autoX = viewLimits.XMin > dataLimits.XMin || viewLimits.XMax < dataLimits.XMax;
        if (autoX)
        {
            double xMin = dataLimits.XMin;
            double xMax = dataLimits.XMax * ExpansionRatio;
            plt.SetAxisLimitsX(xMin, xMax);
        }

        bool autoY = viewLimits.YMin > dataLimits.YMin || viewLimits.YMax < dataLimits.YMax;
        if (autoY)
        {
            double ySpanHalf = (dataLimits.YSpan / 2) * ExpansionRatio;
            double yMin = dataLimits.YCenter - ySpanHalf;
            double yMax = dataLimits.YCenter + ySpanHalf;
            plt.SetAxisLimitsY(yMin, yMax);
        }
    }
}
