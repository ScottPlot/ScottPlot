using SkiaSharp;
using System.Net.Security;

namespace ScottPlot.Legends;

public class Legend(Plot plot)
{
    public bool IsVisible { get; set; } = false;
    public Alignment Location { get; set; } = Alignment.LowerRight;
    public PixelPadding Margin { get; set; } = new PixelPadding(8);
    public PixelPadding Padding { get; set; } = new PixelPadding(3);
    public PixelPadding ItemPadding { get; set; } = new PixelPadding(3);

    public Orientation Orientation { get; set; } = Orientation.Vertical;
    public bool AllowMultiline { get; set; } = false;

    public FontStyle Font { get; set; } = new();
    public LineStyle OutlineStyle { get; set; } = new();
    public FillStyle BackgroundFill { get; set; } = new() { Color = Colors.White };
    public FillStyle ShadowFill { get; set; } = new() { Color = Colors.Black.WithOpacity(.2) };

    public float ShadowOffset = 3;
    public Alignment ShadowAlignment = Alignment.LowerRight;

    private const float SymbolWidth = 20;
    private const float SymbolLabelSeparation = 5;
    public List<LegendItem> ManualItems { get; set; } = new();

    private readonly Plot Plot = plot;

    public void Show() => IsVisible = true;
    public void Hide() => IsVisible = false;

    public void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        IEnumerable<LegendItem> allItems = GetItems();

        LegendItem[] items = GetAllLegendItems(allItems).Where(x => x.IsVisible).ToArray();
        if (!items.Any())
            return;

        // measure all items to determine dimensions of the legend
        using SKPaint paint = new();
        SizedLegendItem[] sizedItems = GetSizedLegendItems(rp.Plot, paint);

        if (sizedItems?.Any() != true)
            return;

        var legendSize = GetLegendSize(sizedItems,
            maxWidth: rp.DataRect.Width - ShadowOffset * 2 - Margin.Horizontal,
            maxHeight: rp.DataRect.Height - ShadowOffset * 2 - Margin.Vertical,
            withOffset: false);

        PixelRect legendRect = legendSize.AlignedInside(rp.DataRect, Location, Margin);
        PixelRect legendShadowRect = legendRect.WithDelta(ShadowOffset, ShadowOffset, Location);
        Pixel offset = new(legendRect.Left + Padding.Left, legendRect.Top + Padding.Top);

        // render the legend panel
        RenderLegend(sizedItems, rp.Canvas, paint, offset, legendRect, legendShadowRect);
    }

    public void AsSvg(ScottPlot.Plot plot, Stream svgStream, int maxWidth = 0, int maxHeight = 0)
    {
        if (svgStream is null)
            throw new NullReferenceException($"invalid Stream");

        // measure all items to determine dimensions of the legend
        using SKPaint paint = new();
        SizedLegendItem[] sizedItems = GetSizedLegendItems(plot, paint);

        if (sizedItems.Any() != true)
            throw new InvalidOperationException($"empty legend items");

        var legendSize = GetLegendSize(sizedItems, maxWidth: maxWidth, maxHeight: maxHeight, withOffset: true);

        PixelRect legendRect = new(new Pixel(ShadowOffset, ShadowOffset), legendSize.Width - 2 * ShadowOffset, legendSize.Height - 2 * ShadowOffset);
        PixelRect legendShadowRect = legendRect.WithDelta(ShadowOffset, ShadowOffset, Location);
        Pixel offset = new(legendRect.Left + Padding.Left + ShadowOffset, legendRect.Top + Padding.Top + ShadowOffset);

        using SKCanvas canvas = SKSvgCanvas.Create(new SKRect(0, 0, legendSize.Width, legendSize.Height), svgStream);
        RenderLegend(sizedItems, canvas, paint, offset, legendRect, legendShadowRect);
    }

    public Image GetImage(Plot plot, int maxWidth = 0, int maxHeight = 0)
    {
        // measure all items to determine dimensions of the legend
        using SKPaint paint = new();
        SizedLegendItem[] sizedItems = GetSizedLegendItems(plot, paint);

        if (sizedItems.Any() != true)
            throw new InvalidOperationException($"empty legend items");

        var legendSize = GetLegendSize(sizedItems, maxWidth: maxWidth, maxHeight: maxHeight, withOffset: true);

        PixelRect legendRect = new(new Pixel(ShadowOffset, ShadowOffset), legendSize.Width - 2 * ShadowOffset, legendSize.Height - 2 * ShadowOffset);
        PixelRect legendShadowRect = legendRect.WithDelta(ShadowOffset, ShadowOffset, Location);
        Pixel offset = new(legendRect.Left + Padding.Left + ShadowOffset, legendRect.Top + Padding.Top + ShadowOffset);

        SKImageInfo info = new(
            width: (int)Math.Ceiling(legendSize.Width),
            height: (int)Math.Ceiling(legendSize.Height),
            colorType: SKColorType.Rgba8888,
            alphaType: SKAlphaType.Premul);

        using SKSurface surface = SKSurface.Create(info) ?? throw new NullReferenceException($"invalid SKImageInfo");

        RenderLegend(sizedItems, surface.Canvas, paint, offset, legendRect, legendShadowRect);

        SKImage skimg = surface.Snapshot();

        return new Image(skimg);
    }

    public string GetSvgXml(Plot plot)
    {
        using SKPaint paint = new();
        SizedLegendItem[] sizedItems = GetSizedLegendItems(plot, paint);

        var legendSize = GetLegendSize(sizedItems, withOffset: true);

        int width = (int)Math.Ceiling(legendSize.Width);
        int height = (int)Math.Ceiling(legendSize.Height);

        PixelRect legendRect = new(0, width, 0, height);
        PixelRect legendShadowRect = legendRect.WithDelta(ShadowOffset, ShadowOffset, Location);
        Pixel offset = new(legendRect.Left + Padding.Left, legendRect.Top + Padding.Top);

        using SvgImage svg = new(width, height);
        RenderLegend(sizedItems, svg.Canvas, paint, offset, legendRect, legendShadowRect);
        string svgXml = svg.GetXml();
        return svgXml;
    }

    public IEnumerable<LegendItem> GetItems()
    {
        return Plot.PlottableList.SelectMany(x => x.LegendItems).Concat(ManualItems);
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

    private PixelSize GetLegendSize(SizedLegendItem[] sizedItems, float maxWidth = 0, float maxHeight = 0, bool withOffset = false)
    {
        float legendWidth, legendHeight;

        if (Orientation == Orientation.Vertical)
        {
            legendWidth = sizedItems.Select(x => x.Size.WithChildren.Width).Max();
            legendHeight = sizedItems.Select(x => x.Size.WithChildren.Height).Sum();
        }
        else if (AllowMultiline && maxWidth > 0)
        {
            float lw = 0;
            float lh = 0;
            legendWidth = legendHeight = 0;
            foreach (var item in sizedItems)
            {
                if (item.Size.WithChildren.Width + legendWidth > maxWidth)
                {
                    lw = Math.Max(lw, legendWidth);
                    legendWidth = 0;
                    legendHeight += lh;
                    lh = 0;
                }
                legendWidth += item.Size.WithChildren.Width;
                lh = Math.Max(lh, item.Size.WithChildren.Height);
            }
            legendHeight += lh;
            legendWidth = Math.Max(lw, legendWidth);
        }
        else
        {
            legendWidth = sizedItems.Select(x => x.Size.WithChildren.Width).Sum();
            legendHeight = sizedItems.Select(x => x.Size.WithChildren.Height).Max();
        }

        legendWidth += Padding.Left + Padding.Right + (withOffset ? 2 * ShadowOffset : 0);
        legendHeight += Padding.Top + Padding.Bottom + (withOffset ? 2 * ShadowOffset : 0);

        if (maxWidth > 0)
            legendWidth = Math.Min(legendWidth, maxWidth);
        if (maxHeight > 0)
            legendHeight = Math.Min(legendHeight, maxHeight);

        return new(legendWidth, legendHeight);
    }

    private void RenderLegend(SizedLegendItem[] sizedItems, SKCanvas canvas, SKPaint paint, Pixel offset, PixelRect legendRect, PixelRect legendShadowRect)
    {
        // render the legend panel
        Drawing.Fillectangle(canvas, legendShadowRect, ShadowFill.Color);
        Drawing.Fillectangle(canvas, legendRect, BackgroundFill.Color);
        Drawing.DrawRectangle(canvas, legendRect, OutlineStyle.Color, OutlineStyle.Width);

        // render all items inside the legend
        float xOffset = offset.X;
        float prevHeight = 0;
        float yOffset = offset.Y;
        foreach (SizedLegendItem item in sizedItems)
        {
            if (Orientation == Orientation.Horizontal && AllowMultiline && (xOffset + item.Size.WithChildren.Width) > legendRect.Right)
            {
                yOffset += prevHeight;
                xOffset = offset.X;
                prevHeight = 0;
            }
            Common.RenderItem(
                canvas: canvas,
                paint: paint,
                sizedItem: item,
                x: xOffset,
                y: yOffset,
                symbolWidth: SymbolWidth,
                symbolPadRight: SymbolLabelSeparation,
                itemPadding: ItemPadding);

            if (Orientation == Orientation.Horizontal)
            {
                xOffset += item.Size.WithChildren.Width;
                prevHeight = Math.Max(prevHeight, item.Size.WithChildren.Height);
            }
            else
                yOffset += item.Size.WithChildren.Height;
        }
    }
}
