namespace ScottPlot.Control;

/// <summary>
/// This object remembers limits of all axes so it can restore them later.
/// It is used for mouse interaction to translate pixel distances 
/// to coordinate distances based on previous renders.
/// </summary>
public class MultiAxisLimitManager
{
    private readonly Dictionary<IAxis, CoordinateRange> RememberedLimits = new();

    public MultiAxisLimitManager()
    {

    }

    public MultiAxisLimitManager(Plot plot)
    {
        Remember(plot.Axes.GetAxes());
    }

    public MultiAxisLimitManager(IPlotControl control)
    {
        Remember(control.Plot.Axes.GetAxes());
    }

    /// <summary>
    /// Remember the current limits of the given axis
    /// </summary>
    private void Remember(IAxis axis)
    {
        RememberedLimits[axis] = new(axis.Min, axis.Max);
    }

    /// <summary>
    /// Remember the current limits of all given axes
    /// </summary>
    private void Remember(IEnumerable<IAxis> axes)
    {
        foreach (IAxis axis in axes)
        {
            Remember(axis);
        }
    }

    /// <summary>
    /// Clear memory of previous axes and remember all the given axis limits
    /// </summary>
    public void Remember(MultiAxisLimitManager newMultiLimits)
    {
        foreach (IAxis axis in newMultiLimits.RememberedLimits.Keys)
        {
            CoordinateRange range = newMultiLimits.RememberedLimits[axis];
            RememberedLimits[axis] = new(range.Min, range.Max);
        }
    }

    /// <summary>
    /// Update all axis limits of the given plot with those previously remembered
    /// </summary>
    public void Apply(Plot plot)
    {
        foreach (IAxis axis in plot.Axes.GetAxes())
        {
            axis.Range.Min = RememberedLimits[axis].Min;
            axis.Range.Max = RememberedLimits[axis].Max;
        }
    }
}
