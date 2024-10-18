namespace ScottPlot.MultiplotLayouts;

public class Columns : IMultiplotLayout
{
    public IEnumerable<(FractionRect, Plot)> GetLayout(IReadOnlyList<Plot> plots)
    {
        for (int i = 0; i < plots.Count; i++)
        {
            FractionRect rect = FractionRect.Column(i, plots.Count);
            yield return (rect, plots[i]);
        }
    }
}
