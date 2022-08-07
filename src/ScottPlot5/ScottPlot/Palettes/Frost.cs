/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */
namespace ScottPlot.Palettes
{
    public class Frost : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.Frost.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
