namespace ScottPlot.Plottable.LoggerViews;

public class FullLoggerView : ILoggerView
{
    private readonly Plot Plot;
    private readonly IPlottable Plottable;

    public FullLoggerView(Plot plot, IPlottable plottable)
    {
        Plot = plot;
        Plottable = plottable;
    }

    /// <summary>
    /// Update the plot's axis limits if the data extends outside the current view area
    /// </summary>
    public void UpdateAxisLimits()
    {
        AxisLimits currentLimits = Plot.GetAxisLimits();
        AxisLimits dataLimits = Plottable.GetAxisLimits();
        UpdateAxisLimitsX(currentLimits, dataLimits);
        UpdateAxisLimitsY(currentLimits, dataLimits);
    }

    /// <summary>
    /// If data extends off the page to the right, extend the view to the right only
    /// </summary>
    private void UpdateAxisLimitsX(AxisLimits currentLimits, AxisLimits dataLimits, double expandFrac = 1.25)
    {
        if (currentLimits.XMin > dataLimits.XMin || currentLimits.XMax < dataLimits.XMax)
        {
            Plot.SetAxisLimitsX(dataLimits.XMin, dataLimits.XMax * expandFrac);
        }
    }

    /// <summary>
    /// If the data extends off the page vertically, zoom out vertically
    /// </summary>
    private void UpdateAxisLimitsY(AxisLimits currentLimits, AxisLimits dataLimits, double expandFrac = 1.25)
    {
        if (currentLimits.YMin > dataLimits.YMin || currentLimits.YMax < dataLimits.YMax)
        {
            double ySpanHalf = (dataLimits.YSpan / 2) * expandFrac;
            double yMin = dataLimits.YCenter - ySpanHalf;
            double yMax = dataLimits.YCenter + ySpanHalf;
            Plot.SetAxisLimitsY(yMin, yMax);
        }
    }
}
