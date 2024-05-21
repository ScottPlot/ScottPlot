namespace ScottPlot.LegendLayouts;

public class Wrapping : ILegendLayout
{
    public LegendLayout GetLayout(Legend legend, LegendItem[] items, PixelSize maxSize)
    {
        using SKPaint paint = new();
        PixelSize maxSizeAfterPadding = maxSize.Contracted(legend.Padding);
        PixelRect maxRectAfterPadding = new(0, maxSizeAfterPadding.Width, maxSizeAfterPadding.Height, 0);
        PixelSize[] labelSizes = items.Select(x => x.LabelStyle.Measure(x.LabelText, paint).Size).ToArray();
        float maxLabelWidth = labelSizes.Select(x => x.Width).Max();
        float maxLabelHeight = labelSizes.Select(x => x.Height).Max();
        float maxItemWidth = legend.SymbolWidth + legend.SymbolPadding + maxLabelWidth;
        float maxItemHeight = maxLabelHeight;

        PixelRect[] labelRects = new PixelRect[items.Length];
        PixelRect[] symbolRects = new PixelRect[items.Length];

        Pixel nextPixel = new(0, 0);
        for (int i = 0; i < items.Length; i++)
        {
            float itemWidth = legend.TightHorizontalWrapping
                ? legend.SymbolWidth + legend.SymbolPadding + labelSizes[i].Width
                : maxItemWidth;

            // if the next position will cause an overflow, wrap to the next position
            if (legend.Orientation == Orientation.Horizontal)
            {
                if (nextPixel.X + itemWidth > maxSizeAfterPadding.Width)
                {
                    nextPixel = new(0, nextPixel.Y + maxItemHeight + legend.InterItemPadding.Bottom);
                }
            }
            else
            {
                if (nextPixel.Y + maxItemHeight > maxSizeAfterPadding.Height)
                {
                    nextPixel = new(nextPixel.X + itemWidth + legend.InterItemPadding.Right, 0);
                }
            }

            // create rectangles for the item using the current position
            PixelRect itemRect = new(nextPixel, new PixelSize(itemWidth, maxItemHeight));
            itemRect = itemRect.Intersect(maxRectAfterPadding);

            symbolRects[i] = new(itemRect.Left, itemRect.Left + legend.SymbolWidth, itemRect.Bottom, itemRect.Top);
            labelRects[i] = new(
                left: itemRect.Left + legend.SymbolWidth + legend.SymbolPadding,
                right: itemRect.Right,
                bottom: itemRect.Bottom,
                top: itemRect.Top);

            // move the position forward according to the size of this item

            if (legend.Orientation == Orientation.Horizontal)
            {
                nextPixel = new(nextPixel.X + itemWidth + legend.InterItemPadding.Right, nextPixel.Y);
            }
            else
            {
                nextPixel = new(nextPixel.X, nextPixel.Y + maxItemHeight + legend.InterItemPadding.Bottom);
            }
        }

        float tightWidth = Math.Min(labelRects.Select(x => x.Right).Max(), maxSizeAfterPadding.Width);
        float tightHeight = Math.Min(labelRects.Select(x => x.Bottom).Max(), maxSizeAfterPadding.Height);
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
