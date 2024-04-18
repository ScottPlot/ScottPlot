namespace ScottPlot.Legends;

public static class GetItemLogic
{
    #region get items

    /// <summary>
    /// Return top-level items for this legend.
    /// Items may have children.
    /// </summary>
    /// <returns></returns>
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
        List<LegendItem> allItems = new();
        foreach (LegendItem item in items)
        {
            allItems.Add(item);
            allItems.AddRange(GetAllLegendItems(legend, item.Children));
        }
        return allItems.ToArray();
    }

    public static ItemSizeAndChildren[] GetSizedLegendItems(Legend legend, SKPaint paint, IEnumerable<LegendItem> allItems)
    {
        LegendItem[] items = GetAllLegendItems(legend, allItems)
            .Where(x => x.IsVisible)
            .Where(x => !string.IsNullOrWhiteSpace(x.Label))
            .ToArray();

        if (items.Length == 0)
            return [];

        // measure all items to determine dimensions of the legend
        return LegendSizing.GetSizedLegendItems(legend, items, paint);
    }

    #endregion
}
