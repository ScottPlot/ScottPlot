namespace ScottPlot.DataSources;

/// <summary>
/// This interface describes data sources which have 2D data
/// </summary>
public interface IHasAxisLimits
{
    CoordinateRange GetLimitsX();
    CoordinateRange GetLimitsY();
    AxisLimits GetLimits();
}
