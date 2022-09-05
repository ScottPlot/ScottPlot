namespace ScottPlot.DataSource;

/// <summary>
/// This interface is used by plottables to access data while rendering.
/// This interface describes a data source which contains points in XY space.
/// </summary>
public interface IScatterSource : IHasAxisLimits
{
    IReadOnlyList<Coordinates> GetScatterPoints();
}
