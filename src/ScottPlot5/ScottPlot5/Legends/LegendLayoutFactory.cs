namespace ScottPlot.Legends;

public static class LegendLayoutFactory
{
    public static LegendLayout LegendOnPlot(Legend legend, PixelRect dataRect)
    {
        LegendItem[] items = legend.GetItems();

        PixelSize[] itemSizes = items.Select(x => x.Measure()).ToArray();
        float maxLabelWidth = itemSizes.Select(x => x.Width).Max();
        float heightOfAllLabels = itemSizes.Select(x => x.Height).Sum();

        float legendWidth = legend.SymbolWidth + legend.SymbolPadding + maxLabelWidth;
        float legendHeight = heightOfAllLabels + legend.VerticalSpacing * (items.Length - 1);

        PixelSize legendSize = new PixelSize(legendWidth, legendHeight).Expanded(legend.Padding);
        PixelRect legendRect = legendSize.AlignedInside(dataRect, legend.Location, legend.Margin);

        PixelRect[] labelRects = new PixelRect[items.Length];
        PixelRect[] symbolRects = new PixelRect[items.Length];
        float nextItemY = legendRect.Top + legend.Padding.Top;
        for (int i = 0; i < items.Length; i++)
        {
            float top = nextItemY;
            float bottom = nextItemY + itemSizes[i].Height;

            float symbolLeft = legendRect.Left + legend.Padding.Left;
            float symbolRight = symbolLeft + legend.SymbolWidth;
            symbolRects[i] = new(symbolLeft, symbolRight, bottom, top);

            float labelOffsetX = legend.Padding.Left + legend.SymbolWidth + legend.SymbolPadding;
            float labelLeft = legendRect.Left + labelOffsetX;
            float labelRight = labelLeft + maxLabelWidth;
            labelRects[i] = new(labelLeft, labelRight, bottom, top);

            nextItemY += itemSizes[i].Height + legend.VerticalSpacing;
        }

        return new LegendLayout
        {
            LegendItems = items,
            LegendRect = legendRect,
            LabelRects = labelRects,
            SymbolRects = symbolRects,
        };
    }

    // TODO: support this later
    public static LegendLayout StandaloneLegend(Legend legend, int maxWidth, int maxHeight)
    {
        return new LegendLayout
        {
            LegendItems = [],
            LegendRect = PixelRect.NaN,
            LabelRects = [],
            SymbolRects = [],
        };
    }
}
