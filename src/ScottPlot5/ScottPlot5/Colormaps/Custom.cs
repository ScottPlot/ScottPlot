namespace ScottPlot.Colormaps;

/// <summary>
/// A custom colormap created from a collection of colors
/// </summary>
public class Custom(Color[] colors, bool smooth = true) : IColormap
{
    readonly IColormap Cmap = smooth ? new CustomInterpolated(colors) : new CustomPalette(colors);
    public string Name => Cmap.Name;
    public Color GetColor(double position) => Cmap.GetColor(position);
}
