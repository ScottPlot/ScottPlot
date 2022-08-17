namespace ScottPlot.DataSource;

/// <summary>
/// This interface describes a data source which contains points in XY space
/// </summary>
public interface IScatterSource : IHasAxisLimits
{
    /// <summary>
    /// Plottables call this to get a readonly version of the data to use inside the Render function
    /// </summary>
    IReadOnlyList<Coordinates> GetScatterPoints();
}
