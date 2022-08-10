namespace ScottPlot.Palettes;

/// <summary>
/// Create a custom ScottPlot4 Palette from a collection of colors
/// </summary>
public class Custom : PaletteBase
{
    public Custom(string[] hexColors, string name = "", string description = "")
    {
        if (hexColors is null)
            throw new System.ArgumentNullException("must provide at least one color");

        if (hexColors.Length == 0)
            throw new System.ArgumentException("must provide at least one color");

        Colors = FromHexColors(hexColors);
        Name = name;
        Description = description;
    }
}
