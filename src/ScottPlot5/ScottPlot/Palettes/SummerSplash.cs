namespace ScottPlot.Palettes
{
    public class SummerSplash : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.SummerSplash.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
