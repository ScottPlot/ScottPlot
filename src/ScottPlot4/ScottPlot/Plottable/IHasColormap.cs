namespace ScottPlot.Plottable;

public interface IHasColormap
{
    Drawing.Colormap Colormap { get; }
    double ColormapMin { get; }
    double ColormapMax { get; }
    bool ColormapMinIsClipped { get; }
    bool ColormapMaxIsClipped { get; }
}
