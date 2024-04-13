namespace ScottPlot;

public interface IMinorTickGenerator
{
    IEnumerable<double> GetMinorTicks(double[] majorTicks, CoordinateRange visibleRange);
}
