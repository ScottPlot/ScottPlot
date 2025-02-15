namespace ScottPlot;

/// <summary>
/// An axis manager contains logic to suggest axis limits 
/// given the current view and size of the data.
/// </summary>
public interface IAxisLimitManager
{
    /// <summary>
    /// Returns the recommended X axis range given the current view and size of the data
    /// </summary>
    /// <param name="viewRangeX">X axis view range</param>
    /// <param name="dataRangeX">X axis data range</param>
    /// <returns></returns>
    CoordinateRange GetRangeX(CoordinateRange viewRangeX, CoordinateRange dataRangeX);

    /// <summary>
    /// Returns the recommended Y axis range given the current view and size of the data
    /// </summary>
    /// <param name="viewRangeY">Y axis view range</param>
    /// <param name="dataRangeY">Y axis view range</param>
    /// <returns></returns>
    CoordinateRange GetRangeY(CoordinateRange viewRangeY, CoordinateRange dataRangeY);
}

public static class IAxisLimitManagerExtensions
{
    /// <summary>
    /// Return recommended axis limits given the current view and size of the data
    /// </summary>
    public static AxisLimits GetAxisLimits(this IAxisLimitManager manager, AxisLimits viewLimits, AxisLimits dataLimits) =>
        new(manager.GetRangeX(viewLimits.XRange, dataLimits.XRange), manager.GetRangeY(viewLimits.YRange, dataLimits.YRange));
}
