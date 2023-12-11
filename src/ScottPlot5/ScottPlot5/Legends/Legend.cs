namespace ScottPlot.Legends;

public class Legend
{
    public bool IsVisible { get; set; } = false;
    public Alignment Alignment { get; set; } = Alignment.LowerRight;
    public PixelPadding Margin { get; set; } = new PixelPadding(8);
    public PixelPadding Padding { get; set; } = new PixelPadding(3);
    public PixelPadding ItemPadding { get; set; } = new PixelPadding(3);

    public FontStyle Font { get; set; } = new();
    public LineStyle OutlineStyle { get; set; } = new();
    public FillStyle BackgroundFill { get; set; } = new() { Color = Colors.White };
    public FillStyle ShadowFill { get; set; } = new() { Color = Colors.Black.WithOpacity(.2) };

    public float ShadowOffset = 3;
    public Alignment ShadowAlignment = Alignment.LowerRight;

    private const float SymbolWidth = 20;
    private const float SymbolLabelSeparation = 5;
    public List<LegendItem> ManualItems { get; set; } = new();

    public void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        IEnumerable<LegendItem> allItems = rp.Plot.PlottableList.SelectMany(x => x.LegendItems).Concat(ManualItems);

        LegendItem[] items = GetAllLegendItems(allItems).Where(x => x.IsVisible).ToArray();
        if (!items.Any())
            return;

        // measure all items to determine dimensions of the legend
        using SKPaint paint = new();
        Font.ApplyToPaint(paint);

        SizedLegendItem[] sizedItems = GetSizedLegendItems(items, paint);

        float maxWidth = sizedItems.Select(x => x.Size.WithChildren.Width).Max();
        float totalheight = sizedItems.Select(x => x.Size.WithChildren.Height).Sum();

        PixelSize legendSize = new(maxWidth + Padding.Left + Padding.Right, totalheight + Padding.Top + Padding.Bottom);
        PixelRect legendRect = legendSize.AlignedInside(rp.DataRect, Alignment, Margin);
        PixelRect legendShadowRect = legendRect.WithDelta(ShadowOffset, ShadowOffset, Alignment);

        // render the legend panel
        Drawing.Fillectangle(rp.Canvas, legendShadowRect, ShadowFill.Color);
        Drawing.Fillectangle(rp.Canvas, legendRect, BackgroundFill.Color);
        Drawing.DrawRectangle(rp.Canvas, legendRect, OutlineStyle.Color, OutlineStyle.Width);

        // render all items inside the legend
        float yOffset = legendRect.Top + Padding.Top;
        for (int i = 0; i < sizedItems.Length; i++)
        {
            Common.RenderItem(
                canvas: rp.Canvas,
                paint: paint,
                sizedItem: sizedItems[i],
                x: legendRect.Left + Padding.Left,
                y: yOffset,
                symbolWidth: SymbolWidth,
                symbolPadRight: SymbolLabelSeparation,
                itemPadding: ItemPadding);

            yOffset += sizedItems[i].Size.WithChildren.Height;
        }
    }
    public Image GetImage(ScottPlot.Plot plot, int maxWidth = 0, int maxHeight = 0)
    {
        IEnumerable<LegendItem> allItems = plot.PlottableList.SelectMany(x => x.LegendItems).Concat(ManualItems);

        LegendItem[] items = GetAllLegendItems(allItems).Where(x => x.IsVisible).ToArray();
        if (!items.Any())
            throw new InvalidOperationException($"empty legend items");

        // measure all items to determine dimensions of the legend
        using SKPaint paint = new();
        Font.ApplyToPaint(paint);

        SizedLegendItem[] sizedItems = GetSizedLegendItems(items, paint);

        float maxItemWidth = sizedItems.Select(x => x.Size.WithChildren.Width).Max() + Padding.Left + Padding.Right + ShadowOffset;
        if (maxWidth > 0)
            maxItemWidth = Math.Min(maxItemWidth, maxWidth);
        float totalheight = sizedItems.Select(x => x.Size.WithChildren.Height).Sum() + Padding.Top + Padding.Bottom + ShadowOffset;
        if (maxHeight > 0)
            totalheight = Math.Min(totalheight, maxHeight);

        PixelSize legendSize = new(maxItemWidth, totalheight);
        PixelRect legendRect = new(new Pixel(0, 0), legendSize.Width - ShadowOffset, legendSize.Height - ShadowOffset);
        //FIXME: perhaps no shadow is the best option in this case?
        PixelRect legendShadowRect = legendRect.WithDelta(ShadowOffset, ShadowOffset, Alignment.LowerLeft);

        SKImageInfo info = new((int)Math.Ceiling(legendSize.Width), (int)Math.Ceiling(legendSize.Height), SKColorType.Rgba8888, SKAlphaType.Premul);
        using SKSurface surface = SKSurface.Create(info);
        if (surface is null)
            throw new NullReferenceException($"invalid SKImageInfo");

        // render the legend panel
        Drawing.Fillectangle(surface.Canvas, legendShadowRect, ShadowFill.Color);
        Drawing.Fillectangle(surface.Canvas, legendRect, BackgroundFill.Color);
        Drawing.DrawRectangle(surface.Canvas, legendRect, OutlineStyle.Color, OutlineStyle.Width);

        // render all items inside the legend
        float yOffset = legendRect.Top + Padding.Top;
        for (int i = 0; i < sizedItems.Length; i++)
        {
            Common.RenderItem(
                canvas: surface.Canvas,
                paint: paint,
                sizedItem: sizedItems[i],
                x: legendRect.Left + Padding.Left,
                y: yOffset,
                symbolWidth: SymbolWidth,
                symbolPadRight: SymbolLabelSeparation,
                itemPadding: ItemPadding);

            yOffset += sizedItems[i].Size.WithChildren.Height;
        }
        return new(surface.Snapshot());
    }

    /// <summary>
    /// Recursively walk through children and return a flat array with all legend items
    /// </summary>
    private LegendItem[] GetAllLegendItems(IEnumerable<LegendItem> items)
    {
        List<LegendItem> allItems = new();
        foreach (LegendItem item in items)
        {
            allItems.Add(item);
            allItems.AddRange(GetAllLegendItems(item.Children));
        }
        return allItems.ToArray();
    }

    private SizedLegendItem[] GetSizedLegendItems(IEnumerable<LegendItem> items, SKPaint paint)
    {
        List<SizedLegendItem> sizedItems = new();

        foreach (LegendItem item in items)
        {
            LegendItem[] visibleItems = GetAllLegendItems(item.Children).Where(x => x.IsVisible).ToArray();
            SizedLegendItem[] sizedChildren = GetSizedLegendItems(visibleItems, paint);
            LegendItemSize itemSize = Common.Measure(item, paint, sizedChildren, SymbolWidth, SymbolLabelSeparation, Padding, ItemPadding);
            SizedLegendItem sizedItem = new(item, itemSize, sizedChildren);
            sizedItems.Add(sizedItem);
        }

        return sizedItems.ToArray();
    }
}
