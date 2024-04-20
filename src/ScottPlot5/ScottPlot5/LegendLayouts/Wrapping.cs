namespace ScottPlot.LegendLayouts;

public class Wrapping : ILegendLayout
{
    public LegendLayout GetLayout(Legend legend, LegendItem[] items, PixelSize maxSize)
    {
        PixelSize maxSizeAfterPadding = maxSize.Contracted(legend.Padding);

        PixelSize[] labelSizes = items.Select(x => legend.FontStyle.MeasureMultiline(x.LabelText)).ToArray();
        float maxLabelWidth = labelSizes.Select(x => x.Width).Max();
        float maxLabelHeight = labelSizes.Select(x => x.Height).Max();
        PixelSize itemSize = new(legend.SymbolWidth + legend.SymbolPadding + maxLabelWidth, maxLabelHeight);

        PixelRect[] labelRects = new PixelRect[items.Length];
        PixelRect[] symbolRects = new PixelRect[items.Length];

        Pixel nextPixel = new(0, 0);
        for (int i = 0; i < items.Length; i++)
        {
            // if the next position will cause an overflow, wrap to the next position
            if (legend.Orientation == Orientation.Horizontal)
            {
                if (nextPixel.X + itemSize.Width > maxSizeAfterPadding.Width)
                {
                    nextPixel = new(0, nextPixel.Y + itemSize.Height + legend.InterItemPadding.Bottom);
                }
            }
            else
            {
                if (nextPixel.Y + itemSize.Height > maxSizeAfterPadding.Height)
                {
                    nextPixel = new(nextPixel.X + itemSize.Width + legend.InterItemPadding.Right, 0);
                }
            }

            // create rectangles for the item using the current position
            PixelRect itemRect = new(nextPixel, itemSize);
            symbolRects[i] = new(itemRect.Left, itemRect.Left + legend.SymbolWidth, itemRect.Bottom, itemRect.Top);
            labelRects[i] = new(itemRect.Left + legend.SymbolWidth + legend.SymbolPadding, itemRect.Right, itemRect.Bottom, itemRect.Top);

            // move the position forward according to the size of this item

            if (legend.Orientation == Orientation.Horizontal)
            {
                nextPixel = new(nextPixel.X + itemSize.Width + legend.InterItemPadding.Right, nextPixel.Y);
            }
            else
            {
                nextPixel = new(nextPixel.X, nextPixel.Y + itemSize.Height + legend.InterItemPadding.Bottom);
            }
        }

        float tightWidth = labelRects.Select(x => x.Right).Max();
        float tightHeight = labelRects.Select(x => x.Bottom).Max();
        PixelRect legendRect = new(0, tightWidth + legend.Padding.Horizontal, tightHeight + legend.Padding.Vertical, 0);
        PixelOffset paddingOffset = new(legend.Padding.Left, legend.Padding.Top);

        return new LegendLayout
        {
            LegendItems = items,
            LegendRect = legendRect,
            LabelRects = labelRects.Select(x => x.WithOffset(paddingOffset)).ToArray(),
            SymbolRects = symbolRects.Select(x => x.WithOffset(paddingOffset)).ToArray(),
        };
    }
}
