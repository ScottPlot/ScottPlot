namespace ScottPlot.LegendLayouts;

public class Wrapping : ILegendLayout
{
    public LegendLayout GetLayout(Legend legend, LegendItem[] items, PixelSize maxSize, Paint paint)
    {
        // Calculate title dimensions if title exists
        PixelSize titleSize = PixelSize.Zero;
        bool hasTitle = !string.IsNullOrEmpty(legend.Title);

        if (hasTitle)
        {
            // Apply title font overrides before measuring
            LabelStyle titleStyle = legend.TitleStyle;
            if (legend.TitleFontSize is not null)
                titleStyle.FontSize = legend.TitleFontSize.Value;
            if (legend.TitleFontName is not null)
                titleStyle.FontName = legend.TitleFontName;
            if (legend.TitleFontColor is not null)
                titleStyle.ForeColor = legend.TitleFontColor.Value;

            titleSize = titleStyle.Measure(legend.Title, paint).Size;
        }

        // Reserve space for title at the top
        float titleReservedHeight = hasTitle ? titleSize.Height : 0;
        PixelSize availableForItems = new(
            maxSize.Width,
            maxSize.Height - titleReservedHeight
        );

        // Run the existing legend item layout logic with reduced available space
        PixelSize maxSizeAfterPadding = availableForItems.Contracted(legend.Padding);
        PixelRect maxRectAfterPadding = new(0, maxSizeAfterPadding.Width, maxSizeAfterPadding.Height, 0);

        PixelSize[] labelSizes = items.Select(x => x.LabelStyle.Measure(x.LabelText, paint).Size).ToArray();
        float maxLabelWidth = labelSizes.Select(x => x.Width).Max();
        float maxLabelHeight = labelSizes.Select(x => x.Height).Max();
        float maxMarkerSize = items.Select(x => x.MarkerStyle.Size).DefaultIfEmpty(0).Max();
        float maxItemSymbolWidth = Math.Max(legend.SymbolWidth, maxMarkerSize);
        float maxItemSymbolHeight = Math.Max(legend.SymbolHeight, maxMarkerSize);
        float maxItemWidth = maxItemSymbolWidth + legend.SymbolPadding + maxLabelWidth;
        float maxItemHeight = Math.Max(maxLabelHeight, maxItemSymbolHeight);

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

            float thisSymbolWidth = Math.Max(legend.SymbolWidth, items[i].MarkerStyle.Size);
            symbolRects[i] = new(itemRect.Left, itemRect.Left + thisSymbolWidth, itemRect.Bottom, itemRect.Top);
            labelRects[i] = new(
                left: itemRect.Left + thisSymbolWidth + legend.SymbolPadding,
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

        // Calculate legend content dimensions
        float itemsWidth = labelRects.Select(x => x.Right).Max();
        float itemsHeight = labelRects.Select(x => x.Bottom).Max();

        // Determine the final legend width (max of title width and items width)
        float contentWidth = hasTitle ? Math.Max(titleSize.Width, itemsWidth) : itemsWidth;
        float tightWidth = Math.Min(contentWidth, maxSize.Width - legend.Padding.Horizontal);
        float tightHeight = Math.Min(itemsHeight + titleReservedHeight, maxSize.Height - legend.Padding.Vertical);

        PixelRect finalLegendRect = new(0, tightWidth + legend.Padding.Horizontal, tightHeight + legend.Padding.Vertical, 0);

        // Position title centered at the top
        PixelRect? titleRect = null;
        if (hasTitle)
        {
            float titleX = legend.Padding.Left + (tightWidth - titleSize.Width) / 2;
            titleRect = new PixelRect(
                left: titleX,
                right: titleX + titleSize.Width,
                bottom: legend.Padding.Top + titleSize.Height,
                top: legend.Padding.Top);
        }

        // Offset all item rectangles down by title height + padding
        PixelOffset itemOffset = new(legend.Padding.Left, legend.Padding.Top + titleReservedHeight);

        return new LegendLayout
        {
            LegendItems = items,
            LegendRect = finalLegendRect,
            LabelRects = labelRects.Select(x => x.WithOffset(itemOffset)).ToArray(),
            SymbolRects = symbolRects.Select(x => x.WithOffset(itemOffset)).ToArray(),
            TitleRect = titleRect,
        };
    }
}
