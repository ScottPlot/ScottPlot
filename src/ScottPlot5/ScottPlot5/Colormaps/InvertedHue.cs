namespace ScottPlot.Colormaps;

internal class InvertedHue(IColormap original) : IColormap
{
    public string Name => $"{Original.Name} (inverted hue)";

    private readonly IColormap Original = original;

    public Color GetColor(double position)
    {
        return Original.GetColor(position).InvertedHue();
    }
}
