/* This color palette was sourced from the examples provided in:
 * Wong 2011, https://www.nature.com/articles/nmeth.1618.pdf
 * This 8-color palette has good overall variability and can be 
 * differentiated by individuals with red-green color blindness.
 */
namespace ScottPlot.Drawing.Colorsets
{
    public class ColorblindFriendly : HexColorset, IPalette
    {
        public override string[] hexColors => Common.HexPalettes.ColorblindFriendly.Colors;
    }
}
