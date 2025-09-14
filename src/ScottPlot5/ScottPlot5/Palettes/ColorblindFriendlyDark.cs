namespace ScottPlot.Palettes;

/// <summary>
/// Inverted version of <see cref="ColorblindFriendly"/> 
/// which is useful for plots with dark backgrounds
/// </summary>
public class ColorblindFriendlyDark : IPalette
{
    private readonly IPalette Palette = new ColorblindFriendly().Inverted();
    public string Name => Palette.Name;
    public string Description => Palette.Description;
    public Color[] Colors => Palette.Colors;
    public Color GetColor(int index) => Palette.GetColor(index);
}
