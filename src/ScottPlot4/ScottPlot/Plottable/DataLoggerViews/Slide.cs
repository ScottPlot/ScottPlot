namespace ScottPlot.Plottable.DataLoggerViews;

/// <summary>
/// Slide the view to the right to keep the newest data points in view
/// </summary>
public class Slide : IDataLoggerView
{
    public int ViewWidth { get; set; } = 1000;

    /// <summary>
    /// Defines the amount of whitespace added to the right of the data when axis limits are set.
    /// 0 for a view that slides every time new data is added
    /// 1 for a view that only slides forward when new data runs off the screen
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
