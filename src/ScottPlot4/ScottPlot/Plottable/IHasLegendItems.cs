namespace ScottPlot.Plottable
{
    public interface IHasLegendItems
    {
        /// <summary>
        /// Returns items to show in the legend. Most plottables return a single item. in this array will appear in the legend.
        /// Plottables which never appear in the legend should return an empty array (not null).
        /// </summary>
        LegendItem[] GetLegendItems();
    }
}
