namespace ScottPlot;

/// <summary>
/// Stores the <see cref="CoordinateRange"/> for all axes on the plot
/// and has methods that can be used to recall them at a future point in time.
/// </summary>
public class MultiAxisLimits(Plot plot)
{
    private readonly Dictionary<IAxis, CoordinateRange> AxisRanges = plot.Axes.GetAxes()
        .ToDictionary(x => x, x => x.GetRange());

    /// <summary>
    /// Set all axis limits to their original ranges
    /// </summary>
    public void Recall()
    {
        foreach (var axisAndRange in AxisRanges)
        {
            axisAndRange.Key.SetRange(axisAndRange.Value);
        }
    }

    /// <summary>
    /// Set limits of the given axis to its original range
    /// </summary>
    public void Recall(IAxis axis)
    {
        if (AxisRanges.TryGetValue(axis, out CoordinateRange range))
        {
            axis.SetRange(range);
        }
    }
}
