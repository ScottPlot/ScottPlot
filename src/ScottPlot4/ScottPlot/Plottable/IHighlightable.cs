namespace ScottPlot.Plottable
{
    public interface IHighlightable
    {
        bool IsHighlighted { get; set; }

        /// <summary>
        /// Scale lines and markers by this fraction (1.0 for no size change)
        /// </summary>
        float HighlightCoefficient { get; set; }
    }
}
