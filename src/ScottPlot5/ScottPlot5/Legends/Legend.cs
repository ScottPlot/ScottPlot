using SkiaSharp;
using System.Net.Security;

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

        // measure all items to determine dimensions of the legend
        using SKPaint paint = new();
        SizedLegendItem[] sizedItems = GetSizedLegendItems(rp.Plot, paint);

        if (sizedItems?.Any() != true)
            return;

        float maxWidth = sizedItems.Select(x => x.Size.WithChildren.Width).Max();
        float totalheight = sizedItems.Select(x => x.Size.WithChildren.Height).Sum();

        PixelSize legendSize = new(maxWidth + Padding.Left + Padding.Right, totalheight + Padding.Top + Padding.Bottom);
        PixelRect legendRect = legendSize.AlignedInside(rp.DataRect, Alignment, Margin);
        PixelRect legendShadowRect = legendRect.WithDelta(ShadowOffset, ShadowOffset, Alignment);
        Pixel offset = new(legendRect.Left + Padding.Left, legendRect.Top + Padding.Top);

        // render the legend panel
        RenderLegend(sizedItems, rp.Canvas, paint, offset, legendRect, legendShadowRect);
    }
    public void AsSvg(ScottPlot.Plot plot, Stream svgStream, int maxWidth = 0, int maxHeight = 0)
    {
        if (svgStream is null)
            throw new NullReferenceException($"invalid Stream");

        var canvas = RenderToObject(plot, svgStream:svgStream, maxWidth: maxWidth, maxHeight: maxHeight) as SKCanvas;
        canvas?.Dispose();
    }
    public SKImage? GetImage(ScottPlot.Plot plot, int maxWidth = 0, int maxHeight = 0)
    {
        var surface = RenderToObject(plot, maxWidth: maxWidth, maxHeight: maxHeight) as SKSurface;
        var image = surface?.Snapshot();
        surface?.Dispose();
        return image;
    }
    private SKObject RenderToObject(ScottPlot.Plot plot, Stream svgStream = null, int maxWidth = 0, int maxHeight = 0)
    {
        // measure all items to determine dimensions of the legend
        using SKPaint paint = new();
        SizedLegendItem[] sizedItems = GetSizedLegendItems(plot, paint);

        if (sizedItems.Any() != true)
            throw new InvalidOperationException($"empty legend items");

        float maxItemWidth = sizedItems.Select(x => x.Size.WithChildren.Width).Max() + Padding.Left + Padding.Right + 2 * ShadowOffset;
        if (maxWidth > 0)
            maxItemWidth = Math.Min(maxItemWidth, maxWidth);
        float totalheight = sizedItems.Select(x => x.Size.WithChildren.Height).Sum() + Padding.Top + Padding.Bottom + 2 * ShadowOffset;
        if (maxHeight > 0)
            totalheight = Math.Min(totalheight, maxHeight);

        PixelSize legendSize = new(maxItemWidth, totalheight);
        PixelRect legendRect = new(new Pixel(ShadowOffset, ShadowOffset), legendSize.Width - 2 * ShadowOffset, legendSize.Height - 2 * ShadowOffset);
        PixelRect legendShadowRect = legendRect.WithDelta(ShadowOffset, ShadowOffset, Alignment);
        Pixel offset = new(legendRect.Left + Padding.Left + ShadowOffset, legendRect.Top + Padding.Top + ShadowOffset);

        if (svgStream is null)
        {
            SKImageInfo info = new((int)Math.Ceiling(legendSize.Width), (int)Math.Ceiling(legendSize.Height), SKColorType.Rgba8888, SKAlphaType.Premul);
            var surface = SKSurface.Create(info);
            if (surface is null)
                throw new NullReferenceException($"invalid SKImageInfo");

            RenderLegend(sizedItems, surface.Canvas, paint, offset, legendRect, legendShadowRect);
            return surface;
        }
        SKCanvas canvas = SKSvgCanvas.Create(new SKRect(0, 0, legendSize.Width, legendSize.Height), svgStream);
        RenderLegend(sizedItems, canvas, paint, offset, legendRect, legendShadowRect);
        return canvas;
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
    private SizedLegendItem[] GetSizedLegendItems(ScottPlot.Plot plot, SKPaint paint)
    {
        IEnumerable<LegendItem> allItems = plot.PlottableList.Where(x => x.IsVisible).SelectMany(x => x.LegendItems).Concat(ManualItems);

        LegendItem[] items = GetAllLegendItems(allItems).Where(x => x.IsVisible).ToArray();
        if (!items.Any())
            return Array.Empty<SizedLegendItem>();

        // measure all items to determine dimensions of the legend
        Font.ApplyToPaint(paint);
        return GetSizedLegendItems(items, paint);
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
    private void RenderLegend(SizedLegendItem[] sizedItems, SKCanvas canvas, SKPaint paint, Pixel offset, PixelRect legendRect, PixelRect legendShadowRect)
    {
        // render the legend panel
        Drawing.Fillectangle(canvas, legendShadowRect, ShadowFill.Color);
        Drawing.Fillectangle(canvas, legendRect, BackgroundFill.Color);
        Drawing.DrawRectangle(canvas, legendRect, OutlineStyle.Color, OutlineStyle.Width);

        // render all items inside the legend
        float yOffset = offset.Y;
        foreach (var item in sizedItems)
        {
            Common.RenderItem(
                canvas: canvas,
                paint: paint,
                sizedItem: item,
                x: offset.X,
                y: yOffset,
                symbolWidth: SymbolWidth,
                symbolPadRight: SymbolLabelSeparation,
                itemPadding: ItemPadding);

            yOffset += item.Size.WithChildren.Height;
        }
    }
}
