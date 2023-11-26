namespace ScottPlot;

/// <summary>
/// Contains logic for determining new axis limits when Autoscale() is called
/// </summary>
public interface IAutoScaler
{
    /// <summary>
    /// Return the recommended axis limits for the plottables that use the given axes
    /// </summary>
    AxisLimits GetAxisLimits(IEnumerable<IPlottable> plottables, IXAxis xAxis, IYAxis yAxis);
}
