namespace ScottPlot.Palettes
{
    public class Nero : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.Nero.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
