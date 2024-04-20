namespace ScottPlot;

public readonly struct LegendLayout
{
    required public LegendItem[] LegendItems { get; init; }
    required public PixelRect LegendRect { get; init; }
    required public PixelRect[] LabelRects { get; init; }
    required public PixelRect[] SymbolRects { get; init; }

    public static LegendLayout NoLegend => new()
    {
        LegendItems = [],
        LegendRect = PixelRect.NaN,
        LabelRects = [],
        SymbolRects = [],
    };
}
