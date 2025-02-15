namespace ScottPlot.Colormaps;

public class GrayscaleReversed : IColormap
{
    public string Name => "Grayscale Reversed";
    public Color GetColor(double normalizedIntensity)
    {
        normalizedIntensity = NumericConversion.Clamp(normalizedIntensity, 0, 1);
        byte value = (byte)(255 - (byte)(255 * normalizedIntensity));
        return Color.Gray(value);
    }
}
