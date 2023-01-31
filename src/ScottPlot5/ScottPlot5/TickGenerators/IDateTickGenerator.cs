namespace ScottPlot.TickGenerators;

public interface IDateTickGenerator : ITickGenerator
{
    IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates);
}
