/* Sourced from Material Design
 * https://material.io/design/color/the-color-system.html
 */
namespace ScottPlot.Palettes
{
    public class Amber : PaletteBase
    {
        protected override Color[] colors => Common.HexPalettes.Amber.Colors.Select(c => Color.FromHex(c)).ToArray();
    }
}
