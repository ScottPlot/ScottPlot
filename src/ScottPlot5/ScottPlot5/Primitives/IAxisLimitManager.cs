namespace ScottPlot;

/// <summary>
/// An axis manager contains logic to suggest axis limits 
/// given the current view and size of the data.
/// </summary>
public interface IAxisLimitManager
{
    /// <summary>
    /// Return recommended axis limits given the current view and size of the data
    /// </summary>
    AxisLimits GetAxisLimits(AxisLimits viewLimits, AxisLimits dataLimits);
}
