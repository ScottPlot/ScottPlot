namespace ScottPlot.Plottable.DataLoggerViews;

/// <summary>
/// Show the entire range of data, changing the axis limits only
/// when the data extends otuside the current view.
/// </summary>
public class Full : IScatterDataLoggerView
{
    public double ExpansionRatio = 1.25;

    public void SetAxisLimits(Plot plt, AxisLimits dataLimits)
    {
        AxisLimits viewLimits = plt.GetAxisLimits();

        bool xOverflow = viewLimits.XMin > dataLimits.XMin || viewLimits.XMax < dataLimits.XMax;
        if (xOverflow)
        {
            double xMin = dataLimits.XMin;
            double xMax = dataLimits.XMax * ExpansionRatio;
            plt.SetAxisLimitsX(xMin, xMax);
        }

        bool yOverflow = viewLimits.YMin > dataLimits.YMin || viewLimits.YMax < dataLimits.YMax;
        if (yOverflow)
        {
            double ySpanHalf = (dataLimits.YSpan / 2) * ExpansionRatio;
            double yMin = dataLimits.YCenter - ySpanHalf;
            double yMax = dataLimits.YCenter + ySpanHalf;
            plt.SetAxisLimitsY(yMin, yMax);
        }
    }
}
