namespace ScottPlot.Legends;

public static class LegendPackFactory
{
    public static LegendPack LegendOnPlot(Legend legend, PixelRect dataRect)
    {
        LegendItem[] items = legend.GetItems();

        PixelSize[] itemSizes = items.Select(x => x.Measure()).ToArray();
        float maxWidth = itemSizes.Select(x => x.Width).Max();
        float totalHeight = itemSizes.Select(x => x.Height).Sum();
        totalHeight += legend.VerticalSpacing.Length * (items.Length - 1);

        PixelSize legendSize = new PixelSize(maxWidth, totalHeight).Expanded(legend.Padding);
        PixelRect legendRect = legendSize.AlignedInside(dataRect, legend.Location, legend.Margin);

        Console.WriteLine(dataRect);
        Console.WriteLine(legendRect);

        PixelRect[] itemRects = new PixelRect[items.Length];
        float nextItemY = legendRect.Top + legend.Padding.Top;
        for (int i = 0; i < items.Length; i++)
        {
            float left = legendRect.Left + legend.Padding.Left;
            float right = legendRect.Right - +legend.Padding.Right;
            float top = nextItemY;
            float bottom = nextItemY + itemSizes[i].Height;
            itemRects[i] = new(left, right, top, bottom);
            nextItemY += itemSizes[i].Height + legend.VerticalSpacing.Length;
        }

        return new LegendPack
        {
            LegendItems = items,
            LegendItemRects = itemRects,
            LegendRect = legendRect,
        };
    }

    // TODO: support this later
    public static LegendPack StandaloneLegend(Legend legend, int maxWidth, int maxHeight)
    {
        return new LegendPack
        {
            LegendItems = [],
            LegendItemRects = [],
            LegendRect = PixelRect.NaN,
        };
    }
}
