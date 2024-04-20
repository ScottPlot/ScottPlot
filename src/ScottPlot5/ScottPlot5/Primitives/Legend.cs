namespace ScottPlot;

public class Legend(Plot plot) : IPlottable
{
    public Plot Plot { get; } = plot;

    public bool IsVisible { get; set; } = true;

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
    public PixelPadding Margin { get; set; } = new(7, 7);

    /// <summary>
    /// Distance between the legend frame and the items within it
    /// </summary>
    public PixelPadding Padding { get; set; } = new(10, 5);

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
    public PixelPadding InterItemPadding { get; set; } = new(10, 10, 3, 3);

    /// <summary>
    /// Items in this list will always be displayed in the legend
    /// </summary>
    public List<LegendItem> ManualItems { get; set; } = [];

    /// <summary>
    /// Enabling this allows multi-language text in the figure legend,
    /// but may slow down the render loop.
    /// </summary>
    public bool SetBestFontOnEachRender { get; set; } = false;

    public FontStyle Font { get; set; } = new();

    // TODO: implement IHasFill and IHasBackground
    public LineStyle OutlineStyle { get; set; } = new();
    public FillStyle BackgroundFill { get; set; } = new() { Color = Colors.White };
    public FillStyle ShadowFill { get; set; } = new() { Color = Colors.Black.WithOpacity(.2) };
    public PixelOffset ShadowOffset { get; set; } = new(3, 3);
    public Alignment ShadowAlignment { get; set; } = Alignment.LowerRight;

    public ILegendLayout LayoutEngine { get; set; } = new LegendLayouts.Wrapping();

    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public LegendItem[] GetItems()
    {
        LegendItem[] items = Plot.PlottableList
            .Where(item => item.IsVisible)
            .SelectMany(x => x.LegendItems)
            .Where(x => !string.IsNullOrEmpty(x.LabelText))
            .Concat(ManualItems)
            .ToArray();

        if (SetBestFontOnEachRender)
        {
            foreach (LegendItem item in items)
            {
                item.LabelStyle.SetBestFont();
            }
        }

        return items;
    }

    /// <summary>
    /// Return an Image containing just the legend
    /// </summary>
    public Image GetImage()
    {
        LegendItem[] items = GetItems();

        LegendLayout lp = items.Length > 0
            ? LayoutEngine.GetLayout(this, items, PixelSize.Infinity)
            : LegendLayout.NoLegend;

        SKImageInfo info = new(
            width: Math.Max(1, (int)Math.Ceiling(lp.LegendRect.Width)),
            height: Math.Max(1, (int)Math.Ceiling(lp.LegendRect.Height)),
            colorType: SKColorType.Rgba8888,
            alphaType: SKAlphaType.Premul);

        using SKSurface surface = SKSurface.Create(info)
            ?? throw new NullReferenceException($"invalid SKImageInfo");

        RenderLayout(surface.Canvas, lp);

        return new Image(surface);
    }

    /// <summary>
    /// Return contents of a SVG image containing just the legend
    /// </summary>
    /// <returns></returns>
    public string GetSvgXml()
    {
        LegendItem[] items = GetItems();

        LegendLayout lp = items.Length > 0
            ? LayoutEngine.GetLayout(this, items, PixelSize.Infinity)
            : LegendLayout.NoLegend;

        int width = (int)Math.Ceiling(lp.LegendRect.Width);
        int height = (int)Math.Ceiling(lp.LegendRect.Height);
        using SvgImage svg = new(width, height);
        RenderLayout(svg.Canvas, lp);
        return svg.GetXml();
    }

    /// <summary>
    /// This is called by the render manager
    /// </summary>
    public void Render(RenderPack rp)
    {
        LegendItem[] items = GetItems();
        if (items.Length == 0)
            return;

        PixelRect dataRectAfterMargin = rp.DataRect.Contract(Margin);
        LegendLayout tightLayout = LayoutEngine.GetLayout(this, items, dataRectAfterMargin.Size);
        PixelRect standaloneLegendRect = tightLayout.LegendRect.AlignedInside(dataRectAfterMargin, Alignment);
        PixelOffset legendOffset = new(standaloneLegendRect.Left, standaloneLegendRect.Top);

        LegendLayout layout = new()
        {
            LegendItems = tightLayout.LegendItems,
            LegendRect = tightLayout.LegendRect.WithOffset(legendOffset),
            LabelRects = tightLayout.LabelRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
            SymbolRects = tightLayout.SymbolRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
        };

        RenderLayout(rp.Canvas, layout);
    }

    private void RenderLayout(SKCanvas canvas, LegendLayout layout)
    {
        using SKPaint paint = new();

        // render the legend panel
        PixelRect shadowRect = layout.LegendRect.WithOffset(ShadowOffset);
        Drawing.FillRectangle(canvas, shadowRect, paint, ShadowFill);
        Drawing.FillRectangle(canvas, layout.LegendRect, paint, BackgroundFill);
        Drawing.DrawRectangle(canvas, layout.LegendRect, paint, OutlineStyle);

        // render items inside the legend
        for (int i = 0; i < layout.LegendItems.Length; i++)
        {
            LegendItem item = layout.LegendItems[i];
            PixelRect labelRect = layout.LabelRects[i];
            PixelRect symbolRect = layout.SymbolRects[i];
            PixelRect symbolFillRect = symbolRect.Contract(0, symbolRect.Height * .2f);
            PixelRect symbolFillOutlineRect = symbolFillRect.Expand(1 - item.OutlineWidth);
            PixelLine symbolLine = new(symbolRect.RightCenter, symbolRect.LeftCenter);

            item.LabelStyle.Render(canvas, labelRect.LeftCenter, paint);
            item.LineStyle.Render(canvas, symbolLine, paint);
            item.FillStyle.Render(canvas, symbolFillRect, paint);
            item.OutlineStyle.Render(canvas, symbolFillOutlineRect, paint);
            item.MarkerStyle.Render(canvas, symbolRect.Center, paint);
            item.ArrowStyle.Render(canvas, symbolLine, paint);
        }
    }
}
