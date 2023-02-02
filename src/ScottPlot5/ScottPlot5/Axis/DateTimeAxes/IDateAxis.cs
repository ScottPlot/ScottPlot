namespace ScottPlot.Axis.DateTimeAxes;

public interface IDateAxis : IAxis
{
    IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates);
}
