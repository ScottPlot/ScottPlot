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

    /// <summary>
    /// Enabling this allows multi-language text in the figure legend,
    /// but may slow down the render loop.
    /// </summary>
    public bool SetBestFontOnEachRender { get; set; } = false;
    // TODO: improve per-item font style customization.
    // This is hard now because plottables don't store legend item styling details.
    // Enabling proper support means adding a LegendItemStyle property to all plottables,
    // Then SetBestFonts() changes the font name based on the label contents.
    // Presently GetLegendItems() generates a new LegendItem every time it's called
    // in the render loop, so it cannot be used for long term storage of font information.

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

    private record LegendPack(SizedLegendItem[] SizedItems, PixelRect LegendRect, PixelRect LegendShadowRect, Pixel Offset);

    public void Render(RenderPack rp)
    {
        LegendPack? lp = GetLegendPack(rp.DataRect);
        if (lp is null)
            return;

        RenderLegend(rp.Canvas, lp);
    }

    public void AsSvg(Stream svgStream, int maxWidth = 0, int maxHeight = 0)
    {
        if (svgStream is null)
            throw new NullReferenceException($"invalid Stream");

        LegendPack? lp = GetLegendPack(maxWidth, maxHeight);
        if (lp is null)
            return;

        SKRect rect = new(0, 0, lp.LegendRect.Width, lp.LegendRect.Height);
        using SKCanvas canvas = SKSvgCanvas.Create(rect, svgStream);
        RenderLegend(canvas, lp);
    }

    public Image GetImage(int maxWidth = 0, int maxHeight = 0)
    {
        LegendPack lp = GetLegendPack(maxWidth, maxHeight);

        SKImageInfo info = new(
            width: (int)Math.Ceiling(lp.LegendRect.Width),
            height: (int)Math.Ceiling(lp.LegendRect.Height),
            colorType: SKColorType.Rgba8888,
            alphaType: SKAlphaType.Premul);

        using SKSurface surface = SKSurface.Create(info)
            ?? throw new NullReferenceException($"invalid SKImageInfo");

        RenderLegend(surface.Canvas, lp);

        return new Image(surface);
    }

    public string GetSvgXml()
    {
        LegendPack lp = GetLegendPack(0, 0);

        int width = (int)Math.Ceiling(lp.LegendRect.Width);
        int height = (int)Math.Ceiling(lp.LegendRect.Height);
        using SvgImage svg = new(width, height);

        RenderLegend(svg.Canvas, lp);

        return svg.GetXml();
    }

    private LegendPack GetLegendPack(int maxWidth, int maxHeight)
    {
        using SKPaint paint = new();
        LegendItem[] items = GetItems();
        SizedLegendItem[] sizedItems = GetSizedLegendItems(paint, items);
        PixelSize legendSize = GetLegendSize(sizedItems, maxWidth: maxWidth, maxHeight: maxHeight, withOffset: true);
        PixelRect legendRect = new(0, legendSize.Width, legendSize.Height, 0);
        Pixel offset = Pixel.Zero;
        return new LegendPack(sizedItems, legendRect, legendRect, offset);
    }

    private LegendPack? GetLegendPack(PixelRect dataRect)
    {
        if (!IsVisible)
            return null;

        LegendItem[] items = GetItems();
        if (items.Length == 0)
            return null;

        using SKPaint paint = new();
        SizedLegendItem[] sizedItems = GetSizedLegendItems(paint, items);
        if (sizedItems.Length == 0)
            return null;

        PixelSize legendSize = GetLegendSize(sizedItems,
            maxWidth: dataRect.Width - ShadowOffset * 2 - Margin.Horizontal,
            maxHeight: dataRect.Height - ShadowOffset * 2 - Margin.Vertical,
            withOffset: false);

        PixelRect legendRect = legendSize.AlignedInside(dataRect, Location, Margin);
        PixelRect legendShadowRect = legendRect.WithDelta(ShadowOffset, ShadowOffset, Location);
        Pixel offset = new(legendRect.Left + Padding.Left, legendRect.Top + Padding.Top);
        return new LegendPack(sizedItems, legendRect, legendShadowRect, offset);
    }

    public LegendItem[] GetItems()
    {
        LegendItem[] items = Plot.PlottableList
            .SelectMany(x => x.LegendItems)
            .Concat(ManualItems)
            .ToArray();

        if (SetBestFontOnEachRender)
        {
            foreach (LegendItem item in items)
            {
                if (item.Label is null)
                    continue;

                item.CustomFontStyle = Font.Clone();
                item.CustomFontStyle.SetBestFont(item.Label);
            }
        }

        return items;
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

    private SizedLegendItem[] GetSizedLegendItems(SKPaint paint, IEnumerable<LegendItem> allItems)
    {
        LegendItem[] items = GetAllLegendItems(allItems)
            .Where(x => x.IsVisible)
            .Where(x => !string.IsNullOrWhiteSpace(x.Label))
            .ToArray();

        if (items.Length == 0)
            return [];

        // measure all items to determine dimensions of the legend
        return GetSizedLegendItems(items, paint);
    }

    private SizedLegendItem[] GetSizedLegendItems(IEnumerable<LegendItem> items, SKPaint paint)
    {
        List<SizedLegendItem> sizedItems = new();

        foreach (LegendItem item in items)
        {
            if (item.CustomFontStyle is not null)
            {
                item.CustomFontStyle.ApplyToPaint(paint);
            }
            else
            {
                Font.ApplyToPaint(paint);
            }

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

    private void RenderLegend(SKCanvas canvas, LegendPack lp)
    {
        using SKPaint paint = new();

        // render the legend panel
        Drawing.FillRectangle(canvas, lp.LegendShadowRect, ShadowFill.Color);
        Drawing.FillRectangle(canvas, lp.LegendRect, BackgroundFill.Color);
        Drawing.DrawRectangle(canvas, lp.LegendRect, OutlineStyle.Color, OutlineStyle.Width);

        // render all items inside the legend
        float xOffset = lp.Offset.X;
        float prevHeight = 0;
        float yOffset = lp.Offset.Y;
        foreach (SizedLegendItem item in lp.SizedItems)
        {
            bool isHorizontal = Orientation == Orientation.Horizontal;
            bool isWider = (xOffset + item.Size.WithChildren.Width) > lp.LegendRect.Right;

            if (isHorizontal && AllowMultiline && isWider)
            {
                yOffset += prevHeight;
                xOffset = lp.Offset.X;
                prevHeight = 0;
            }

            if (item.Item.CustomFontStyle is not null)
            {
                item.Item.CustomFontStyle.ApplyToPaint(paint);
            }
            else
            {
                Font.ApplyToPaint(paint);
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
            {
                yOffset += item.Size.WithChildren.Height;
            }
        }
    }
}
