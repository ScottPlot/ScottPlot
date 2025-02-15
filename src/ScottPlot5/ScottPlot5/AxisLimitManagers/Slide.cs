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

    public CoordinateRange GetRangeX(CoordinateRange viewRangeX, CoordinateRange dataRangeX)
    {
        double padHorizontal = Width * PaddingFractionX;

        bool xOverflow = dataRangeX.Max > viewRangeX.Max || dataRangeX.Max < viewRangeX.Min;
        double xMax = xOverflow ? dataRangeX.Max + padHorizontal : viewRangeX.Max;
        double xMin = xOverflow ? xMax - Width : viewRangeX.Min;

        return new CoordinateRange(xMin, xMax);
    }

    public CoordinateRange GetRangeY(CoordinateRange viewRangeY, CoordinateRange dataRangeY)
    {
        double padVertical = viewRangeY.Span * PaddingFractionY;

        bool yOverflow = dataRangeY.Min < viewRangeY.Min || dataRangeY.Max > viewRangeY.Max;
        double yMin = yOverflow ? dataRangeY.Min - padVertical : viewRangeY.Min;
        double yMax = yOverflow ? dataRangeY.Max + padVertical : viewRangeY.Max;

        return new CoordinateRange(yMin, yMax);
    }
}
