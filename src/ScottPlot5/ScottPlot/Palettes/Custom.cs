namespace ScottPlot.Palettes;

/// <summary>
/// Create a custom ScottPlot5 Palette from a collection of colors
/// </summary>
public class Custom : Palette
{
    public Custom(string[] hexColors, string name = "", string description = "") : base(hexColors, name, description) { }
    public Custom(Color[] colors, string name = "", string description = "") : base(colors, name, description) { }
}