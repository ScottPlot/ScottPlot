namespace ScottPlot.DataSource;

public interface IDataSource
{
    int Count { get; }
    Coordinates this[int index] { get; set; }
    CoordinateRange GetLimitsX();
    CoordinateRange GetLimitsY();
    AxisLimits GetLimits();
}
