namespace ScottPlot.Plottable
{
    public interface IIsHighlightable
    {
        bool IsHighlighted { get; set; }
        double HighlightCoefficient { get; set; }
    }
}
