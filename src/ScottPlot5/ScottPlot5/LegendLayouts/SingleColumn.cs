namespace ScottPlot.LegendLayouts;

public class SingleColumn : ILegendLayout
{
    public LegendLayout GetLayout(Legend legend, LegendItem[] items, PixelSize maxSize)
    {
        PixelSize[] itemSizes = items.Select(x => x.MeasureLabel()).ToArray();
        float maxLabelWidth = itemSizes.Select(x => x.Width).Max();
        float heightOfAllLabels = itemSizes.Select(x => x.Height).Sum();

        float legendWidth = legend.SymbolWidth + legend.SymbolPadding + maxLabelWidth;
        float legendHeight = heightOfAllLabels + legend.InterItemPadding.Bottom * (items.Length - 1);

        PixelSize legendSize = new PixelSize(legendWidth, legendHeight).Expanded(legend.Padding);
        PixelRect legendRect = new(legendSize);

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

            nextItemY += itemSizes[i].Height + legend.InterItemPadding.Bottom;
        }

        return new LegendLayout
        {
            LegendItems = items,
            LegendRect = legendRect,
            LabelRects = labelRects,
            SymbolRects = symbolRects,
        };
    }
}
