namespace ScottPlot;

public interface IHasColorAxis
{
    Range GetRange();
    IColormap Colormap { get; }
}
