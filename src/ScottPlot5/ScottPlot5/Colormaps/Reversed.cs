namespace ScottPlot.Colormaps;

public class Reversed(IColormap cmap) : IColormap
{
    private readonly IColormap Cmap = cmap;
    public string Name => $"{Cmap.Name} (Reversed)";
    public Color GetColor(double position) => Cmap.GetColor(1.0 - position);
}
