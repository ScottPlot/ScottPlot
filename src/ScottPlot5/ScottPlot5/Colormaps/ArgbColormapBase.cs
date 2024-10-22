namespace ScottPlot.Colormaps;

public abstract class ArgbColormapBase
{
    public abstract uint[] Argbs { get; }

    public Color GetColor(double normalizedIntensity)
    {
        var argb = Argbs[(int)(normalizedIntensity * (Argbs.Length - 1))];
        return Color.FromARGB(argb);
    }
}
