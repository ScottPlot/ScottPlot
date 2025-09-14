namespace ScottPlot.Colormaps;

internal class Inverted(IColormap original) : IColormap
{
    public string Name => $"{Original.Name} (inverted)";

    private readonly IColormap Original = original;

    public Color GetColor(double position)
    {
        return Original.GetColor(position).Inverted();
    }
}
