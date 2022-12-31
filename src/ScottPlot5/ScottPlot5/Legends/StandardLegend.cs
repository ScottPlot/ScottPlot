using ScottPlot.Style;

namespace ScottPlot.Legends;

internal struct LegendItemSize
{
    public PixelSize OwnSize { get; }
    public PixelSize WithChildren { get; }

    public LegendItemSize(PixelSize ownSize, PixelSize withChildren)
    {
        OwnSize = ownSize;
        WithChildren = withChildren;
    }
}

internal struct SizedLegendItem
{
    public LegendItem Item { get; }
    public LegendItemSize Size { get; }
    public SizedLegendItem[] Children { get; }

    public SizedLegendItem(LegendItem item, LegendItemSize size, SizedLegendItem[] children)
    {
        Item = item;
        Size = size;
        Children = children;
    }
}

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

    public Stroke Stroke = new();
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

        // measure all items to determine dimensions of the legend
        using SKPaint paint = new() { Typeface = Font.ToSKTypeface(), TextSize = Font.Size, IsAntialias = true };
        SizedLegendItem[] sizedItems = GetSizedLegendItems(items, paint);

        float maxWidth = sizedItems.Select(x => x.Size.WithChildren.Width).Max();
        float totalheight = sizedItems.Select(x => x.Size.WithChildren.Height).Sum();

        PixelSize legendSize = new PixelSize(maxWidth, totalheight).Expand(Padding);
        PixelRect legendRect = legendSize.AlignedInside(dataRect, Alignment.LowerRight, Margin);
        PixelRect legendShadowRect = legendRect.WithDelta(ShadowOffset, ShadowOffset, Alignment);

        // render the legend panel
        Drawing.Fillectangle(canvas, legendShadowRect, ShadowColor);
        Drawing.Fillectangle(canvas, legendRect, BackgroundColor);
        Drawing.DrawRectangle(canvas, legendRect, Stroke.Color, Stroke.Width);

        // render all items inside the legend
        float yOffset = legendRect.Top + Padding.Top;
        for (int i = 0; i < sizedItems.Length; i++)
        {
            RenderItem(canvas, paint, sizedItems[i], legendRect.Left + Padding.Left, yOffset);
            yOffset += sizedItems[i].Size.WithChildren.Height;
        }
    }

    private void RenderItem(SKCanvas canvas, SKPaint paint, SizedLegendItem sizedItem, float x, float y)
    {
        var item = sizedItem.Item;

        if (string.IsNullOrEmpty(item.Label))
            throw new ArgumentNullException(nameof(item.Label));

        SKPoint textPoint = new(x, y + paint.TextSize);
        var ownHeight = sizedItem.Size.OwnSize.Height;

        if (HasSymbol(item))
        {
            RenderSymbol(canvas, item, x, y + ItemPadding.Bottom, ownHeight - ItemPadding.TotalVertical);
            textPoint.X += SymbolWidth + SymbolLabelSeparation;
        }

        canvas.DrawText(item.Label, textPoint, paint);

        y += ownHeight;
        using SKAutoCanvasRestore _ = new(canvas);
        canvas.Translate(ItemPadding.Left, 0);

        foreach (var curr in sizedItem.Children)
        {
            RenderItem(canvas, paint, curr, x, y);
            y += curr.Size.WithChildren.Height;
        }
    }

    private bool HasSymbol(LegendItem item) => item.Line.HasValue || item.Marker.HasValue || item.Fill.HasValue;

    private void RenderSymbol(SKCanvas canvas, LegendItem item, float x, float y, float height)
    {
        PixelRect rect = new(x, x + SymbolWidth, y + height, y);

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

    private LegendItemSize Measure(LegendItem item, SKPaint paint)
    {
        return Measure(item, paint, GetSizedLegendItems(GetVisibleItems(item.Children), paint));
    }

    // This overload is faster because it uses the cached sizes of its children, rather than remeasuring them
    private LegendItemSize Measure(LegendItem item, SKPaint paint, SizedLegendItem[] children)
    {
        if (string.IsNullOrWhiteSpace(item.Label))
            throw new NullReferenceException(nameof(item.Label));

        PixelSize labelRect = Drawing.MeasureString(item.Label, paint);

        var symbolWidth = HasSymbol(item) ? SymbolWidth : 0;
        float width = symbolWidth + SymbolLabelSeparation + labelRect.Width + ItemPadding.TotalHorizontal;
        float height = paint.TextSize + Padding.TotalVertical;

        PixelSize ownSize = new(width, height);

        foreach (SizedLegendItem childItem in children)
        {
            width = Math.Max(width, Padding.Left + childItem.Size.WithChildren.Width);
            height += childItem.Size.WithChildren.Height;
        }

        return new LegendItemSize(ownSize, new(width, height));
    }

    private LegendItem[] GetVisibleItems(IEnumerable<LegendItem> items)
    {
        List<LegendItem> visibleItems = new();

        foreach (LegendItem item in items)
        {
            if (!string.IsNullOrWhiteSpace(item.Label))
            {
                visibleItems.Add(item);
            } else
            {
                visibleItems.AddRange(item.Children);
            }
        }

        return visibleItems.ToArray();
    }

    private SizedLegendItem[] GetSizedLegendItems(IEnumerable<LegendItem> items, SKPaint paint)
    {
        List<SizedLegendItem> sizedItems = new();

        foreach (var item in items)
        {
            var sizedChildren = GetSizedLegendItems(GetVisibleItems(item.Children), paint);
            sizedItems.Add(new(item, Measure(item, paint, sizedChildren), sizedChildren));
        }

        return sizedItems.ToArray();
    }
}
