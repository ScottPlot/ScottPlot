/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */
namespace ScottPlot.Palettes
{
    public class PolarNight : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.PolarNight.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
