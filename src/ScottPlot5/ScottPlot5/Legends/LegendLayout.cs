namespace ScottPlot.Legends;

public readonly struct LegendLayout
{
    required public LegendItem[] LegendItems { get; init; }
    required public PixelRect LegendRect { get; init; }
    required public PixelRect[] LabelRects { get; init; }
    required public PixelRect[] SymbolRects { get; init; }
}
