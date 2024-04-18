namespace ScottPlot.Legends;

public class Legend(Plot plot)
{
    public Plot Plot { get; } = plot;
    public bool IsVisible { get; set; } = false;
    public Alignment Location { get; set; } = Alignment.LowerRight;
    public PixelPadding Margin { get; set; } = new(8);
    public PixelPadding Padding { get; set; } = new(3);
    public PixelPadding ItemPadding { get; set; } = new(3);
    public Orientation Orientation { get; set; } = Orientation.Vertical;
    public float SymbolWidth { get; } = 20;
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
    public float ShadowOffset { get; set; } = 3;
    public Alignment ShadowAlignment { get; set; } = Alignment.LowerRight;

    public void Show() => IsVisible = true;
    public void Hide() => IsVisible = false;

    // Logic for the following methods got OUT OF CONTROL
    // so they have been refactored into distinct static classes
    public LegendItem[] GetItems() => GetItemLogic.GetItems(this);
    public void Render(RenderPack rp) => Rendering.Render(this, rp);
    public string GetSvgXml() => Rendering.GetSvgXml(this);
    public Image GetImage(int maxWidth = 0, int maxHeight = 0) => Rendering.GetImage(this, maxWidth, maxHeight);
}
