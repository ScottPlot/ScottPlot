namespace ScottPlot.Legends;

public class StandardLegend : ILegend
{
    public bool IsVisible { get; set; } = true;
    public Alignment Alignment { get; set; } = Alignment.LowerRight;
    public PixelPadding Margin { get; set; } = new PixelPadding(8);
    public PixelPadding Padding { get; set; } = new PixelPadding(3);
    public PixelPadding ItemPadding { get; set; } = new PixelPadding(3);

    // TODO: use a class to store these grouped font properties
    public string FontName = Font.SansFontName;
    public float FontSize = 12;
    public Color FontColor = Colors.Black;
    public bool FontBold = false;
    public bool FontItalic = false;
    public Font Font => new(FontName, FontSize, FontBold ? 800 : 400, FontItalic);

    public float LineWidth = 1;
    public Color LineColor = Colors.Black;
    public Color BackgroundColor = Colors.White;

    public Color ShadowColor = Colors.Black.WithOpacity(.2);
    public float ShadowOffset = 3;
    public Alignment ShadowAlignment = Alignment.LowerRight;

    private const float SymbolWidth = 20;
    private const float SymbolLabelSeparation = 5;

    public void Render(SKCanvas canvas, PixelRect dataRect, LegendItem[] items)
    {
        items = GetVisibleItems(items);
        if (!items.Any())
            return;

        // TODO: I don't like the recursion or frequent measurements that happen in the render loop.
        // This section should be refactored to:
        //   1.) Recursively identify every item to show in the legend and store it as LegendItem[]
        //   2.) Measure every item and store it as PixelSize[]
        //   3.) Render each item one by one

        // measure all items to determine dimensions of the legend
        using SKPaint paint = new() { Typeface = Font.ToSKTypeface(), TextSize = FontSize, IsAntialias = true };
        PixelSize[] itemSizes = items.Select(x => Measure(x, paint)).ToArray();
        float maxWidth = itemSizes.Select(x => x.Width).Max();
        float totalheight = itemSizes.Select(x => x.Height).Sum();
        PixelSize legendSize = new PixelSize(maxWidth, totalheight).Expand(Padding);
        PixelRect legendRect = legendSize.AlignedInside(dataRect, Alignment.LowerRight, Margin);
        PixelRect legendShadowRect = legendRect.WithDelta(ShadowOffset, ShadowOffset, Alignment);

        // render the legend panel
        Drawing.Fillectangle(canvas, legendShadowRect, ShadowColor);
        Drawing.Fillectangle(canvas, legendRect, BackgroundColor);
        Drawing.DrawRectangle(canvas, legendRect, LineColor, LineWidth);

        // render all items inside the legend
        float yOffset = legendRect.Top + Padding.Top;
        for (int i = 0; i < items.Length; i++)
        {
            RenderItem(canvas, paint, items[i], legendRect.Left + Padding.Left, yOffset);
            yOffset += itemSizes[i].Height;
        }
    }

    private void RenderItem(SKCanvas canvas, SKPaint paint, LegendItem item, float x, float y)
    {
        if (string.IsNullOrEmpty(item.Label))
            throw new ArgumentNullException(nameof(item.Label));

        SKPoint textPoint = new(x, y + paint.TextSize);

        bool hasSymbol = item.Line.HasValue || item.Marker.HasValue || item.Fill.HasValue;
        if (hasSymbol)
        {
            RenderSymbol(canvas, item, x, y, paint.TextSize);
            textPoint.X += SymbolWidth + SymbolLabelSeparation;
        }

        canvas.DrawText(item.Label, textPoint, paint);

        // TODO: use child measurements that were already made
        y += Measure(item, paint, includeChildren: false).Height;
        foreach (var curr in item.Children)
        {
            RenderItem(canvas, paint, curr, x, y);
            y += Measure(curr, paint).Height;
        }
    }

    private void RenderSymbol(SKCanvas canvas, LegendItem item, float x, float y, float height)
    {
        // TODO: symbol/text alignment is pretty wonky

        PixelRect rect = new(x, x + SymbolWidth, y + height * 1.5f, y);

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

    private PixelSize Measure(LegendItem item, SKPaint paint, bool includeChildren = true)
    {
        if (string.IsNullOrWhiteSpace(item.Label))
            throw new NullReferenceException(nameof(item.Label));

        PixelSize labelRect = Drawing.MeasureString(item.Label, paint);

        float width = SymbolWidth + SymbolLabelSeparation + labelRect.Width + ItemPadding.TotalHorizontal;
        float height = paint.TextSize + Padding.TotalVertical;

        if (!includeChildren)
            return new PixelSize(width, height);

        foreach (LegendItem childItem in item.Children)
        {
            PixelSize childSize = Measure(childItem, paint);
            width = Math.Max(width, Padding.Left + childSize.Width);
            height += childSize.Height;
        }

        return new PixelSize(width, height);
    }

    private LegendItem[] GetVisibleItems(IEnumerable<LegendItem> items)
    {
        List<LegendItem> visibleItems = new();

        foreach (LegendItem item in items)
        {
            if (!string.IsNullOrWhiteSpace(item.Label))
            {
                visibleItems.Add(item);
            }

            visibleItems.AddRange(GetVisibleItems(item.Children));
        }

        return visibleItems.ToArray();
    }
}
