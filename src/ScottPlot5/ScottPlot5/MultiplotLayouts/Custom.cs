namespace ScottPlot.MultiplotLayouts;

/// <summary>
/// Arranges plots from left to right according to the number of <paramref name="columns"/>, 
/// wrapping to the next row as needed to accommodate the total number of plots.
public class Custom(IEnumerable<FractionRect> rectangles) : IMultiplotLayout
{
    FractionRect[] Rectangles { get; } = rectangles.ToArray();

    public IEnumerable<(FractionRect, Plot)> GetLayout(IReadOnlyList<Plot> plots)
    {
        if (plots.Count != Rectangles.Length)
            throw new InvalidOperationException($"Attempting to arrange {plots.Count} plots using a custom layout with {Rectangles.Length} rectangles");

        for (int i = 0; i < plots.Count; i++)
        {
            yield return (Rectangles[i], plots[i]);
        }
    }
}
