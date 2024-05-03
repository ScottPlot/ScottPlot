namespace ScottPlot;

/// <summary>
/// This interface is applied to plottables which modify axis limits at render time.
/// The update method is called at render time before the ticks are calculated.
/// </summary>
public interface IManagesAxisLimits
{
    void UpdateAxisLimits(Plot plot);
    bool ManageAxisLimits { get; set; }
}
