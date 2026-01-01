namespace ScottPlot;

public class Legend(Plot plot) : IPlottable, IHasOutline, IHasBackground, IHasShadow
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
    /// Height of the symbol in a legend item
    /// </summary>
    public float SymbolHeight { get; set; } = 10;

    /// <summary>
    /// Padding between a symbol and label within a legend item
    /// </summary>
    public float SymbolPadding { get; set; } = 5;

    /// <summary>
    /// Space separating legend items
    /// </summary>
    public PixelPadding InterItemPadding { get; set; } = new(10, 10, 6, 6);

    /// <summary>
    /// Items in this list will always be displayed in the legend
    /// </summary>
    public List<LegendItem> ManualItems { get; set; } = [];

    /// <summary>
    /// If enabled, items in horizontal oriented legends will not
    /// be aligned in columns but instead resized tightly to fit their contents
    /// </summary>
    public bool TightHorizontalWrapping { get; set; } = false;

    public bool ShowItemRectangles_DEBUG { get; set; } = false;

    /// <summary>
    /// Enabling this allows multi-language text in the figure legend,
    /// but may slow down the render loop.
    /// </summary>
    public bool SetBestFontOnEachRender { get; set; } = false;

    /// <summary>
    /// If set, this overrides the value in the LegendItem's FontStyle
    /// </summary>
    public float? FontSize { get; set; } = null;

    /// <summary>
    /// If set, this overrides the value in the LegendItem's FontStyle
    /// </summary>
    public string? FontName { get; set; } = null;

    /// <summary>
    /// If set, this overrides the value in the LegendItem's FontStyle
    /// </summary>
    public Color? FontColor { get; set; } = null;

    [Obsolete("Assign FontSize, FontName, or FontColor to control appearance of all legend items", true)]
    public FillStyle Font { get; set; } = new();

    [Obsolete("Multiline is now enabled by default.", true)]
    public bool AllowMultiline { get; set; }

    public ILegendLayout Layout { get; set; } = new LegendLayouts.Wrapping();
    public PixelSize LastRenderSize { get; private set; } = PixelSize.NaN;

    public LineStyle OutlineStyle { get; set; } = new() { Width = 1, Color = Colors.Black, };
    public float OutlineWidth { get => OutlineStyle.Width; set => OutlineStyle.Width = value; }
    public LinePattern OutlinePattern { get => OutlineStyle.Pattern; set => OutlineStyle.Pattern = value; }
    public Color OutlineColor { get => OutlineStyle.Color; set => OutlineStyle.Color = value; }


    [Obsolete("Assign BackgroundColor or interact with BackgroundFillStyle")]
    public FillStyle BackgroundFill { get => BackgroundFillStyle; set { } }
    public FillStyle BackgroundFillStyle { get; } = new() { Color = Colors.White };
    public Color BackgroundColor { get => BackgroundFillStyle.Color; set => BackgroundFillStyle.Color = value; }
    public Color BackgroundHatchColor { get => BackgroundFillStyle.HatchColor; set => BackgroundFillStyle.HatchColor = value; }
    public IHatch? BackgroundHatch { get => BackgroundFillStyle.Hatch; set => BackgroundFillStyle.Hatch = value; }

    [Obsolete("Assign ShadowColor or interact with ShadowFillStyle")]
    public FillStyle ShadowFill { get => ShadowFillStyle; set { } }
    public FillStyle ShadowFillStyle { get; } = new() { Color = Colors.Black.WithOpacity(.2) };
    public Color ShadowColor { get => ShadowFillStyle.Color; set => ShadowFillStyle.Color = value; }
    public PixelOffset ShadowOffset { get; set; } = new(3, 3);
    public Alignment ShadowAlignment { get; set; } = Alignment.LowerRight;

    public IAxes Axes { get; set; } = new Axes();
    public object? Tag { get; set; }
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    /// <summary>
    /// If supplied, always use this marker shape
    /// </summary>
    public MarkerShape? MarkerShapeOverride { get; set; } = null;

    public bool DisplayPlottableLegendItems { get; set; } = true;

    /// <summary>
    /// If enabled, the legend will include items from hidden plottables.
    /// They will be partially painted over using the background color to simulate semitransparency.
    /// </summary>
    public bool ShowItemsFromHiddenPlottables { get; set; } = false;

    /// <summary>
    /// This property controls how visible legend items are when their parent control's visibility is disabled.
    /// This property is only used when <see cref="ShowItemsFromHiddenPlottables"/> is enabled.
    /// </summary>
    public double HiddenItemOpacity { get; set; } = 0.25;

    public virtual LegendItem[] GetItems()
    {
        List<LegendItem> items = [];

        // manually added items with indexes come first
        var manualItemsWithIndexes = ManualItems.Where(x => x.Index.HasValue).OrderBy(x => x.Index!.Value);
        items.AddRange(manualItemsWithIndexes);

        // items from plottables come next
        if (DisplayPlottableLegendItems)
        {
            var plottableLegendItems = Plot.PlottableList
                        .Where(item => (ShowItemsFromHiddenPlottables || item.IsVisible))
                        .SelectMany(x => x.LegendItems)
                        .Where(x => !string.IsNullOrEmpty(x.LabelText));

            items.AddRange(plottableLegendItems);
        }

        // manually added items without indexes are last
        var manualItemsWithoutIndexes = ManualItems.Where(x => x.Index is null);
        items.AddRange(manualItemsWithoutIndexes);

        items = items.Where(x => x.IsVisible).ToList();

        if (SetBestFontOnEachRender)
        {
            foreach (LegendItem item in items)
            {
                item.LabelStyle.SetBestFont();
            }
        }

        foreach (LegendItem item in items)
        {
            if (FontSize is not null)
                item.LabelFontSize = FontSize.Value;
            if (FontName is not null)
                item.LabelFontName = FontName;
            if (FontColor is not null)
                item.LabelFontColor = FontColor.Value;
        }

        return items.ToArray();
    }

    public LegendLayout GetLayout(PixelSize size, Paint paint)
    {
        return Layout.GetLayout(this, GetItems(), size, paint);
    }

    /// <summary>
    /// Return an Image containing just the legend
    /// </summary>
    public Image GetImage()
    {
        using Paint paint = Paint.NewDisposablePaint();
        LegendItem[] items = GetItems();

        LegendLayout lp = items.Length > 0
            ? Layout.GetLayout(this, items, PixelSize.Infinity, paint)
            : LegendLayout.NoLegend;

        SKImageInfo info = new(
            width: Math.Max(1, (int)Math.Ceiling(lp.LegendRect.Width)),
            height: Math.Max(1, (int)Math.Ceiling(lp.LegendRect.Height)),
            colorType: SKColorType.Rgba8888,
            alphaType: SKAlphaType.Premul);

        using SKSurface surface = SKSurface.Create(info)
            ?? throw new NullReferenceException($"invalid SKImageInfo");

        RenderLayout(surface.Canvas, paint, lp);

        return new Image(surface);
    }

    /// <summary>
    /// Return contents of a SVG image containing just the legend
    /// </summary>
    /// <returns></returns>
    public string GetSvgXml()
    {
        LegendItem[] items = GetItems();

        using Paint paint = Paint.NewDisposablePaint();
        LegendLayout lp = items.Length > 0
            ? Layout.GetLayout(this, items, PixelSize.Infinity, paint)
            : LegendLayout.NoLegend;

        int width = (int)Math.Ceiling(lp.LegendRect.Width);
        int height = (int)Math.Ceiling(lp.LegendRect.Height);
        using SvgImage svg = new(width, height);
        RenderLayout(svg.Canvas, paint, lp);
        return svg.GetXml();
    }

    /// <summary>
    /// This is called automatically by the render manager
    /// </summary>
    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        LegendItem[] items = GetItems();
        if (items.Length == 0)
            return;

        PixelRect dataRectAfterMargin = rp.DataRect.Contract(Margin);
        LegendLayout tightLayout = Layout.GetLayout(this, items, dataRectAfterMargin.Size, rp.Paint);
        PixelRect standaloneLegendRect = tightLayout.LegendRect.AlignedInside(dataRectAfterMargin, Alignment);
        PixelOffset legendOffset = new(standaloneLegendRect.Left, standaloneLegendRect.Top);
        LegendLayout layout = new()
        {
            LegendItems = tightLayout.LegendItems,
            LegendRect = tightLayout.LegendRect.WithOffset(legendOffset),
            LabelRects = tightLayout.LabelRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
            SymbolRects = tightLayout.SymbolRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
        };

        RenderLayout(rp.Canvas, rp.Paint, layout);
    }

    public void Render(SKCanvas canvas, Paint paint, PixelRect rect, Alignment alignment)
    {
        LegendItem[] items = GetItems();
        if (items.Length == 0)
            return;

        LegendLayout tightLayout = Layout.GetLayout(this, items, rect.Size, paint);
        PixelRect standaloneLegendRect = tightLayout.LegendRect.AlignedInside(rect, alignment);
        PixelOffset legendOffset = new(standaloneLegendRect.Left, standaloneLegendRect.Top);
        LegendLayout layout = new()
        {
            LegendItems = tightLayout.LegendItems,
            LegendRect = tightLayout.LegendRect.WithOffset(legendOffset),
            LabelRects = tightLayout.LabelRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
            SymbolRects = tightLayout.SymbolRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
        };

        RenderLayout(canvas, paint, layout);
    }

    private void RenderLayout(SKCanvas canvas, Paint paint, LegendLayout layout)
    {
        LastRenderSize = layout.LegendRect.Size;

        // render the legend panel
        PixelRect shadowRect = layout.LegendRect.WithOffset(ShadowOffset);
        Drawing.FillRectangle(canvas, shadowRect, paint, ShadowFillStyle);
        Drawing.FillRectangle(canvas, layout.LegendRect, paint, BackgroundFillStyle);
        Drawing.DrawRectangle(canvas, layout.LegendRect, paint, OutlineStyle);

        CanvasState canvasState = new(canvas);
        canvasState.Save();

        PixelRect clipRect = layout.LegendRect.Contract(Padding).Expand(1f);
        canvasState.Clip(clipRect);

        // render items inside the legend
        for (int i = 0; i < layout.LegendItems.Length; i++)
        {
            LegendItem item = layout.LegendItems[i];
            PixelRect labelRect = layout.LabelRects[i];
            PixelRect symbolRect = layout.SymbolRects[i];
            PixelRect symbolFillRect = symbolRect.Contract(0, symbolRect.Height * .2f);
            PixelRect symbolFillOutlineRect = symbolFillRect.Expand(1 - item.OutlineWidth);
            PixelLine symbolLine = new(symbolRect.RightCenter, symbolRect.LeftCenter);

            if (ShowItemRectangles_DEBUG)
            {
                LineStyle ls = new() { Color = Colors.Magenta.WithAlpha(.2) };
                Drawing.DrawRectangle(canvas, symbolRect, paint, ls);
                Drawing.DrawRectangle(canvas, labelRect, paint, ls);
            }

            // draw symbol components
            if (MarkerShapeOverride.HasValue)
            {
                item.MarkerShape = MarkerShapeOverride.Value;

                if (item.MarkerColor == Colors.Transparent)
                    item.MarkerColor = Colors.Black;

                // NOTE: Some plottables like signal plots have dynamically sized markers that can get very small
                item.MarkerSize = Math.Max(item.LabelFontSize, item.MarkerSize);

                // when custom marker shape is in use, only render that and the label
                item.LabelStyle.Render(canvas, labelRect.LeftCenter, paint, true);
                item.MarkerStyle.Render(canvas, symbolRect.Center, paint);
            }
            else
            {
                item.LabelStyle.Render(canvas, labelRect.LeftCenter, paint, true);
                item.LineStyle.Render(canvas, symbolLine, paint);
                item.FillStyle.Render(canvas, symbolFillRect, paint);
                item.OutlineStyle.Render(canvas, symbolFillOutlineRect, paint);
                item.MarkerStyle.Render(canvas, symbolRect.Center, paint);
                item.ArrowStyle.Render(canvas, symbolLine, paint);
            }

            // Partially hide legend item to reflect that plottable is invisible
            if (item.Plottable is not null && !item.Plottable.IsVisible)
            {
                PixelRect itemRect = new(symbolRect.TopLeft, labelRect.BottomRight);
                FillStyle fs = new() { Color = BackgroundColor.WithAlpha(1.0 - HiddenItemOpacity) };
                Drawing.FillRectangle(canvas, itemRect, paint, fs);
            }
        }

        canvasState.Restore();
    }
}
