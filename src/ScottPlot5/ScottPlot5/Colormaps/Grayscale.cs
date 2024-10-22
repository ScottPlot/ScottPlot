namespace ScottPlot.Colormaps;

public class Grayscale
{
    public string Name => "Grayscale";

    public Color GetColor(double normalizedIntensity)
    {
        byte value = (byte)(255 * normalizedIntensity);
        return Color.Gray(value);
    }
}
