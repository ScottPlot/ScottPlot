namespace ScottPlot.Palettes;

public class Penumbra : PaletteBase
{
    protected override Color[] colors => Common.HexPalettes.Penumbra.Colors.Select(c => Color.FromHex(c)).ToArray();
}
