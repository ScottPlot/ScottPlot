/* Sourced from NordConEmu:
 * https://github.com/arcticicestudio/nord-conemu
 * Seems to be an extended version of Aurora
 */
namespace ScottPlot.Palettes
{
    public class Nord : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.Nord.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
