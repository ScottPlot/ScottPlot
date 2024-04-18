namespace ScottPlot.Legends;

public static class GetItemLogic
{
    #region get items

    /// <summary>
    /// Return top-level items for this legend.
    /// Items may have children.
    /// </summary>
    public static LegendItem[] GetItems(Legend legend)
    {
        LegendItem[] items = legend.Plot.PlottableList
            .Where(item => item.IsVisible)
            .SelectMany(x => x.LegendItems)
            .Concat(legend.ManualItems)
            .ToArray();

        if (legend.SetBestFontOnEachRender)
        {
            foreach (LegendItem item in items)
            {
                if (item.Label is null)
                    continue;

                item.CustomFontStyle = legend.Font.Clone();
                item.CustomFontStyle.SetBestFont(item.Label);
            }
        }

        return items;
    }

    /// <summary>
    /// Recursively walk through children and return a flat array with all legend items
    /// </summary>
    public static LegendItem[] GetAllLegendItems(Legend legend, IEnumerable<LegendItem> items)
    {
        List<LegendItem> allItems = [];

        foreach (LegendItem item in items)
        {
            allItems.Add(item);
            allItems.AddRange(GetAllLegendItems(legend, item.Children));
        }

        return [.. allItems];
    }

    #endregion
}
