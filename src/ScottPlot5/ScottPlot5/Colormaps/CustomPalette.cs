namespace ScottPlot.Colormaps;

/// <summary>
/// A palette of colors with "hard edges" (no interpolation between colors)
/// </summary>
public class CustomPalette : IColormap
{
    public string Name { get; }

    private readonly Color[] Colors;

    public CustomPalette(Color[] colors, string name = "custom")
    {
        Colors = colors;
        Name = name;
    }

    public Color GetColor(double position)
    {
        position = NumericConversion.Clamp(position, 0, 1);
        int index = (int)((Colors.Length - 1) * position);
        return Colors[index];
    }
}
