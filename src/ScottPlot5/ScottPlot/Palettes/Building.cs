namespace ScottPlot.Palettes;

public class Building : PaletteBase
{
    protected override Color[] colors => Common.HexPalettes.Building.Colors.Select(c => Color.FromHex(c)).ToArray();
}
