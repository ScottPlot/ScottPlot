namespace ScottPlot.Plottable.AxisManagers;

/// <summary>
/// Slide the view to the right to keep the newest data points in view
/// </summary>
public class Slide : IAxisManager
{
    /// <summary>
    /// Amount of horizontal area to display (in axis units)
    /// </summary>
    public double Width { get; set; } = 1000;

    /// <summary>
    /// Defines the amount of whitespace added to the right of the data when data runs outside the current view.
    /// 0 for a view that slides every time new data is added
    /// 1 for a view that only slides forward when new data runs off the screen
    /// </summary>
    public double PaddingFractionX = 0;

    /// <summary>
    /// Defines the amount of whitespace added to the top or bottom of the data when data runs outside the current view.
    /// 0 sets axis limits to tightly fit the data height
    /// 1 sets axis limits to double the vertical span in the direction of the vertical overflow
    /// </summary>
    public double PaddingFractionY = .5;

    public AxisLimits GetAxisLimits(AxisLimits viewLimits, AxisLimits dataLimits)
    {
        double padHorizontal = Width * PaddingFractionX;
        double padVertical = viewLimits.YSpan * PaddingFractionY;

        bool xOverflow = dataLimits.XMax > viewLimits.XMax || dataLimits.XMax < viewLimits.XMin;
        double xMax = xOverflow ? dataLimits.XMax + padHorizontal : viewLimits.XMax;
        double xMin = xOverflow ? xMax - Width : viewLimits.XMin;

        bool yOverflow = dataLimits.YMin < viewLimits.YMin || dataLimits.YMax > viewLimits.YMax;
        double yMin = yOverflow ? dataLimits.YMin - padVertical : viewLimits.YMin;
        double yMax = yOverflow ? dataLimits.YMax + padVertical : viewLimits.YMax;

        return new AxisLimits(xMin, xMax, yMin, yMax);
    }
}
