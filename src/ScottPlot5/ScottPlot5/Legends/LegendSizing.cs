namespace ScottPlot.Legends;

[Obsolete("use new sizing class")]
public static class LegendSizing
{
    public static ItemSizeAndChildren[] GetSizedLegendItems(Legend legend, IEnumerable<LegendItem> items, SKPaint paint)
    {
        List<ItemSizeAndChildren> sizedItems = [];

        foreach (LegendItem item in items)
        {
            if (item.CustomFontStyle is not null)
            {
                item.CustomFontStyle.ApplyToPaint(paint);
            }
            else
            {
                legend.Font.ApplyToPaint(paint);
            }

            LegendItem[] visibleItems = GetItemLogic.GetAllLegendItems(legend, item.Children).Where(x => x.IsVisible).ToArray();
            ItemSizeAndChildren[] sizedChildren = GetSizedLegendItems(legend, visibleItems, paint);
            ItemSize sizeWithChildren = MeasureLegendItemAndItsChildren(item, paint, sizedChildren, legend.SymbolWidth, legend.SymbolLabelSeparation, legend.Padding, legend.ItemPadding);
            ItemSizeAndChildren sizedItem = new(item, sizeWithChildren, sizedChildren);
            sizedItems.Add(sizedItem);
        }

        return [.. sizedItems];
    }

    public static PixelSize GetSizeOfEntireLegend(Legend legend, ItemSizeAndChildren[] sizedItems, float maxWidth = 0, float maxHeight = 0, bool withOffset = false)
    {
        if (sizedItems.Length == 0)
            return PixelSize.Zero;

        float legendWidth, legendHeight;

        if (legend.Orientation == Orientation.Vertical)
        {
            legendWidth = sizedItems.Select(x => x.Size.WithChildren.Width).Max();
            legendHeight = sizedItems.Select(x => x.Size.WithChildren.Height).Sum();
        }
        else if (maxWidth > 0)
        {
            float lw = 0;
            float lh = 0;
            legendWidth = legendHeight = 0;
            foreach (var item in sizedItems)
            {
                if (item.Size.WithChildren.Width + legendWidth > maxWidth)
                {
                    lw = Math.Max(lw, legendWidth);
                    legendWidth = 0;
                    legendHeight += lh;
                    lh = 0;
                }
                legendWidth += item.Size.WithChildren.Width;
                lh = Math.Max(lh, item.Size.WithChildren.Height);
            }
            legendHeight += lh;
            legendWidth = Math.Max(lw, legendWidth);
        }
        else
        {
            legendWidth = sizedItems.Select(x => x.Size.WithChildren.Width).Sum();
            legendHeight = sizedItems.Select(x => x.Size.WithChildren.Height).Max();
        }

        legendWidth += legend.Padding.Left + legend.Padding.Right + (withOffset ? 2 * legend.ShadowOffset : 0);
        legendHeight += legend.Padding.Top + legend.Padding.Bottom + (withOffset ? 2 * legend.ShadowOffset : 0);

        if (maxWidth > 0)
            legendWidth = Math.Min(legendWidth, maxWidth);
        if (maxHeight > 0)
            legendHeight = Math.Min(legendHeight, maxHeight);

        return new(legendWidth, legendHeight);
    }

    /// <summary>
    /// Return the size of the given item including all its children
    /// </summary>
    private static ItemSize MeasureLegendItemAndItsChildren(LegendItem item, SKPaint paint, ItemSizeAndChildren[] sizeAndChildren, 
        float symbolWidth, float symbolPadRight, PixelPadding Padding, PixelPadding ItemPadding)
    {
        PixelSize labelRect = !string.IsNullOrWhiteSpace(item.Label)
            ? item.LabelStyle.MeasureMultiLines(paint, item.LabelText)
            : new(0, 0);

        Console.WriteLine($"TEXT: {item.LabelText} \t HEIGHT: {labelRect.Height}");

        float width2 = item.HasSymbol ? symbolWidth : 0;
        float width = width2 + symbolPadRight + labelRect.Width + ItemPadding.Horizontal;
        float height = paint.TextSize + Padding.Vertical;

        PixelSize ownSize = new(width, height);

        foreach (ItemSizeAndChildren childItem in sizeAndChildren)
        {
            width = Math.Max(width, Padding.Left + childItem.Size.WithChildren.Width);
            height += childItem.Size.WithChildren.Height;
        }

        return new ItemSize(ownSize, new(width, height));
    }
}
