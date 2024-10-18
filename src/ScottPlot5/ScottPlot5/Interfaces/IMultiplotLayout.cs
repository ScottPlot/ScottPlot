namespace ScottPlot;

public interface IMultiplotLayout
{
    IEnumerable<(FractionRect, Plot)> GetLayout(IReadOnlyList<Plot> plots);
}
