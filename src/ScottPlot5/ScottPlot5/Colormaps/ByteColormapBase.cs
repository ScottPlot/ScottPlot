namespace ScottPlot.Colormaps;

public abstract class ByteColormapBase
{
    public abstract (byte r, byte g, byte b)[] Rgbs { get; }

    public Color GetColor(double normalizedIntensity)
    {
        var rgb = Rgbs?[(int)(normalizedIntensity * (Rgbs.Length - 1))] ?? (0, 0, 0);
        return new(rgb.r, rgb.g, rgb.b);
    }
}
