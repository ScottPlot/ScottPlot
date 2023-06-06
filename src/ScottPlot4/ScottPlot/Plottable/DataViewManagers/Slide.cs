namespace ScottPlot.Plottable.DataViewManagers;

/// <summary>
/// Slide the view to the right to keep the newest data points in view
/// </summary>
public class Slide : IDataViewManager
{
    public int ViewWidth { get; set; } = 1000;

    /// <summary>
    /// Defines the amount of whitespace added to the right of the data when axis limits are set.
    /// 0 for a view that slides every time new data is added
    /// 1 for a view that only slides forward when new data runs off the screen
    /// </summary>
    public double PaddingFractionX = 0;

    public double PaddingFractionY = .5;

    public AxisLimits GetAxisLimits(AxisLimits viewLimits, AxisLimits dataLimits)
    {
        double padHorizontal = ViewWidth * PaddingFractionX;
        double padVertical = viewLimits.YSpan * PaddingFractionY;

        bool xOverflow = dataLimits.XMax > viewLimits.XMax || dataLimits.XMax < viewLimits.XMin;
        double xMax = xOverflow ? dataLimits.XMax + padHorizontal : viewLimits.XMax;
        double xMin = xOverflow ? xMax - ViewWidth : viewLimits.XMin;

        bool yOverflow = dataLimits.YMin < viewLimits.YMin || dataLimits.YMax > viewLimits.YMax;
        double yMin = yOverflow ? dataLimits.YMin - padVertical : viewLimits.YMin;
        double yMax = yOverflow ? dataLimits.YMax + padVertical : viewLimits.YMax;

        return new AxisLimits(xMin, xMax, yMin, yMax);
    }
}
