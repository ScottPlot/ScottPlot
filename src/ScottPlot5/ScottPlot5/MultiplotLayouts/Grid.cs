namespace ScottPlot.MultiplotLayouts;

/// <summary>
/// Arranges plots from left to right according to the number of <paramref name="columns"/>, 
/// wrapping to the next row as needed to accommodate the total number of plots.
public class Grid(int columns) : IMultiplotLayout
{
    public int Columns { get; } = columns;

    public IEnumerable<(FractionRect, Plot)> GetLayout(IReadOnlyList<Plot> plots)
    {
        int rows = (int)Math.Ceiling((float)plots.Count / Columns);
        for (int i = 0; i < plots.Count; i++)
        {
            int x = i % Columns;
            int y = i / Columns;
            FractionRect rect = FractionRect.GridCell(x, y, Columns, rows);
            yield return (rect, plots[i]);
        }
    }
}
