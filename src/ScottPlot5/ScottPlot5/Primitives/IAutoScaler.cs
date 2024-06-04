namespace ScottPlot;

/// <summary>
/// Contains logic for determining new axis limits when Autoscale() is called
/// </summary>
public interface IAutoScaler
{
    // TODO: Add this so auto-scalers can provide scaling information without requiring plottable collections
    //AxisLimits GetRecommendedLimits(AxisLimits originalLimits);

    /// <summary>
    /// Return the recommended axis limits for the plottables that use the given axes
    /// </summary>
    AxisLimits GetAxisLimits(Plot plot, IXAxis xAxis, IYAxis yAxis);

    /// <summary>
    /// Autoscale every unset axis used by plottables.
    /// </summary>
    public void AutoScaleAll(IEnumerable<IPlottable> plottables);

    // TODO: axis-specific autoscaling can be moved out of the control class and placed somewhere in here

    // TODO: GetRecommendedAxisLimits() should return a dictionary of limits by axis,
    // then both functions can be collapsed into one.

    public bool InvertedX { get; set; }
    public bool InvertedY { get; set; }
}
