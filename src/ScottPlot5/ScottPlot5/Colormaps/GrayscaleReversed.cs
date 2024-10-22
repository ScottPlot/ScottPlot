namespace ScottPlot.Colormaps;

public class GrayscaleReversed
{
    public string Name => "Grayscale Reversed";
    public Color GetColor(double normalizedIntensity)
    {
        byte value = (byte)(255 - (byte)(255 * normalizedIntensity));
        return Color.Gray(value);
    }
}
