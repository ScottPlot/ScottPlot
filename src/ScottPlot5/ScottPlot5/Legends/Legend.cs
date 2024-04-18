namespace ScottPlot.Legends;

public class Legend(Plot plot)
{
    public Plot Plot { get; } = plot;
    public bool IsVisible { get; set; } = false;

    /// <summary>
    /// Position of the legend relative to the data area
    /// </summary>
    public Alignment Location { get; set; } = Alignment.LowerRight; // TODO: name Alignment

    /// <summary>
    /// Distance from the edge of the data area to the edge of the legend
    /// </summary>
    public PixelPadding Margin { get; set; } = new(8);

    /// <summary>
    /// Distance between the legend frame and the items within it
    /// </summary>
    public PixelPadding Padding { get; set; } = new(5);

    /// <summary>
    /// Padding between a symbol and label within a legend item
    /// </summary>
    public float SymbolPadding { get; set; } = 5;

    /// <summary>
    /// Width of the symbol in a legend item
    /// </summary>
    public float SymbolWidth { get; set; } = 20;

    /// <summary>
    /// Vertical spacing separating legend items
    /// </summary>
    public float VerticalSpacing { get; set; } = 3;

    public Orientation Orientation { get; set; } = Orientation.Vertical;

    public float SymbolLabelSeparation { get; } = 5;

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

    public void Show() => IsVisible = true;
    public void Hide() => IsVisible = false;

    public LegendItem[] GetItems() => Plot.PlottableList
            .Where(item => item.IsVisible)
            .SelectMany(x => x.LegendItems)
            .Concat(ManualItems)
            .ToArray();

    public void Render(RenderPack rp) => Rendering.Render(this, rp);
    public string GetSvgXml() => Rendering.GetSvgXml(this);
    public Image GetImage(int maxWidth = 0, int maxHeight = 0) => Rendering.GetImage(this, maxWidth, maxHeight);
}
