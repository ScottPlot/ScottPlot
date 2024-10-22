namespace ScottPlot.Colormaps;

/// <summary>
/// A palette of colors with "hard edges" (no interpolation between colors)
/// </summary>
public class Custom(Color[] colors, string name = "custom")
{
    public string Name { get; } = name;

    private readonly Color[] Colors = colors;

    public Color GetColor(double position)
    {
        position = NumericConversion.Clamp(position, 0, 1);
        int index = (int)((Colors.Length - 1) * position);
        return Colors[index];
    }
}
