namespace ScottPlot.TickGenerators;

public interface IDateTimeTickGenerator : ITickGenerator
{
    IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates);
}
