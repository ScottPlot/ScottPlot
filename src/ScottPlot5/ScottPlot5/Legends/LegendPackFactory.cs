namespace ScottPlot.Legends;

public static class LegendPackFactory
{
    public static LegendPack StandaloneLegend(Legend legend, int maxWidth, int maxHeight)
    {
        using SKPaint paint = new();
        LegendItem[] items = GetItemLogic.GetItems(legend);
        ItemSizeAndChildren[] sizedItems = LegendSizing.GetSizedLegendItems(legend, items, paint);
        PixelSize legendSize = LegendSizing.GetSizeOfEntireLegend(legend, sizedItems, maxWidth: maxWidth, maxHeight: maxHeight, withOffset: true);
        PixelRect legendRect = new(0, legendSize.Width, legendSize.Height, 0);
        Pixel offset = Pixel.Zero;
        return new LegendPack()
        {
            SizedItems = sizedItems,
            LegendRect = legendRect,
            LegendShadowRect = legendRect,
            Offset = offset,
        };
    }

    public static LegendPack LegendOnPlot(Legend legend, PixelRect dataRect)
    {
        LegendItem[] items = GetItemLogic.GetItems(legend);

        using SKPaint paint = new();
        ItemSizeAndChildren[] sizedItems = LegendSizing.GetSizedLegendItems(legend, items, paint);

        PixelSize legendSize = LegendSizing.GetSizeOfEntireLegend(legend, sizedItems,
            maxWidth: dataRect.Width - legend.ShadowOffset * 2 - legend.Margin.Horizontal,
            maxHeight: dataRect.Height - legend.ShadowOffset * 2 - legend.Margin.Vertical,
            withOffset: false);

        PixelRect legendRect = legendSize.AlignedInside(dataRect, legend.Location, legend.Margin);
        PixelRect legendShadowRect = legendRect.WithDelta(legend.ShadowOffset, legend.ShadowOffset, legend.Location);
        Pixel offset = new(legendRect.Left + legend.Padding.Left, legendRect.Top + legend.Padding.Top);
        return new LegendPack()
        {
            SizedItems = sizedItems,
            LegendRect = legendRect,
            LegendShadowRect = legendShadowRect,
            Offset = offset,
        };
    }
}
