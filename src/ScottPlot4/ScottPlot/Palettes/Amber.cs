namespace ScottPlot.Palettes;

public class Amber : PaletteBase, IPalette
{
    public Amber()
    {
        ISharedPalette shared = new SharedPalettes.Amber();
        Colors = shared.Colors.ToSDColors();
        Name = shared.Title;
        Description = shared.Description;
    }
}
