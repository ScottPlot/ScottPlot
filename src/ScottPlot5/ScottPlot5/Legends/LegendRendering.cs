namespace ScottPlot.Legends;

/// <summary>
/// Common methods which legends may choose to use for rendering
/// </summary>
public static class LegendRendering
{
    /// <summary>
    /// Render a leger item: its label, symbol, and all its children
    /// </summary>
    public static void RenderItem(
        SKCanvas canvas,
        SKPaint paint,
        SizedLegendItem sizedItem,
        float x,
        float y,
        float symbolWidth,
        float symbolPadRight,
        PixelPadding itemPadding)
    {
        LegendItem item = sizedItem.Item;

        SKPoint textPoint = new(x, y + paint.TextSize);
        float ownHeight = sizedItem.Size.OwnSize.Height;

        if (item.HasSymbol)
        {
            RenderSymbol(
                canvas: canvas,
                item: item,
                x: x,
                y: y + itemPadding.Bottom,
                height: ownHeight - itemPadding.TotalVertical,
                symbolWidth: symbolWidth);

            textPoint.X += symbolWidth + symbolPadRight;
        }

        using SKAutoCanvasRestore _ = new(canvas);
        if (!string.IsNullOrEmpty(item.Label))
        {
            canvas.DrawText(item.Label, textPoint, paint);
            canvas.Translate(itemPadding.Left, 0);
        }

        y += ownHeight;
        foreach (var curr in sizedItem.Children)
        {
            RenderItem(canvas, paint, curr, x, y, symbolWidth, symbolPadRight, itemPadding);
            y += curr.Size.WithChildren.Height;
        }
    }

    /// <summary>
    /// Render just the symbol of a legend
    /// </summary>
    public static void RenderSymbol(
        SKCanvas canvas,
        LegendItem item,
        float x,
        float y,
        float height,
        float symbolWidth)
    {
        // TODO: make LegendSymbol its own object that include size and padding

        PixelRect rect = new(x, x + symbolWidth, y + height, y);

        using SKPaint paint = new();

        if (item.Line.HasValue)
        {
            paint.SetStroke(item.Line.Value);
            canvas.DrawLine(new(rect.Left, rect.VerticalCenter), new(rect.Right, rect.VerticalCenter), paint);
        }

        if (item.Marker.HasValue)
        {
            paint.Style = SKPaintStyle.Fill;
            paint.Color = item.Marker.Value.Color.ToSKColor();
            Drawing.DrawMarkers(canvas, item.Marker.Value, EnumerableHelpers.One<Pixel>(new(rect.HorizontalCenter, rect.VerticalCenter)));
        }

        if (item.Fill.HasValue)
        {
            paint.SetFill(item.Fill.Value);
            canvas.DrawRect(rect.ToSKRect(), paint);
        }
    }

    /// <summary>
    /// Return the size of the given item including all its children
    /// </summary>
    public static LegendItemSize Measure(
        LegendItem item,
        SKPaint paint,
        SizedLegendItem[] children,
        float symbolWidth,
        float symbolPadRight,
        PixelPadding Padding,
        PixelPadding ItemPadding)
    {
        PixelSize labelRect = !string.IsNullOrWhiteSpace(item.Label) ? Drawing.MeasureString(item.Label, paint) : new(0, 0);

        float width2 = item.HasSymbol ? symbolWidth : 0;
        float width = width2 + symbolPadRight + labelRect.Width + ItemPadding.TotalHorizontal;
        float height = paint.TextSize + Padding.TotalVertical;

        PixelSize ownSize = new(width, height);

        foreach (SizedLegendItem childItem in children)
        {
            width = Math.Max(width, Padding.Left + childItem.Size.WithChildren.Width);
            height += childItem.Size.WithChildren.Height;
        }

        return new LegendItemSize(ownSize, new(width, height));
    }
}
