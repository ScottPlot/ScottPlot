namespace ScottPlot;

/// <summary>
/// Contains logic for determining new axis limits when Autoscale() is called
/// </summary>
public interface IAutoScaler
{
    /// <summary>
    /// Return the recommended axis limits for the plottables that use the given axes
    /// </summary>
    AxisLimits GetAxisLimits(Plot plot, IXAxis xAxis, IYAxis yAxis);

    public void AutoScaleAll(IEnumerable<IPlottable> plottables); // TODO: deprecate

    public bool InvertedX { get; set; }
    public bool InvertedY { get; set; }
}
