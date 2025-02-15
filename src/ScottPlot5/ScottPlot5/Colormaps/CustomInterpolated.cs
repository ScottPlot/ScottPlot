namespace ScottPlot.Colormaps;

/// <summary>
/// A custom palette which smoothly blends across a collection of colors using linear interpolation
/// </summary>
public class CustomInterpolated : IColormap
{
    public string Name => "Interpolated";

    private readonly Color[] Colors;

    public CustomInterpolated(Color[] colors)
    {
        if (colors.Length == 0)
        {
            // user supplied no colors so make it transparent
            Colors = [ScottPlot.Colors.Transparent, ScottPlot.Colors.Transparent];
        }
        else if (colors.Length == 1)
        {
            // user supplied a single color to place it twice so interpolation doesn't crash
            Colors = [colors[0], colors[0]];
        }
        else
        {
            Colors = colors;
        }
    }

    public Color GetColor(double position)
    {
        if (position <= 0)
            return Colors[0];

        if (position >= 1)
            return Colors[^1];

        int rangeCount = Colors.Length - 1;
        double rangeSize = 1.0 / rangeCount;

        int firstColorIndex = (int)(position / rangeSize);
        double positionInRange = (position - (rangeSize * firstColorIndex)) / rangeSize;

        Color color1 = Colors[firstColorIndex];
        Color color2 = Colors[firstColorIndex + 1];

        return color1.MixedWith(color2, positionInRange);
    }
}
