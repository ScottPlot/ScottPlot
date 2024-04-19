namespace ScottPlot.Legends;

public class Legend(Plot plot)
{
    public Plot Plot { get; } = plot;
    public bool IsVisible { get; set; } = false;

    /// <summary>
    /// Position of the legend relative to the data area
    /// </summary>
    public Alignment Alignment { get; set; } = Alignment.LowerRight;

    /// <summary>
    /// Position of the legend relative to the data area
    /// </summary>
    [Obsolete("use Alignment")]
    public Alignment Location { get => Alignment; set => Alignment = value; }

    /// <summary>
    /// Stack items in the legend according to this preferred orientation
    /// </summary>
    public Orientation Orientation { get; set; } = Orientation.Vertical;

    /// <summary>
    /// Distance from the edge of the data area to the edge of the legend
    /// </summary>
    public PixelPadding Margin { get; set; } = new(10);

    /// <summary>
    /// Distance between the legend frame and the items within it
    /// </summary>
    public PixelPadding Padding { get; set; } = new(5);

    /// <summary>
    /// Width of the symbol in a legend item
    /// </summary>
    public float SymbolWidth { get; set; } = 20;

    /// <summary>
    /// Padding between a symbol and label within a legend item
    /// </summary>
    public float SymbolPadding { get; set; } = 5;

    /// <summary>
    /// Space separating legend items
    /// </summary>
    public float InterItemPadding { get; set; } = 3;

    /// <summary>
    /// Items in this list will always be displayed in the legend
    /// </summary>
    public List<LegendItem> ManualItems { get; set; } = [];

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

    // TODO: implement IHasFill and IHasBackground
    public LineStyle OutlineStyle { get; set; } = new();
    public FillStyle BackgroundFill { get; set; } = new() { Color = Colors.White };
    public FillStyle ShadowFill { get; set; } = new() { Color = Colors.Black.WithOpacity(.2) };
    public PixelOffset ShadowOffset { get; set; } = new(3, 3);
    public Alignment ShadowAlignment { get; set; } = Alignment.LowerRight;

    public static bool ShowDebugLines { get; set; } = false;

    public ILegendLayoutEngine LayoutEngine { get; set; } = new LegendLayoutEngines.SingleColumn();

    public LegendItem[] GetItems() => Plot.PlottableList
            .Where(item => item.IsVisible)
            .SelectMany(x => x.LegendItems)
            .Concat(ManualItems)
            .ToArray();

    /// <summary>
    /// This is called by the render manager
    /// </summary>
    public void Render(RenderPack rp)
    {
        if (GetItems().Length == 0)
            return;

        LegendLayout layout = LayoutEngine.GetLayout(this);
        PixelRect standaloneLegendRect = layout.LegendRect.AlignedInside(rp.DataRect, Alignment, Margin);
        PixelOffset legendOffset = new(standaloneLegendRect.Left, standaloneLegendRect.Top);

        LegendLayout layout2 = new()
        {
            LegendItems = layout.LegendItems,
            LegendRect = layout.LegendRect.WithOffset(legendOffset),
            LabelRects = layout.LabelRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
            SymbolRects = layout.SymbolRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
        };

        Render(rp.Canvas, layout2);
    }

    /// <summary>
    /// Return an Image containing just the legend
    /// </summary>
    public Image GetImage()
    {
        LegendLayout lp = GetItems().Length > 0
            ? LayoutEngine.GetLayout(this)
            : LegendLayout.NoLegend;

        SKImageInfo info = new(
            width: Math.Max(1, (int)Math.Ceiling(lp.LegendRect.Width)),
            height: Math.Max(1, (int)Math.Ceiling(lp.LegendRect.Height)),
            colorType: SKColorType.Rgba8888,
            alphaType: SKAlphaType.Premul);

        using SKSurface surface = SKSurface.Create(info)
            ?? throw new NullReferenceException($"invalid SKImageInfo");

        Render(surface.Canvas, lp);

        return new Image(surface);
    }

    /// <summary>
    /// Return contents of a SVG image containing just the legend
    /// </summary>
    /// <returns></returns>
    public string GetSvgXml()
    {
        LegendLayout lp = GetItems().Length > 0
            ? LayoutEngine.GetLayout(this)
            : LegendLayout.NoLegend;

        int width = (int)Math.Ceiling(lp.LegendRect.Width);
        int height = (int)Math.Ceiling(lp.LegendRect.Height);
        using SvgImage svg = new(width, height);
        Render(svg.Canvas, lp);
        return svg.GetXml();
    }

    private void Render(SKCanvas canvas, LegendLayout lp)
    {
        using SKPaint paint = new();

        // render the legend panel
        PixelRect shadowRect = lp.LegendRect.WithOffset(ShadowOffset);
        Drawing.FillRectangle(canvas, shadowRect, paint, ShadowFill);
        Drawing.FillRectangle(canvas, lp.LegendRect, paint, BackgroundFill);
        Drawing.DrawRectangle(canvas, lp.LegendRect, paint, OutlineStyle);

        // render items inside the legend
        for (int i = 0; i < lp.LegendItems.Length; i++)
        {
            LegendItem item = lp.LegendItems[i];
            PixelRect labelRect = lp.LabelRects[i];
            PixelRect symbolRect = lp.SymbolRects[i];
            PixelRect symbolFillRect = symbolRect.Contract(0, symbolRect.Height * .2f);
            PixelRect symbolFillOutlineRect = symbolFillRect.Expand(1 - item.OutlineWidth);
            PixelLine symbolLine = new(symbolRect.RightCenter, symbolRect.LeftCenter);

            if (ShowDebugLines)
            {
                Drawing.DrawRectangle(canvas, symbolRect, Colors.Black.WithAlpha(.2), 1);
                Drawing.DrawRectangle(canvas, labelRect, Colors.Black.WithAlpha(.2), 1);
            }

            item.LabelStyle.Render(canvas, labelRect.LeftCenter, paint);
            item.LineStyle.Render(canvas, symbolLine, paint);
            item.FillStyle.Render(canvas, symbolFillRect, paint);
            item.OutlineStyle.Render(canvas, symbolFillOutlineRect, paint);
            item.MarkerStyle.Render(canvas, symbolRect.Center, paint);
            item.ArrowStyle.Render(canvas, symbolLine, paint);
        }
    }
}
