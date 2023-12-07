namespace ScottPlot;

/// <summary>
/// Holds a collection of bars which are all styled the same and have a common label
/// </summary>
[Obsolete("temporarially not in use", true)]
public class BarSeries
{
    public IList<Bar> Bars { get; set; } = Array.Empty<Bar>();
    public string? Label { get; set; }
    public Color Color { get; set; }
}
