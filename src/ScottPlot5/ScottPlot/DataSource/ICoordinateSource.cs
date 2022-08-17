namespace ScottPlot.DataSource;

/// <summary>
/// This interface describes a data source which contains points in XY space
/// </summary>
public interface ICoordinateSource : IReadOnlyList<Coordinates>, IHasAxisLimits
{
}
