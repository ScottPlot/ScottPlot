namespace ScottPlot.Colormaps;

public class Grayscale : IColormap
{
    public string Name => "Grayscale";

    public Color GetColor(double normalizedIntensity)
    {
        normalizedIntensity = NumericConversion.Clamp(normalizedIntensity, 0, 1);
        byte value = (byte)(255 * normalizedIntensity);
        return Color.Gray(value);
    }
}
