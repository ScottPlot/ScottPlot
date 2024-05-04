namespace ScottPlot.AxisLimitManagers;

/// <summary>
/// Slide the view to the right to keep the newest data points in view
/// </summary>
public class Slide : IAxisLimitManager
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
        double padVertical = viewLimits.VerticalSpan * PaddingFractionY;

        bool xOverflow = dataLimits.Right > viewLimits.Right || dataLimits.Right < viewLimits.Left;
        double xMax = xOverflow ? dataLimits.Right + padHorizontal : viewLimits.Right;
        double xMin = xOverflow ? xMax - Width : viewLimits.Left;

        bool yOverflow = dataLimits.Bottom < viewLimits.Bottom || dataLimits.Top > viewLimits.Top;
        double yMin = yOverflow ? dataLimits.Bottom - padVertical : viewLimits.Bottom;
        double yMax = yOverflow ? dataLimits.Top + padVertical : viewLimits.Top;

        return new AxisLimits(xMin, xMax, yMin, yMax);
    }
}
