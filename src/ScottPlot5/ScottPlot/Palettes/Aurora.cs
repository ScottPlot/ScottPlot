/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */
namespace ScottPlot.Palettes
{
    public class Aurora : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.Aurora.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
