namespace ScottPlot;

public readonly struct LegendLayout
{
    required public LegendItem[] LegendItems { get; init; }
    required public PixelRect LegendRect { get; init; }
    required public PixelRect[] LabelRects { get; init; }
    required public PixelRect[] SymbolRects { get; init; }

    /// <summary>
    /// Rectangle for the legend title (if present)
    /// </summary>
    public PixelRect? TitleRect { get; init; }

    /// <summary>
    /// True if this layout includes a title
    /// </summary>
    public bool HasTitle => TitleRect.HasValue;

    public static LegendLayout NoLegend => new()
    {
        LegendItems = [],
        LegendRect = PixelRect.NaN,
        LabelRects = [],
        SymbolRects = [],
        TitleRect = null,
    };
}
