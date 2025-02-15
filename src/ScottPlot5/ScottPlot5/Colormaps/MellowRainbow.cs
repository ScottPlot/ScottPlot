namespace ScottPlot.Colormaps;

/// <summary>
/// A rainbow colormap inspired by Jet and Turbo but adapted by Scott Harden to have 
/// less dark edges (red or purple) and a more mellow center (darker yellow/green) 
/// for more even brightness perception when displayed using thin lines on a white background.
/// </summary>
public class MellowRainbow : IColormap
{
    public string Name => "Mellow Rainbow";
    readonly CustomInterpolated Colormap;
    public Color GetColor(double position) => Colormap.GetColor(position);

    public MellowRainbow()
    {
        Color[] colors = "#466be3 #29bbec #30f199 #edd03a #fb8023 #d23104"
            .Split(' ')
            .Select(x => new Color(x))
            .ToArray();

        Colormap = new(colors);
    }
}
