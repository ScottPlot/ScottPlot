namespace ScottPlot.DataSource;

public interface IHasAxisLimits
{
    CoordinateRange GetLimitsX();
    CoordinateRange GetLimitsY();
    AxisLimits GetLimits();
}
