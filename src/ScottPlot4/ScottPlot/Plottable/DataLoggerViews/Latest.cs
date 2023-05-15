namespace ScottPlot.Plottable.DataLoggerViews;

/// <summary>
/// Continuously scroll the plot horizontally to continuously focus on the latest data.
/// </summary>
public class Latest : IDataLoggerView
{
    public int ViewWidth { get; set; } = 1000;

    /// <summary>
    /// Defines the amount of whitespace added to the right of the data when axis limits are set.
    /// Setting to 0 results in a continuously scrolling plot.
    /// Setting to 1 results in a "sweep" strip chart effect.
    /// </summary>
    public double PaddingFraction = 0;

    public void SetAxisLimits(Plot plt, AxisLimits dataLimits)
    {
        AxisLimits viewLimits = plt.GetAxisLimits();

        bool xOverflow = dataLimits.XMax > viewLimits.XMax;
        if (xOverflow)
        {
            double xMax = dataLimits.XMax + ViewWidth * PaddingFraction;
            double xMin = xMax - ViewWidth;
            plt.SetAxisLimitsX(xMin, xMax);
        }

        bool yOverflow = dataLimits.YMin < viewLimits.YMin || dataLimits.YMax > viewLimits.YMax;
        if (yOverflow)
        {
            plt.SetAxisLimitsY(dataLimits.YMin, dataLimits.YMax);
        }
    }
}
