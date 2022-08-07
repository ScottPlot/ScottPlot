/* Sourced from Nord:
 * https://github.com/arcticicestudio/nord
 * https://www.nordtheme.com/docs/colors-and-palettes
 */
namespace ScottPlot.Palettes
{
    public class Snowstorm : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.Snowstorm.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
